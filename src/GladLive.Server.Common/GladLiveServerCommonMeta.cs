using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	/// <summary>
	/// Meta-data marker type for referencing the assembly
	/// </summary>
	public class GladLiveServerCommonMeta
	{
		public Assembly Assembly { get { return typeof(GladLiveServerCommonMeta).Assembly; } }
	}
}
