using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Service offers sign verification and signing.
	/// </summary>
	public interface ISigningService
	{
		/// <summary>
		/// Indicates if the <paramref name="message"/> was signed.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="signedMessage">The signed version of the message.</param>
		/// <returns>True if the message was properly signed.</returns>
		bool isSigned(string message, byte[] signedMessage);

		/// <summary>
		/// Indicates if the <paramref name="message"/> was signed.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="signedMessage">The signed version of the message.</param>
		/// <returns>True if the message was properly signed.</returns>
		bool isSigned(byte[] message, byte[] signedMessage);

		/// <summary>
		/// Signs the <paramref name="message"/>.
		/// </summary>
		/// <param name="message">The message to sign.</param>
		/// <returns>byte[] representation of the message signed.</returns>
		byte[] SignMessage(string message);

		/// <summary>
		/// Signs the <paramref name="message"/>.
		/// </summary>
		/// <param name="message">The message to sign.</param>
		/// <returns>byte[] representation of the message signed.</returns>
		byte[] SignMessage(byte[] message);
	}
}
