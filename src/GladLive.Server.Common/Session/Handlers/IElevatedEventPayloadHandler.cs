using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladLive.Common.Payload
{
	/// <summary>
	/// IoC/Meta-data Marker for elevated Event handlers.
	/// </summary>
	/// <typeparam name="TPeerType">Type of the peer.</typeparam>
	/// <typeparam name="TPayloadType">Type of the payload</typeparam>
	public interface IElevatedEventPayloadHandler<in TPeerType, TPayloadType> : IElevatedEventPayloadHandler<TPeerType>, IEventPayloadHandler<TPeerType, TPayloadType>
		where TPeerType : INetPeer where TPayloadType : PacketPayload
	{

	}

	/// <summary>
	/// IoC/Meta-data Marker for elevated Event handlers.
	/// </summary>
	/// <typeparam name="TPeerType">Type of the peer.</typeparam>
	/// <typeparam name="TPayloadType">Type of the payload</typeparam>
	public interface IElevatedEventPayloadHandler<in TPeerType> : IEventPayloadHandler<TPeerType>
		where TPeerType : INetPeer
	{

	}
}
