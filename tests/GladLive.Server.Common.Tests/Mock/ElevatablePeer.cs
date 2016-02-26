using GladNet.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common.Tests
{
	public class ElevatablePeer : INetPeer, IElevatableSession
	{
		public INetworkMessageSender NetworkSendService
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IConnectionDetails PeerDetails
		{
			get
			{
				Mock<IConnectionDetails> details = new Mock<IConnectionDetails>();

				details.SetupGet(x => x.RemoteIP)
					.Returns(IPAddress.Any);

				return details.Object;
			}
		}

		public NetStatus Status
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Guid UniqueAuthToken
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public bool CanSend(OperationType opType)
		{
			throw new NotImplementedException();
		}
	}
}
