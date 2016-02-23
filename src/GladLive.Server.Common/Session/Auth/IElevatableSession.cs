using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Session that can be elevated.
	/// </summary>
	public interface IElevatableSession
	{
		/// <summary>
		/// Token used for elevated priviliges.
		/// </summary>
		AuthorizationToken Token { get; set; }

		/// <summary>
		/// Indicates if the session has elevated.
		/// </summary>
		bool isElevated { get; }
	}
}
