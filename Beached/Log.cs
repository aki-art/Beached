namespace Beached
{
    public class Log
    {
        public static void Debug(object msg)
        {
            if (Mod.DebugMode)
            {
                global::Debug.Log($"[Beached]: {msg}");
            }
        }

        public static void Info(object msg)
        {
            global::Debug.Log($"[Beached]: {msg}");
        }

        public static void Warning(object msg)
        {
            global::Debug.Log($"[Beached] [WARNING]: {msg}");
        }
    }
}
