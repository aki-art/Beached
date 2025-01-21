using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Beached.Patches.DebugPatches
{
	public class DebugPatches
	{
		[HarmonyPatch]
		public class Debug_Assert_Patch
		{
			public static IEnumerable<MethodBase> TargetMethods()
			{
				yield return AccessTools.Method(typeof(Debug), "Assert", [typeof(bool)]);
				yield return AccessTools.Method(typeof(Debug), "Assert", [typeof(bool), typeof(object)]);
				yield return AccessTools.Method(typeof(Debug), "Assert", [typeof(bool), typeof(object), typeof(UnityEngine.Object)]);
			}

			public static void Prefix(bool condition)
			{
				if (!condition)
				{
					var stackTrace = new StackTrace();
					Debug.Log($"[ASSERT FAILED]");
					for (int i = 1; i < 10; i++)
						Debug.Log($"{stackTrace.GetFrame(i)?.GetMethod().FullDescription()}");
				}
			}
		}
	}
}
