using GladNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Service for protecting server sessions from being created/established when
	/// they shouldn't be. Is essentially the protector of the gate to the server.
	/// </summary>
	/// <typeparam name="TSessionType"></typeparam>
	public interface IConnectionGateKeeper<TSessionType>
		where TSessionType : struct, IConvertible
	{
		/// <summary>
		/// Indicates if the port is valid for connecting on this server.
		/// </summary>
		/// <param name="port">Port value.</param>
		/// <returns>True if the port is a valid connection port.</returns>
		bool isValidPort(int port);

		/// <summary>
		/// Requests to pass through the gate.
		/// </summary>
		/// <param name="sessionType">Sessiontype this session will be.</param>
		/// <param name="details">Connection details of the session.</param>
		/// <returns>True is <typeparamref name="TSessionType"/> was valid for the connection details.</returns>
		bool RequestPassage(TSessionType sessionType, IConnectionDetails details);
	}
}
