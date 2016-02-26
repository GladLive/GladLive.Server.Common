using GladNet.Common;
using GladNet.Serializer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladLive.Server.Common
{
	[TestFixture]
	public static class PayloadTests
	{
		[Test]
		[TestCaseSource(nameof(PayloadTypes))]
		public static void Test_Inherits_From_PacketPayload(Type payloadType)
		{
			//assert
			Assert.IsTrue(typeof(PacketPayload).IsAssignableFrom(payloadType));
		}

		[Test]
		[TestCaseSource(nameof(PayloadTypes))]
		public static void Test_Has_Parameterless_Constructor(Type payloadType)
		{
			//assert: Check that it has parameterless constructors
			Assert.IsTrue(payloadType.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
				.FirstOrDefault(x => x.GetParameters().Count() == 0) != null);
		}

		[Test]
		[TestCaseSource(nameof(PayloadTypes))]
		public static void Test_Has_Contract_For_Serialization(Type payloadType)
		{
			//assert: Check that all of them have serialization enabled
			Assert.IsTrue(payloadType.GetCustomAttributes(typeof(GladNetSerializationContractAttribute), true).Count() != 0);
		}

		//Gathers all the payload types in the assembly
		public static Type[] PayloadTypes = typeof(AuthenticationRequest).Assembly.GetTypes().Where(x => typeof(PacketPayload).IsAssignableFrom(x)).ToArray();
	}
}
