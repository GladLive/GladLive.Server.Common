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
	/// Wire ready message that sends an auth token that should be signed.
	/// </summary>
	[GladNetSerializationContract]
	[GladNetSerializationInclude((int)PayloadNumber.AuthTokenResponse, typeof(PacketPayload), false)]
	public class AuthTokenResponse : PacketPayload, IStaticPayloadParameters
	{
		/// <summary>
		/// Authentication token that should be signed by a peer.
		/// </summary>
		[GladNetMember(1)]
		public Guid AuthToken { get; }

		public byte Channel { get { return 0; } }

		public DeliveryMethod DeliveryMethod { get { return DeliveryMethod.ReliableOrdered; } }

		public bool Encrypted { get { return true; } }

		public AuthTokenResponse(Guid authToken)
		{
			AuthToken = authToken;
		}

		/// <summary>
		/// Protobuf-net constructor
		/// </summary>
		protected AuthTokenResponse()
		{

		}
	}
}
