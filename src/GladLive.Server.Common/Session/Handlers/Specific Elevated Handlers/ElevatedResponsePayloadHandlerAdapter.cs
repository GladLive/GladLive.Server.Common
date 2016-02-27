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
	/// Decorates an <see cref="IPayloadHandler{TPeerType}"/> with elevated requirement semantics.
	/// Specically inherits and extends the elevated handler as a response handler
	/// </summary>
	/// <typeparam name="TSessionType">Peer type.</typeparam>
	public class ElevatedResponsePayloadHandlerAdapter<TSessionType> : ElevatedSessionPayloadHandlerDecorator<TSessionType>, IElevatedResponsePayloadHandler<TSessionType>
		where TSessionType : INetPeer, IElevatableSession
	{
		public ElevatedResponsePayloadHandlerAdapter(ILog logger, IElevationVerificationService verifyService, IPayloadHandler<TSessionType> handlerToDecorate) 
			: base(logger, verifyService, handlerToDecorate)
		{

		}
	}
}
