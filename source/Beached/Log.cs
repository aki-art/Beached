using Beached.ModDevTools;

namespace Beached
{
	public class Log
	{
		public static void Debug(object msg)
		{
			if (Mod.debugMode)
			{
				global::Debug.Log($"[Beached]: {msg}");
				if (msg != null)
				{
					ConsoleDevTool.AddToLog(ConsoleDevTool.LogType.Debug, msg as string);
				}
			}
		}

		public static void Info(object msg)
		{
			global::Debug.Log($"[Beached]: {msg}");
			ConsoleDevTool.AddToLog(ConsoleDevTool.LogType.Info, $"[INFO]: {msg}");
		}

		public static void Warning(object msg)
		{
			global::Debug.Log($"[Beached] [WARNING]: {msg}");
			ConsoleDevTool.AddToLog(ConsoleDevTool.LogType.Warning, $"[WARNING]: {msg}");
		}
	}
}
