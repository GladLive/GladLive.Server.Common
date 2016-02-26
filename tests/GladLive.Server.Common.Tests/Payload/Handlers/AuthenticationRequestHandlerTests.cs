using Common.Logging;
using GladNet.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	[TestFixture]
	public static class AuthenticationRequestHandlerTests
	{
		[Test]
		public static void Test_Ctor_Doesnt_Throw()
		{
			//arrange
			Assert.DoesNotThrow(() => new AuthenticationRequestHandler<ElevatablePeer>(Mock.Of<ILog>(), Mock.Of<IElevationAuthenticationService>()));
		}

		[Test]
		public static void Test_Handler_Deffers_To_Auth_Service()
		{
			//arrange
			Mock<IElevationAuthenticationService> authService = new Mock<IElevationAuthenticationService>();
			AuthenticationRequestHandler<ElevatablePeer> handler = new AuthenticationRequestHandler<ElevatablePeer>(Mock.Of<ILog>(), authService.Object);
			ElevatablePeer peer = new ElevatablePeer();

			//act
			bool result = handler.TryProcessPayload(Mock.Of<AuthenticationRequest>(), Mock.Of<IMessageParameters>(), peer);

			//assert
			Assert.IsTrue(result);
			authService.Verify(x => x.TryAuthenticate(peer, It.IsAny<AuthenticationMessage>()), Times.Once());
		}
	}
}
