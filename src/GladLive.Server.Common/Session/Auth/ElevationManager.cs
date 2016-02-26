using Common.Logging;
using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Manager that implements the various elevation and authentication services involving <see cref="IElevatableSession"/>s.
	/// </summary>
	public class ElevationManager : IElevationAuthenticationService, IElevationVerificationService,
		IClassLogger
	{
		/// <summary>
		/// Simple construct that manages a session and its simple boolean state of authentication
		/// </summary>
		protected class ManagedSession
		{
			public IElevatableSession Session { get; }

			public bool isAuthenticated { get; set; }

			public ManagedSession(IElevatableSession session, bool authenticated)
			{
				Session = session;
				isAuthenticated = authenticated;
			}
		}

		public ILog Logger { get; }

		/// <summary>
		/// Internal locking mechanism for reading and writing data inside of the class
		/// </summary>
		private ReaderWriterLockSlim lockObj { get; }

		/// <summary>
		/// Signing services used to authenticate sessions.
		/// </summary>
		private ISigningService signingService;

		/// <summary>
		/// Map of currently managed session with the auth token guid as the key.
		/// </summary>
		private IDictionary<Guid, ManagedSession> authTokenMap { get; }

		public ElevationManager(ILog logger, ISigningService signService)
		{
			Logger = logger;

			lockObj = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			authTokenMap = new Dictionary<Guid, ManagedSession>();

			signingService = signService;
		}

		public bool isElevated(IElevatableSession session)
		{
			//We're reading so we need a lock
			//We could reduce contention if this bottlenecks something
			lockObj.EnterReadLock();
			try
			{
				//If the session has a token we granted
				if (authTokenMap.ContainsKey(session.UniqueAuthToken))
				{
					//We need to check if the token was stolen
					if (authTokenMap[session.UniqueAuthToken].Session != session)
					{
						Logger.WarnFormat("Session {0} tried to check elevation with Token {1} when it doesn't belong to the session.", session.ToString(), session.UniqueAuthToken);
						return false;
					}

					//If we're managing the session check if it's authenticated
					//This will mean it's also elvated
					return authTokenMap[session.UniqueAuthToken].isAuthenticated;
				}
			}
			finally
			{
				lockObj.ExitReadLock();
			}


			Logger.DebugFormat("Session {0} had elevation checked but failed.", session.ToString());

			return false;
		}

		/// <summary>
		/// Tries to authenticate/elevate the <paramref name="session"/> with the <paramref name="authMessage"/>.
		/// </summary>
		/// <param name="session">Session attempting to authenticate.</param>
		/// <param name="authMessage">Auth message.</param>
		/// <returns>True if the session authenticated.</returns>
		public bool TryAuthenticate(IElevatableSession session, AuthenticationMessage authMessage)
		{
			Logger.DebugFormat("Authenticated requested for Session {0}.", session.ToString());

			//We reduce contention by only read locking for a short moment

			lockObj.EnterReadLock();
			try
			{
				//Check if this is a token we granted
				if (!authTokenMap.ContainsKey(session.UniqueAuthToken))
				{
					Logger.WarnFormat("Session {0} tried to authenticate with Token {1} but that token was not issued.", session.ToString(), session.UniqueAuthToken);
					return false;
				}

				//Check if the session matches the token
				if (authTokenMap[session.UniqueAuthToken].Session != session)
				{
					Logger.WarnFormat("Session {0} tried to authenticate with Token {1} but that token was not issued for that session. Was issued for {2}.", session.ToString(), session.UniqueAuthToken, authTokenMap[session.UniqueAuthToken].Session);
					return false;
				}
			}
			finally
			{
				lockObj.ExitReadLock();
			}

			bool result = HandleAuthentication(session.UniqueAuthToken.ToByteArray(), authMessage.SignedMessage);

			if (!result)
				return false;

			AddAuthenticatedSession(session.UniqueAuthToken, session);

			return true;
		}

		private void AddAuthenticatedSession(Guid token, IElevatableSession session)
		{
			lockObj.EnterWriteLock();
			try
			{
				authTokenMap[token].isAuthenticated = true;
			}
			finally
			{
				Logger.WarnFormat("Authenticated Session {0} with Token {1}.", session.ToString(), token.ToString());
				lockObj.ExitWriteLock();
			}
		}

		private bool HandleAuthentication(byte[] expectedMessage, byte[] signedMessage)
		{
			//If the signed message we were sent was null then it obviously fails
			if (signedMessage == null)
			{
				Logger.Warn("Session tried to authenticate with a null signed token.");
				return false;
			}

			Logger.DebugFormat("About to check signed message.");

			//Ask the signing service if the token has been signed
			return signingService.isSigned(expectedMessage, signedMessage);
		}


		/// <summary>
		/// Tries to revoke auth status of the <paramref name="session"/>.
		/// </summary>
		/// <param name="session">Session to revoke for.</param>
		/// <returns>True if the session was found and unelevated.</returns>
		public bool TryRevokeAuthentication(IElevatableSession session)
		{
			lockObj.EnterWriteLock();
			try
			{
				//We have to find the session key
				//Don't trust the session instance to have a valid key
				Guid? key = authTokenMap.Where(x => x.Value.Session == session)
					.Select(x => (Nullable<Guid>)x.Key)
					.FirstOrDefault();

				if (!key.HasValue)
					return false;
				else
					return authTokenMap.Remove(key.Value);

			}
			finally
			{
				Logger.WarnFormat("Finished removing Session {0} from Auth table.", session.ToString());
				lockObj.ExitWriteLock();
			}
		}

		/// <summary>
		/// Grants a tracked token for a single given <see cref="IElevatableSession"/>.
		/// </summary>
		/// <param name="session">Session to grant the token for.</param>
		/// <returns>A new <see cref="Guid"/> token for the session</returns>
		public Guid RequestSingleUseToken(IElevatableSession session)
		{
			lockObj.EnterWriteLock();
			try
			{
				Guid newToken = Guid.NewGuid();

				Logger.DebugFormat("Created new Guid {0} adding to managed sessions for Session {1}.", newToken.ToString(), session.ToString());

				authTokenMap.Add(newToken, new ManagedSession(session, false));

				session.UniqueAuthToken = newToken;

				return newToken;
			}
			finally
			{
				lockObj.ExitWriteLock();
			}
		}
	}
}
