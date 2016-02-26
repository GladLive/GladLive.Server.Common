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
	/// Handler for <see cref="AuthenticationRequest"/>s messages for <see cref="IElevatableSession"/> peers.
	/// </summary>
	/// <typeparam name="TPeerType">Peer that that implements <see cref="IElevatableSession"/>.</typeparam>
	[PayloadHandlerType(OperationType.Request, typeof(AuthenticationRequest))]
	public class AuthenticationRequestHandler<TPeerType> : IRequestPayloadHandler<TPeerType, AuthenticationRequest>, IClassLogger
		where TPeerType : IElevatableSession, INetPeer
	{
		public ILog Logger { get; }

		/// <summary>
		/// Authentication service the handler defers requests to.
		/// </summary>
		private IElevationAuthenticationService elevationAuthService { get; }

		public AuthenticationRequestHandler(ILog logger, IElevationAuthenticationService authService)
		{
			Logger = logger;
			elevationAuthService = authService;
		}

		//explictly implement this
		bool IPayloadHandler<TPeerType>.TryProcessPayload(PacketPayload payload, IMessageParameters parameters, TPeerType peer)
		{
			//Try cast and if it's not null then we can call the more derived version
			AuthenticationRequest request = payload as AuthenticationRequest;

			if (request == null)
				return false;
			else
			{
				return this.TryProcessPayload(request, parameters, peer);
			}
		}

		public bool TryProcessPayload(AuthenticationRequest payload, IMessageParameters parameters, TPeerType peer)
		{
			Logger.DebugFormat("{0} session is requesting elevation. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);

			//Check if we were sent the expected valid signed message for authentication/elevation
			if (elevationAuthService.TryAuthenticate(peer, payload.Message))
			{
				Logger.DebugFormat("{0} session is now elevated. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);
			}
			else
				Logger.WarnFormat("{0} session failed elevation. ID {1}", peer.GetType(), peer.PeerDetails.ConnectionID);

			return true;
		}
	}
}
