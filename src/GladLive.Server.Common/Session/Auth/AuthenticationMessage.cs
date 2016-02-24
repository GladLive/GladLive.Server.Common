using GladNet.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	[GladNetSerializationContract]
	public class AuthenticationMessage
	{
		/// <summary>
		/// The byte[] value of the signed message
		/// </summary>
		[GladNetMember(1)]
		public byte[] SignedMessage { get; }

		public AuthenticationMessage(byte[] signedMessage)
		{
			SignedMessage = signedMessage;
		}

		/// <summary>
		/// Protobuf-net constructor
		/// </summary>
		protected AuthenticationMessage()
		{

		}
	}
}
