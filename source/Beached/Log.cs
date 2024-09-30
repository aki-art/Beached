using System;

namespace Beached
{
	public class Log
	{
		public static void Debug(object msg)
		{
			if (Mod.debugMode)
			{
				global::Debug.Log($"[Beached]: {msg}");
			}
		}

		public static void Info(object msg)
		{
			global::Debug.Log($"[Beached]: {msg}");
		}

		public static void Assert(Func<bool> fn, object msg)
		{
			if (!fn())
				global::Debug.Log($"[Beached] [ASSERT FAILED]: {msg}");
		}

		public static void AssertNotNull(object obj, object objectName)
		{
			if (obj == null)
				global::Debug.Log($"[Beached] [ASSERT FAILED]: {objectName} is null");
		}

		public static void Warning(object msg)
		{
			global::Debug.Log($"[Beached] [WARNING]: {msg}");
		}
	}
}
