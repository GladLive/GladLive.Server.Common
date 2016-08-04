using Common.Logging;
using GladLive.Server.Common;
using GladNet.Common;
using GladNet.Message;
using GladNet.Message.Handlers;
using GladNet.Payload;
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
			Assert.DoesNotThrow(() => new ElevatedSessionMessageHandlerStrategyDecorator<ElevatablePeer, RequestMessage>(Mock.Of<ILog>(), Mock.Of<IElevationVerificationService>(), Mock.Of<ChainMessageHandlerStrategy<ElevatablePeer, RequestMessage>>()));
		}

		[Test]
		public static void Test_Doesnt_Call_Handlers_If_Auth_Services_Indicates_Unauthed()
		{
			//arrange: Setup the services and init the handler
			Mock<IElevationVerificationService> verification = new Mock<IElevationVerificationService>();
			Mock<ChainMessageHandlerStrategy<ElevatablePeer, RequestMessage>> handlers = new Mock<ChainMessageHandlerStrategy<ElevatablePeer, RequestMessage>>();

			verification.Setup(x => x.isElevated(It.IsAny<IElevatableSession>()))
				.Returns(false);

			ElevatedSessionMessageHandlerStrategyDecorator<ElevatablePeer, RequestMessage> handler = new ElevatedSessionMessageHandlerStrategyDecorator<ElevatablePeer, RequestMessage>(Mock.Of<ILog>(), verification.Object, handlers.Object);

			//act
			bool result = handler.TryProcessMessage(new RequestMessage(Mock.Of<PacketPayload>()), Mock.Of<IMessageParameters>(), new ElevatablePeer());

			//assert
			Assert.False(result);
			//Make sure the handlers weren't called
			handlers.Verify(x => x.TryProcessMessage(It.IsAny<RequestMessage>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()), Times.Never());
		}

		[Test]
		public static void Test_Calls_Handlers_If_Auth_Services_Indicates_Authed()
		{
			//arrange: Setup the services and init the handler
			Mock<IElevationVerificationService> verification = new Mock<IElevationVerificationService>();
			Mock<ChainMessageHandlerStrategy<ElevatablePeer, IRequestMessage>> handlers = new Mock<ChainMessageHandlerStrategy<ElevatablePeer, IRequestMessage>>();

			verification.Setup(x => x.isElevated(It.IsAny<IElevatableSession>()))
				.Returns(true);

			handlers.Setup(x => x.TryProcessMessage(It.IsAny<IRequestMessage>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()))
				.Returns(true);

			ElevatedSessionMessageHandlerStrategyDecorator<ElevatablePeer, IRequestMessage> handler = new ElevatedSessionMessageHandlerStrategyDecorator<ElevatablePeer, IRequestMessage>(Mock.Of<ILog>(), verification.Object, handlers.Object);

			//act
			bool result = handler.TryProcessMessage(Mock.Of<IRequestMessage>(), Mock.Of<IMessageParameters>(), new ElevatablePeer());

			//assert
			Assert.True(result);
			//Make sure the handlers were called
			handlers.Verify(x => x.TryProcessMessage(It.IsAny<IRequestMessage>(), It.IsAny<IMessageParameters>(), It.IsAny<ElevatablePeer>()), Times.Once());
		}
	}
}
