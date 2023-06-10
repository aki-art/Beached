using Database;

namespace Beached.Content.ModDb
{
	public class BThoughts
	{
		public static Thought scared;

		[DbEntry]
		public static void Register(Thoughts __instance)
		{
			scared = new Thought(
				"Beached_Thought_Scared",
				__instance,
				"beached_crew_state_scared",
				null,
				"crew_state_unhappy",
				"bubble_alert",
				SpeechMonitor.PREFIX_SAD,
				"tooltip");
		}
	}
}
