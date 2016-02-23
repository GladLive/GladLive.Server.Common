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
		bool TryAuthenticate(IElevatableSession session, AuthenticationMessage authMessage);
	}
}
