namespace Beached.Content.Scripts
{
	public class FMonoBehaviour : KMonoBehaviour
	{
		protected void DebugLog(object message)
		{
			Beached.Log.Debug($"({GetType().Name}): {message}");
		}

		protected void Log(object message)
		{
			Beached.Log.Debug($"({GetType().Name}): {message}");
		}
	}
}
