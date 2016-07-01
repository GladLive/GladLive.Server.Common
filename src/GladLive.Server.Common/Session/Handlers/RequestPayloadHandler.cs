using Common.Logging;
using GladLive.Common;
using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Consolidated functionality for common <see cref="IRequestPayloadHandler{TPeerType, TPayloadType}"/>
	/// </summary>
	/// <typeparam name="TPeerType">Type of peer this event was recieved on.</typeparam>
	/// <typeparam name="TPayloadType">Type of payload that this handler can handle.</typeparam>
	public abstract class RequestPayloadHandler<TPeerType, TPayloadType> : IRequestPayloadHandler<TPeerType, TPayloadType>, IClassLogger
		where TPayloadType : PacketPayload
		where TPeerType : INetPeer
	{
		/// <summary>
		/// Class logging service for handlers.
		/// </summary>
		public ILog Logger { get; private set; }

		public RequestPayloadHandler(ILog logger)
		{
			Logger = logger;
		}

		/// <summary>
		/// Attempts to handle the loosely-typed payload.
		/// Will return false if <see cref="PacketPayload"/> isn't a <typeparamref name="TPayloadType"/>.
		/// </summary>
		/// <param name="payload">Packet payload to be handled.</param>
		/// <param name="parameters">Message parameters (null with Photon)</param>
		/// <param name="peer">The peer that recieved this <see cref="PacketPayload"/>.</param>
		/// <returns>True if the payload was handled. False if it was not.</returns>
		bool IPayloadHandler<TPeerType>.TryProcessPayload(PacketPayload payload, IMessageParameters parameters, TPeerType peer)
		{
			//Just try to as cast it and chek null in the other
			return TryProcessPayload(payload as TPayloadType, parameters, peer);
		}

		/// <summary>
		/// Attempts to handle the <typeparamref name="TPayloadType"/>.
		/// </summary>
		/// <param name="payload">Packet payload to be handled.</param>
		/// <param name="parameters">Message parameters (null with Photon)</param>
		/// <param name="peer">The peer that recieved this <see cref="TPayloadType"/> payload.</param>
		/// <returns>True if the payload was handled. False if it was not.</returns>
		public bool TryProcessPayload(TPayloadType payload, IMessageParameters parameters, TPeerType peer)
		{
			if (payload == null)
				return false;
			else
				HandleNonNullStronglyTypedPayload(payload, parameters, peer);

			//at this point it SHOULD have been handled since we can handle this type
			return true;
		}

		/// <summary>
		/// Handlesthe <typeparamref name="TPayloadType"/>.
		/// </summary>
		/// <param name="payload">Packet payload to be handled.</param>
		/// <param name="parameters">Message parameters (null with Photon)</param>
		/// <param name="peer">The peer that recieved this <see cref="TPayloadType"/> payload.</param>
		protected abstract void HandleNonNullStronglyTypedPayload(TPayloadType payload, IMessageParameters parameters, TPeerType peer);
	}
}
