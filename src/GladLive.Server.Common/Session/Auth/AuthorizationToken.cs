using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// A grantable authorization token for elevation control.
	/// </summary>
	public class AuthorizationToken
	{
		/// <summary>
		/// ID of the authorization.
		/// </summary>
		public Guid AuthID { get; }

		/// <summary>
		/// Owner of the token.
		/// </summary>
		public object TokenOwner { get; }

		public AuthorizationToken(object tokenOwener, Guid token)
		{
			TokenOwner = tokenOwener;
			AuthID = token;
		}
	}
}
