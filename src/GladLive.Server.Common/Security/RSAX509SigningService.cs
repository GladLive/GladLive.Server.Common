using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	public class RSAX509SigningService : ISigningService
	{
		private X509Certificate2 cert { get; }

		public RSAX509SigningService(string certPath)
		{
			cert = new X509Certificate2(certPath);
		}

		public RSAX509SigningService(X509Certificate2 certificate)
		{
			cert = certificate;
		}

		public bool isSigned(byte[] message, byte[] signedMessage)
		{
			using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
			{
				provider.FromXmlString(cert.PrivateKey.ToXmlString(false));

				return provider.VerifyData(message, CryptoConfig.MapNameToOID("SHA256"), signedMessage);
			}
		}

		public bool isSigned(string message, byte[] signedMessage)
		{
			return isSigned(Encoding.ASCII.GetBytes(message), signedMessage);
		}

		public byte[] SignMessage(byte[] message)
		{
			using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
			{
				provider.FromXmlString(cert.PrivateKey.ToXmlString(true));

				return provider.SignData(message, CryptoConfig.MapNameToOID("SHA256"));
			}
		}

		public byte[] SignMessage(string message)
		{
			return SignMessage(Encoding.ASCII.GetBytes(message));
		}
	}
}
