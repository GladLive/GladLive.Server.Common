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
		/// The plaintext string of the authorization message.
		/// </summary>
		[GladNetMember(1)]
		public string AuthMessage { get; }

		/// <summary>
		/// The byte[] value of the signed <see cref="AuthMessage"/>.
		/// </summary>
		[GladNetMember(2)]
		public byte[] SignedMessage { get; }

		public AuthenticationMessage(string authMessage, byte[] signed)
		{
			AuthMessage = authMessage;
			SignedMessage = signed;
		}

		/// <summary>
		/// Protobuf-net constructor
		/// </summary>
		protected AuthenticationMessage()
		{

		}
	}
}
