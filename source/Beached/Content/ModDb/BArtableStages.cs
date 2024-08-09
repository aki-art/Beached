using Beached.Content.Defs.Buildings;
using Database;

namespace Beached.Content.ModDb
{
	public class BArtableStages
	{
		public static void Register(ArtableStages stages)
		{
			RegisterWoodenCarvings(stages);
		}

		private static void RegisterWoodenCarvings(ArtableStages stages)
		{
			stages.Add(new ArtableStage(
				"Beached_WoodCarving_Owl",
				STRINGS.BUILDINGS.PREFABS.BEACHED_WOODCARVING.FACADES.OWL.NAME,
				STRINGS.BUILDINGS.PREFABS.BEACHED_WOODCARVING.FACADES.OWL.DESC,
				PermitRarity.Universal,
				"beached_woodcarving_owl_kanim",
				"idle",
				15,
				true,
				Db.Get().ArtableStatuses.LookingGreat,
				WoodCarvingConfig.ID,
				string.Empty,
				DlcManager.AVAILABLE_ALL_VERSIONS));
		}
	}
}
