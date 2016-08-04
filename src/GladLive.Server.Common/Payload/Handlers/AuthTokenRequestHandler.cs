using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet.Engine.Common;
using GladNet.Message;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Handler for <see cref="AuthenticationRequest"/>s messages for <see cref="IElevatableSession"/> peers.
	/// </summary>
	/// <typeparam name="TPeerType">Peer that that implements <see cref="IElevatableSession"/>.</typeparam>
	public class AuthenticationRequestHandler<TPeerType> : RequestMessagePayloadHandler<TPeerType, AuthenticationRequest>
		where TPeerType : IElevatableSession, INetPeer
	{
		/// <summary>
		/// Authentication service the handler defers requests to.
		/// </summary>
		private IElevationAuthenticationService elevationAuthService { get; }

		public AuthenticationRequestHandler(ILog logger, IElevationAuthenticationService authService)
			: base(logger)
		{
			elevationAuthService = authService;
		}

		protected override void OnIncomingHandlableMessage(IRequestMessage message, AuthenticationRequest payload, IMessageParameters parameters, TPeerType peer)
		{
			//TODO: Network the response

			Logger.Debug($"{peer?.GetType()} session is requesting elevation. ID {peer?.PeerDetails?.ConnectionID}");

			//Check if we were sent the expected valid signed message for authentication/elevation
			if (elevationAuthService.TryAuthenticate(peer, payload.Message))
			{
				Logger.Debug($"{peer?.GetType()} session is now elevated. ID {peer?.PeerDetails?.ConnectionID}");
			}
			else
				Logger.Warn($"{peer?.GetType()} session failed elevation. ID {peer?.PeerDetails?.ConnectionID}");
		}
	}
}
