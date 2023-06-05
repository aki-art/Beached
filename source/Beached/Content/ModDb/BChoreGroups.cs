using Database;

namespace Beached.Content.ModDb
{
	public class BChoreGroups
	{
		//public static ChoreGroup HandyWork;
		//public static string HANDYWORK_ID = "Beached_ChoreGroup_Handywork";

		public static ChoreGroup AnimalKeeping;
		public static string ANIMALKEEPING_ID = "Beached_ChoreGroup_AnimalKeeping";

		public static void Register(ChoreGroups parent)
		{
			/*            HandyWork = parent.Add(new ChoreGroup(
                            HANDYWORK_ID,
                            STRINGS.DUPLICANTS.CHOREGROUPS.BEACHED_CHOREGROUP_HANDYWORK.NAME,
                            BAttributes.handSteadiness,
                            ModAssets.Sprites.ERRAND_MINERALOGY, 
                            5));*/
			AnimalKeeping = parent.Add(new ChoreGroup(
				ANIMALKEEPING_ID,
				STRINGS.DUPLICANTS.CHOREGROUPS.BEACHED_CHOREGROUP_HANDYWORK.NAME,
				BAttributes.handSteadiness,
				ModAssets.Sprites.ERRAND_MINERALOGY,
				5));
		}
	}
}
