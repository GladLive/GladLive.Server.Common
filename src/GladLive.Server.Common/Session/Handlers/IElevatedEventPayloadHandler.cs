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
	public interface IElevatedEventPayloadHandler<in TPeerType, TPayloadType> : IElevatedEventPayloadHandler<TPeerType>, IPayloadHandler<TPeerType, TPayloadType>
		where TPeerType : INetPeer where TPayloadType : PacketPayload
	{

	}

	/// <summary>
	/// IoC/Meta-data Marker for elevated Event handlers.
	/// </summary>
	/// <typeparam name="TPeerType">Type of the peer.</typeparam>
	/// <typeparam name="TPayloadType">Type of the payload</typeparam>
	public interface IElevatedEventPayloadHandler<in TPeerType> : IPayloadHandler<TPeerType>
		where TPeerType : INetPeer
	{

	}
}
