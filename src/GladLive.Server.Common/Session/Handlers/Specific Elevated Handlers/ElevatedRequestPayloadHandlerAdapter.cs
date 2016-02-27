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
	/// Specically inherits and extends the elevated handler as a request handler
	/// </summary>
	/// <typeparam name="TSessionType">Peer type.</typeparam>
	public class ElevatedRequestPayloadHandlerDecorator<TSessionType> : ElevatedSessionPayloadHandlerDecorator<TSessionType>, IElevatedRequestPayloadHandler<TSessionType>
		where TSessionType : INetPeer, IElevatableSession
	{
		public ElevatedRequestPayloadHandlerDecorator(ILog logger, IElevationVerificationService verifyService, IPayloadHandler<TSessionType> handlerToDecorate) 
			: base(logger, verifyService, handlerToDecorate)
		{

		}
	}
}
