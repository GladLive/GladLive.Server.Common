using Common.Logging;
using GladLive.Security.Common;
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
	public static class ElevationManagerTests
	{
		[Test]
		public static void Test_Ctor_Doesnt_Throw()
		{
			//arrange
			Assert.DoesNotThrow(() => new ElevationManager(Mock.Of<ILog>(), Mock.Of<ISigningService>()));
		}

		[Test]
		public static void Test_Returns_False_On_Expected_Unelevated_Session()
		{
			//arrange
			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), Mock.Of<ISigningService>());

			//assrt
			Assert.False(elevationManager.isElevated(Mock.Of<IElevatableSession>()));
		}


		[Test]
		public static void Test_Returns_False_After_Failed_Authentication_Without_Valid_Token()
		{
			//arrange
			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), Mock.Of<ISigningService>());
			IElevatableSession session = Mock.Of<IElevatableSession>();

			//act:Preform a failed authentication
			elevationManager.TryAuthenticate(session, Mock.Of<AuthenticationMessage>());

			//assrt
			Assert.False(elevationManager.isElevated(session));
		}

		[Test]
		public static void Test_Returns_False_After_Failed_Authentication_With_Valid_Toke()
		{
			//arrange
			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), Mock.Of<ISigningService>());
			IElevatableSession session = Mock.Of<IElevatableSession>();

			session.UniqueAuthToken = elevationManager.RequestSingleUseToken(session);

			//act:Preform a failed authentication
			elevationManager.TryAuthenticate(session, Mock.Of<AuthenticationMessage>());

			//assrt
			Assert.False(elevationManager.isElevated(session));
		}

		[Test]
		public static void Test_Returns_True_After_Sucessful_Authentication()
		{
			//arrange
			Mock<ISigningService> signingService = new Mock<ISigningService>();

			signingService.Setup(x => x.isSigned(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
				.Returns(true);

			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), signingService.Object);
			IElevatableSession session = Mock.Of<IElevatableSession>();
			AuthenticationMessage message = new AuthenticationMessage(new byte[0]);

			session.UniqueAuthToken = elevationManager.RequestSingleUseToken(session);

			//act:Preform a authentication
			elevationManager.TryAuthenticate(session, message);

			//assrt
			Assert.True(elevationManager.isElevated(session));
		}

		[Test]
		public static void Test_Returns_False_After_Sucessful_Authentication_And_Then_Removal()
		{
			//arrange
			Mock<ISigningService> signingService = new Mock<ISigningService>();

			signingService.Setup(x => x.isSigned(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
				.Returns(true);

			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), signingService.Object);
			IElevatableSession session = Mock.Of<IElevatableSession>();
			AuthenticationMessage message = new AuthenticationMessage(new byte[0]);

			session.UniqueAuthToken = elevationManager.RequestSingleUseToken(session);

			//act:Preform a authentication
			elevationManager.TryAuthenticate(session, message);

			//now revoke it
			elevationManager.TryRevokeAuthentication(session);

			//assrt
			Assert.False(elevationManager.isElevated(session));
		}

		[Test]
		public static void Test_Returns_False_For_Session_Who_Stole_Others_Token()
		{
			//arrange
			Mock<ISigningService> signingService = new Mock<ISigningService>();

			signingService.Setup(x => x.isSigned(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
				.Returns(true);

			ElevationManager elevationManager = new ElevationManager(Mock.Of<ILog>(), signingService.Object);
			IElevatableSession session = Mock.Of<IElevatableSession>();
			AuthenticationMessage message = new AuthenticationMessage(new byte[0]);

			session.UniqueAuthToken = elevationManager.RequestSingleUseToken(session);

			//act:Preform a authentication
			elevationManager.TryAuthenticate(session, message);

			//assert
			Assert.True(elevationManager.isElevated(session));

			//Should fail because this session isn't authorized with this token
			IElevatableSession criminalSession = Mock.Of<IElevatableSession>();
			criminalSession.UniqueAuthToken = session.UniqueAuthToken;

			Assert.False(elevationManager.isElevated(criminalSession));
		}
	}
}
