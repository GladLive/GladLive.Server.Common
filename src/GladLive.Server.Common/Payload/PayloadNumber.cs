using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	public enum PayloadNumber : int
	{
		AuthTokenRequest = 255,
		AuthTokenResponse = 256,
		AuthenticationRequest = 257
	}
}
