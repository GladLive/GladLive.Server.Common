using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet.Engine.Common;
using GladNet.Message;
using GladLive.Common;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Handler for <see cref="AuthTokenRequestHandler"/>s messages for <see cref="IElevatableSession"/> peers.
	/// </summary>
	/// <typeparam name="TPeerType">Peer that that implements <see cref="IElevatableSession"/>.</typeparam>
	public class AuthTokenRequestHandler<TPeerType> : RequestMessagePayloadHandler<TPeerType, AuthTokenRequest>
		where TPeerType : IElevatableSession, INetPeer
	{
		/// <summary>
		/// Authentication service the handler defers requests to.
		/// </summary>
		private IElevationAuthenticationService elevationAuthService { get; }

		public AuthTokenRequestHandler(ILog logger, IElevationAuthenticationService authService)
			: base(logger)
		{
			elevationAuthService = authService;
		}

		protected override void OnIncomingHandlableMessage(IRequestMessage message, AuthTokenRequest payload, IMessageParameters parameters, TPeerType peer)
		{
			Logger.Debug($"{peer?.GetType()} session is requesting an auth token. ID {peer?.PeerDetails?.ConnectionID}");

			//Ask for a new token and associate it with the requesting session
			Guid token = elevationAuthService.RequestSingleUseToken(peer);

			//Send the token to the session
			peer.TrySendMessage(OperationType.Response, new AuthTokenResponse(token));
		}
	}
}
