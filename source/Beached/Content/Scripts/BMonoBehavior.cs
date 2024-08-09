namespace Beached.Content.Scripts
{
	public class BMonoBehavior : KMonoBehaviour
	{
		protected void Debug(object message) => FUtility.Log.Debug($"{name} - {message}");
		protected void Info(object message) => FUtility.Log.Info($"{name} - {message}");
		protected void Warn(object message) => FUtility.Log.Warning($"{name} - {message}");
	}
}
