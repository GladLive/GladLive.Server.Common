using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Service verifies that an <see cref="IElevatableSession"/> is truly authenticated
	/// and hasn't had its authentication revoked.
	/// </summary>
	public interface IElevationVerificationService
	{
		bool isElevated(AuthorizationToken token, IElevatableSession session);
	}
}
