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
		/// <summary>
		/// Checks the elevation status of the <paramref name="session"/>.
		/// </summary>
		/// <param name="session">Instance of the session to check</param>
		/// <returns>True if the session is elevated.</returns>
		bool isElevated(IElevatableSession session);
	}
}
