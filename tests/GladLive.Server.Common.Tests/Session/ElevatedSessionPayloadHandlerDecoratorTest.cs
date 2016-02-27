using Common.Logging;
using GladLive.Common;
using GladLive.Server.Common;
using GladNet.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common.Tests
{
	[TestFixture]
	public static class ElevatedSessionPayloadHandlerTest
	{
		[Test]
		public static void Test_Ctor_Doesnt_Throw()
		{
			//assert
			Assert.DoesNotThrow(() => new ElevatedSessionChainPayloadHandlerStrategyDecorator<ElevatablePeer>(Mock.Of<ILog>(), Mock.Of<IElevationVerificationService>(), Mock.Of<ChainPayloadHandler<ElevatablePeer>>()));
		}

		[Test]
		public static void Test_Doesnt_Call_Handlers_If_Auth_Services_Indicates_Unauthed()
		{
			//arrange: Setup the services and init the handler
			Mock<IElevationVerificationService> verification = new Mock<IElevationVerificationService>();
			Mock<ChainPayloadHandler<ElevatablePeer>> handlers = new Mock<ChainPayloadHandler<ElevatablePeer>>();

			verification.Setup(x => x.isElevated(It.IsAny<IElevatableSession>()))
				.Returns(false);

			ElevatedSessionChainPayloadHandlerStrategyDecorator<ElevatablePeer> handler = new ElevatedSessionChainPayloadHandlerStrategyDecorator<ElevatablePeer>(Mock.Of<ILog>(), verification.Object, handlers.Object);

			//act
			bool result = handler.TryProcessPayload(Mock.Of<PacketPayload>(), Mock.Of<IMessageParameters>(), new ElevatablePeer());

			//assert
			Assert.False(result);
			//Make sure the handlers weren't called
			handlers.Verify(x => x.TryProcessPayload(It.IsAny<PacketPayload>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()), Times.Never());
		}

		[Test]
		public static void Test_Calls_Handlers_If_Auth_Services_Indicates_Authed()
		{
			//arrange: Setup the services and init the handler
			Mock<IElevationVerificationService> verification = new Mock<IElevationVerificationService>();
			Mock<ChainPayloadHandler<ElevatablePeer>> handlers = new Mock<ChainPayloadHandler<ElevatablePeer>>();

			verification.Setup(x => x.isElevated(It.IsAny<IElevatableSession>()))
				.Returns(true);

			handlers.Setup(x => x.TryProcessPayload(It.IsAny<PacketPayload>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()))
				.Returns(true);

			ElevatedSessionChainPayloadHandlerStrategyDecorator<ElevatablePeer> handler = new ElevatedSessionChainPayloadHandlerStrategyDecorator<ElevatablePeer>(Mock.Of<ILog>(), verification.Object, handlers.Object);

			//act
			bool result = handler.TryProcessPayload(Mock.Of<PacketPayload>(), Mock.Of<IMessageParameters>(), new ElevatablePeer());

			//assert
			Assert.True(result);
			//Make sure the handlers were called
			handlers.Verify(x => x.TryProcessPayload(It.IsAny<PacketPayload>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()), Times.Once());
		}
	}
}
