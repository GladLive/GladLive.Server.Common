using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Service that issues authorization tokens and elevates the privileges of
	/// <see cref="IElevatableSession"/>s.
	/// </summary>
	public interface IElevationAuthenticationService
	{
		/// <summary>
		/// Tries to authenticate/elevate the <paramref name="session"/> with the <paramref name="authMessage"/>.
		/// </summary>
		/// <param name="session">Session attempting to authenticate.</param>
		/// <param name="authMessage">Auth message.</param>
		/// <returns>True if the session authenticated.</returns>
		bool TryAuthenticate(IElevatableSession session, AuthenticationMessage authMessage);

		/// <summary>
		/// Tries to revoke auth status of the <paramref name="session"/>.
		/// </summary>
		/// <param name="session">Session to revoke for.</param>
		/// <returns>True if the session was found and unelevated.</returns>
		bool TryRevokeAuthentication(IElevatableSession session);

		/// <summary>
		/// Grants a tracked token for a single given <see cref="IElevatableSession"/>.
		/// </summary>
		/// <param name="session">Session to grant the token for.</param>
		/// <returns>A new <see cref="Guid"/> token for the session</returns>
		Guid RequestSingleUseToken(IElevatableSession session);
	}
}
