using GladLive.Common;
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
		public static void Test_Handler_Returns_False_On_Null_Payload()
		{
			//arrange
			RequestPayloadHandlerTest testHandler = new RequestPayloadHandlerTest(Mock.Of<ILog>());

			//assert
			Assert.False(testHandler.TryProcessPayload(null, Mock.Of<IMessageParameters>(), Mock.Of<INetPeer>()));
		}

		[Test]
		public static void Test_Handler_Calls_Protected_Payload_Handling_Method()
		{
			//arrange
			Mock<RequestPayloadHandlerTest> testHandler = new Mock<RequestPayloadHandlerTest>(MockBehavior.Loose);

			PacketPayload testPayload = Mock.Of<PacketPayload>();
			IMessageParameters testParameters = Mock.Of<IMessageParameters>();
			INetPeer testNetPeer = Mock.Of<INetPeer>();

			//act: Call the handler's process method
			testHandler.Object.TryProcessPayload(testPayload, testParameters, testNetPeer);

			//assert: Check to see if internal was called
			testHandler.Protected().Verify("HandleNonNullStronglyTypedPayload", Times.Once(), testPayload, testParameters, testNetPeer);
		}

		public class RequestPayloadHandlerTest : RequestPayloadHandler<INetPeer, PacketPayload>
		{
			public RequestPayloadHandlerTest()
				: base (Mock.Of<ILog>())
			{

			}

			public RequestPayloadHandlerTest(ILog logger) : base(logger)
			{
			}

			protected override void HandleNonNullStronglyTypedPayload(PacketPayload payload, IMessageParameters parameters, INetPeer peer)
			{
				
			}
		}
	}
}
