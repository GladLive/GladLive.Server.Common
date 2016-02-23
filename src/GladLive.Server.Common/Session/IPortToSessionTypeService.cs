using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Provides port to <typeparamref name="TSessionType"/> mapping services.
	/// </summary>
	/// <typeparam name="TSessionType"></typeparam>
	public interface IPortToSessionTypeService<TSessionType>
		where TSessionType : struct, IConvertible
	{
		/// <summary>
		/// Converts a primtive port value to the <typeparamref name="TSessionType"/>.
		/// </summary>
		/// <param name="port">Port value.</param>
		/// <returns>A <see cref="TSessionType"/> mapped from the port value or default if invalid.</returns>
		TSessionType ToSessionType(int port);
	}
}
