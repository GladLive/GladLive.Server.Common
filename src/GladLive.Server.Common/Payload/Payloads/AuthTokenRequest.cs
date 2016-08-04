using GladNet.Common;
using GladNet.Message;
using GladNet.Payload;
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
	[GladNetSerializationInclude((GladNetIncludeIndex)PayloadNumber.AuthTokenRequest, typeof(PacketPayload), false)]
	public class AuthTokenRequest : PacketPayload, IStaticPayloadParameters
	{
		//We don't need to send any data here
		//The type of this message is enough to indicate what the request is for

		public byte Channel { get { return 0; } }

		public DeliveryMethod DeliveryMethod { get { return DeliveryMethod.ReliableOrdered; } }

		public bool Encrypted { get { return true; } }

		/// <summary>
		/// Creates an auth token request encoded by the type.
		/// </summary>
		public AuthTokenRequest()
		{

		}
	}
}
