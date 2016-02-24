using NUnit.Framework;
using Pluralsight.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common.Tests
{
	[TestFixture]
	public static class RSAX509SigningServiceTests
	{
		[Test]
		public static void Test_Sign_Message_Returns_Non_Null_Bytes()
		{
			//arrange
			using (CryptContext ctx = new CryptContext())
			{

				ctx.Open();

				X509Certificate2 cert = ctx.CreateSelfSignedCertificate(

				new SelfSignedCertProperties
				{

					IsPrivateKeyExportable = true,

					KeyBitLength = 4096,

					Name = new X500DistinguishedName("cn=localhost"),

					ValidFrom = DateTime.Today.AddDays(-1),

					ValidTo = DateTime.Today.AddYears(1)

				});

				//Create the signing service
				RSAX509SigningService signer = new RSAX509SigningService(cert);

				//act
				byte[] bytes = signer.SignMessage("hello");

				//assert
				Assert.NotNull(bytes);
				Assert.IsTrue(bytes.Count() != 0);
				Assert.AreNotSame(bytes, Encoding.ASCII.GetBytes("hello"));
			}
		}

		[Test]
		public static void Test_Sign_Message_Verifies_After_Signing()
		{
			//arrange
			using (CryptContext ctx = new CryptContext())
			{

				ctx.Open();

				X509Certificate2 cert = ctx.CreateSelfSignedCertificate(

				new SelfSignedCertProperties
				{

					IsPrivateKeyExportable = true,

					KeyBitLength = 4096,

					Name = new X500DistinguishedName("cn=localhost"),

					ValidFrom = DateTime.Today.AddDays(-1),

					ValidTo = DateTime.Today.AddYears(1)

				});

				//Create the signing service
				RSAX509SigningService signer = new RSAX509SigningService(cert);

				//act
				byte[] bytes = signer.SignMessage("hello");

				Assert.IsTrue(signer.isSigned("hello", bytes));
				Assert.False(signer.isSigned("this should fail", bytes));
			}
		}
	}
}
