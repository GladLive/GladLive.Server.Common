using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet.Message.Handlers;
using GladNet.Engine.Common;
using GladNet.Message;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Decorator for adding elevation requirements for payload handling.
	/// It will add elevation requirement semantics to <see cref="IPayloadHandler{TSessionType}"/>
	/// </summary>
	/// <typeparam name="TSessionType">Elevatable session type.</typeparam>
	public class ElevatedSessionMessageHandlerStrategyDecorator<TSessionType, TNetworkMessageType> : IMessageHandlerStrategy<TSessionType, TNetworkMessageType>
		where TSessionType : IElevatableSession, INetPeer
		where TNetworkMessageType : INetworkMessage
	{
		/// <summary>
		/// Public class logger instance.
		/// </summary>
		public ILog Logger { get; }

		//As odd as it may sound sessions could be lying about their status.
		//They could be maliciously created by unknown exploits and this adds
		//one more layer of security. Also, we could have accidently elevated them
		//in some way with an invalid token so this service is the end-all authority on elevation
		/// <summary>
		/// Service that can verify if sessions are truly authenticated.
		/// </summary>
		private IElevationVerificationService verificationService { get; }

		/// <summary>
		/// Internal chain payload handler strategy to decorate with elevation required semantics.
		/// </summary>
		private IMessageHandlerStrategy<TSessionType, TNetworkMessageType> decoratedStrategy { get; }

		public ElevatedSessionMessageHandlerStrategyDecorator(ILog logger, IElevationVerificationService verifyService, IMessageHandlerStrategy<TSessionType, TNetworkMessageType> handlerStrategyToDecorate)
		{
			//TODO: Check null-ness

			Logger = logger;
			verificationService = verifyService;
			decoratedStrategy = handlerStrategyToDecorate;
		}

		public bool TryProcessMessage(TNetworkMessageType message, IMessageParameters parameters, TSessionType peer)
		{
			//Decorates the internal strategy with auth/elevation semantics

			//Very important to check elevation status
			if (verificationService.isElevated(peer))
			{
				return decoratedStrategy.TryProcessMessage(message, parameters, peer);
			}
			else
			{
				Logger.Warn($"PeerType {peer?.GetType()} with IP {peer?.PeerDetails?.RemoteIP?.ToString()} tried to have elevated message handled.");
				return false;
			}
		}
	}
}
