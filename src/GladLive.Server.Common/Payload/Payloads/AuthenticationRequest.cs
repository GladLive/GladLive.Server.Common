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
	/// Wire ready authentication request.
	/// </summary>
	[GladNetSerializationContract]
	[GladNetSerializationInclude((int)PayloadNumber.AuthenticationRequest, typeof(PacketPayload), false)]
	public class AuthenticationRequest : PacketPayload, IStaticPayloadParameters
	{
		/// <summary>
		/// Authentication message to be used in the request.
		/// </summary>
		[GladNetMember(1, IsRequired = true)]
		public AuthenticationMessage Message { get; }

		/// <summary>
		/// Auth requests should be sent encrypted only.
		/// </summary>
		public bool Encrypted { get { return true; } }

		/// <summary>
		/// Auth requests have high priority so they are sent over the default channel.
		/// </summary>
		public byte Channel { get { return 0; } }

		/// <summary>
		/// Auth requests should be reliable and non-dropped.
		/// </summary>
		public DeliveryMethod DeliveryMethod { get { return DeliveryMethod.ReliableOrdered; } }

		/// <summary>
		/// Creates a new Auth request with the message used for authorization.
		/// </summary>
		/// <param name="message">Message to be used to determine authorization.</param>
		public AuthenticationRequest(AuthenticationMessage message)
		{
			Message = message;
		}

		/// <summary>
		/// Protobuf-net constructor
		/// </summary>
		protected AuthenticationRequest()
		{

		}
	}
}
