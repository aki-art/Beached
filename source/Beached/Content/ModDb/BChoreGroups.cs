using Database;

namespace Beached.Content.ModDb
{
	public class BChoreGroups
	{
		public static ChoreGroup handyWork;
		public static string HANDYWORK_ID = "Beached_ChoreGroup_HandyWork";

		[DbEntry]
		public static void Register(ChoreGroups __instance)
		{
			Log.Debug("Adding chore group" + HANDYWORK_ID);
			handyWork = __instance.Add(new ChoreGroup(
				HANDYWORK_ID,
				STRINGS.DUPLICANTS.CHOREGROUPS.BEACHED_CHOREGROUP_HANDYWORK.NAME,
				BAttributes.handSteadiness,
				ModAssets.Sprites.ERRAND_MINERALOGY,
				5));
			Log.Debug(handyWork.Name);
		}
	}
}
