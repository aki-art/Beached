using Beached.Content.ModDb.Sicknesses;

namespace Beached.Content.ModDb
{
	public class BChoreTypes
	{
		public static ChoreType iceWrathLashOut;

		[DbEntry]
		public static void Register(Database.ChoreTypes __instance)
		{
			iceWrathLashOut = __instance.Add(
				"Beached_IceWrathLashOut",
				[],
				"",
				["MoveTo"],
				global::STRINGS.DUPLICANTS.CHORES.STRESSACTINGOUT.NAME,
				global::STRINGS.DUPLICANTS.CHORES.STRESSACTINGOUT.STATUS,
				$"This Duplicant is agitated by their {STRINGS.Link("Ice Wraiths.", IceWrathSickness.ID)}",
				false);
		}
	}
}
