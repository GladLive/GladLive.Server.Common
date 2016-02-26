using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladLive.Common.Payload
{
	/// <summary>
	/// IoC/Meta-data Marker for elevated Request handlers.
	/// </summary>
	/// <typeparam name="TPeerType">Type of the peer.</typeparam>
	/// <typeparam name="TPayloadType">Type of the payload</typeparam>
	public interface IElevatedRequestPayloadHandler<in TPeerType, TPayloadType> : IElevatedRequestPayloadHandler<TPeerType>, IRequestPayloadHandler<TPeerType, TPayloadType>
		where TPeerType : INetPeer where TPayloadType : PacketPayload
	{

	}

	/// <summary>
	/// IoC/Meta-data Marker for elevated Request handlers.
	/// </summary>
	/// <typeparam name="TPeerType">Type of the peer.</typeparam>
	/// <typeparam name="TPayloadType">Type of the payload</typeparam>
	public interface IElevatedRequestPayloadHandler<in TPeerType> : IRequestPayloadHandler<TPeerType>
		where TPeerType : INetPeer
	{

	}
}
