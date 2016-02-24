using GladNet.Common;
using GladNet.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Wire ready message that requests an authentication token.
	/// </summary>
	[GladNetSerializationContract]
	[GladNetSerializationInclude((int)PayloadNumber.AuthTokenRequest, typeof(PacketPayload), false)]
	public class AuthTokenRequest
	{
		//We don't need to send any data here
		//The type of this message is enough to indicate what the request is for

		/// <summary>
		/// Creates an auth token request encoded by the type.
		/// </summary>
		public AuthTokenRequest()
		{

		}
	}
}
