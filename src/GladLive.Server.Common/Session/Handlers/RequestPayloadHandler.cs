using Common.Logging;
using GladNet.Engine.Common;
using GladNet.Message.Handlers;
using GladNet.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GladNet.Message;
using Easyception;

namespace GladLive.Server.Common
{
	/// <summary>
	/// <see cref="IRequestMessage"/> payload handler.
	/// </summary>
	/// <typeparam name="TPeerType">Type of peer this request was recieved on.</typeparam>
	/// <typeparam name="TPayloadType">Type of payload that this handler can handle.</typeparam>
	public abstract class RequestMessagePayloadHandler<TPeerType, TPayloadType> : IRequestMessageHandler<TPeerType>, IClassLogger
		where TPayloadType : PacketPayload
		where TPeerType : INetPeer
	{
		/// <summary>
		/// Class logging service for handlers.
		/// </summary>
		public ILog Logger { get; private set; }

		public RequestMessagePayloadHandler(ILog logger)
		{
			Throw<ArgumentNullException>.If.IsNull(logger)?.Now(nameof(logger), $"Must provide non-null logging instance.");

			Logger = logger;
		}

		/// <summary>
		/// Handles the <see cref="IRequestMessage"/> and specified <typeparamref name="TPayloadType"/>.
		/// </summary>
		/// <param name="payload">Packet payload to be handled.</param>
		/// <param name="parameters">Message parameters (null with Photon)</param>
		/// <param name="peer">The peer that recieved this <see cref="TPayloadType"/> payload.</param>
		protected abstract void OnIncomingHandlableMessage(IRequestMessage message, TPayloadType payload, IMessageParameters parameters, TPeerType peer);

		public bool TryProcessMessage(IRequestMessage message, IMessageParameters parameters, TPeerType peer)
		{
			TPayloadType payload = message.Payload.Data as TPayloadType;

			//if it's the not the payload type this handler handles then we
			//indicate non-consumption
			if (payload == null)
				return false;
			else
				OnIncomingHandlableMessage(message, payload, parameters, peer);

			//If an exception wasn't thrown we'll be indicating that the payload has been consumed.
			return true;
		}
	}
}
