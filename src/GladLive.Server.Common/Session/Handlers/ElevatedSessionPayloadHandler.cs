using GladLive.Common;
using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Chain of responsibility payload handling semantics with elevation required for handling.
	/// </summary>
	/// <typeparam name="TSessionType">Elevatable session type.</typeparam>
	public class ElevatedSessionPayloadHandler<TSessionType> : ChainPayloadHandler<TSessionType>, IClassLogger
		where TSessionType : IElevatableSession, INetPeer
	{
		public ILog Logger { get; }

		//As odd as it may sound sessions could be lying about their status.
		//They could be maliciously created by unknown exploits and this adds
		//one more layer of security. Also, we could have accidently elevated them
		//in some way with an invalid token so this service is the end-all authority on elevation
		/// <summary>
		/// Service that can verify if sessions are truly authenticated.
		/// </summary>
		private IElevationVerificationService verificationService;

		public ElevatedSessionPayloadHandler(ILog logger, IElevationVerificationService verifyService)
		{
			Logger = logger;
			verificationService = verifyService;
		}

		public override bool TryProcessPayload(PacketPayload payload, IMessageParameters parameters, TSessionType peer)
		{
			//Very important to check elevation status
			if(verificationService.isElevated(peer.Token, peer))
			{
				return base.TryProcessPayload(payload, parameters, peer);
			}
			else
			{
				Logger.WarnFormat("PeerType {0} with IP {1} tried to have elevated message handled.", peer.GetType(), peer.PeerDetails.RemoteIP.ToString());
				return false;
			}
			
		}
	}
}
