using GladLive.Common;
using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Handler for <see cref="AuthenticationRequest"/>s messages for <see cref="IElevatableSession"/> peers.
	/// </summary>
	/// <typeparam name="TPeerType">Peer that that implements <see cref="IElevatableSession"/>.</typeparam>
	[PayloadHandlerType(OperationType.Request, typeof(AuthenticationRequest))]
	public class AuthenticationRequestHandler<TPeerType> : RequestPayloadHandler<TPeerType, AuthenticationRequest>
		where TPeerType : IElevatableSession, INetPeer
	{
		/// <summary>
		/// Authentication service the handler defers requests to.
		/// </summary>
		private IElevationAuthenticationService elevationAuthService { get; }

		public AuthenticationRequestHandler(ILog logger, IElevationAuthenticationService authService)
			: base(logger)
		{
			elevationAuthService = authService;
		}

		protected override void HandleNonNullStronglyTypedPayload(AuthenticationRequest payload, IMessageParameters parameters, TPeerType peer)
		{
			//TODO: Network the response

			Logger.DebugFormat("{0} session is requesting elevation. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);

			//Check if we were sent the expected valid signed message for authentication/elevation
			if (elevationAuthService.TryAuthenticate(peer, payload.Message))
			{
				Logger.DebugFormat("{0} session is now elevated. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);
			}
			else
				Logger.WarnFormat("{0} session failed elevation. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);
		}
	}
}
