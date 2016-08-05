using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Metadata to indcate that a handler requires elevated session verification semantics.
	/// Sessions that aren't elvated should be unable to reach the handler. Implementation is left to the consumer
	/// of the libraries.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ElevatedSessionHandlingRequiredAttribute : Attribute
	{
		//We need nothing but the attribute as a metadata marker
	}
}
