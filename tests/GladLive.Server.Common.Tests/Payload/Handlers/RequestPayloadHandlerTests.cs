using GladNet.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Protected;
using Common.Logging;
using GladNet.Message;
using GladNet.Engine.Common;
using GladNet.Payload;

namespace GladLive.Server.Common.Tests
{
	[TestFixture]
	public static class RequestPayloadHandlerTests
	{
		[Test]
		public static void Test_Ctor_Doesnt_Throw()
		{
			//assert
			Assert.DoesNotThrow(() => new RequestPayloadHandlerTest(Mock.Of<ILog>()));
		}

		[Test]
		public static void Test_Handler_Calls_Protected_Payload_Handling_Method()
		{
			//arrange
			Mock<RequestPayloadHandlerTest> testHandler = new Mock<RequestPayloadHandlerTest>(MockBehavior.Loose);

			PacketPayload testPayload = Mock.Of<PacketPayload>();
			IMessageParameters testParameters = Mock.Of<IMessageParameters>();
			INetPeer testNetPeer = Mock.Of<INetPeer>();
			RequestMessage message = new RequestMessage(testPayload);

			//act: Call the handler's process method
			testHandler.Object.TryProcessMessage(message, testParameters, testNetPeer);

			//assert: Check to see if internal was called
			testHandler.Protected().Verify("OnIncomingHandlableMessage", Times.Once(), message, testPayload, testParameters, testNetPeer);
		}

		public class RequestPayloadHandlerTest : RequestMessagePayloadHandler<INetPeer, PacketPayload>
		{
			public RequestPayloadHandlerTest()
				: base (Mock.Of<ILog>())
			{

			}

			public RequestPayloadHandlerTest(ILog logger) 
				: base(logger)
			{

			}

			protected override void OnIncomingHandlableMessage(IRequestMessage message, PacketPayload payload, IMessageParameters parameters, INetPeer peer)
			{
				
			}
		}
	}
}
