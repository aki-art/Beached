using Beached.Content.Defs.Buildings;
using Database;

namespace Beached.Content.ModDb
{
	public class BArtableStages
	{
		public static void Register(ArtableStages stages)
		{
			RegisterSandboxes(stages);
			RegisterWoodenCarvings(stages);
		}

		private static void RegisterSandboxes(ArtableStages stages)
		{
			var great = Db.Get().ArtableStatuses.LookingGreat;

			Add(stages, SandBoxConfig.ID, "castle", "beached_sandbox_castle_kanim", great, 5);
			Add(stages, SandBoxConfig.ID, "castle2", "beached_sandbox_castle2_kanim", great, 5);
			//Add(stages, SandBoxConfig.ID, "crab", "beached_sandbox_crab_kanim", great, 5);
			Add(stages, SandBoxConfig.ID, "tiger_shark", "beached_sandbox_tiger_shark_kanim", great, 5);
		}

		private static void RegisterWoodenCarvings(ArtableStages stages)
		{
			Great(stages, WoodCarvingConfig.ID, "owl", "beached_woodcarving_owl_kanim");
		}

		private static void Ugly(ArtableStages stages, string buildingId, string id, string kanim)
		{
			Add(stages, buildingId, id, kanim, Db.Get().ArtableStatuses.LookingUgly);
		}

		private static void Okay(ArtableStages stages, string buildingId, string id, string kanim)
		{
			Add(stages, buildingId, id, kanim, Db.Get().ArtableStatuses.LookingOkay);
		}

		private static void Great(ArtableStages stages, string buildingId, string id, string kanim)
		{
			Add(stages, buildingId, id, kanim, Db.Get().ArtableStatuses.LookingGreat);
		}

		private static void Add(ArtableStages stages, string buildingId, string id, string kanim, ArtableStatusItem status, int? decorOverride = null)
		{
			var name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.NAME");
			var desc = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.DESC");

			stages.Add(new ArtableStage(
				$"Beached_{buildingId}_{id}",
				name,
				desc,
				PermitRarity.Universal,
				kanim,
				"idle",
				decorOverride ?? (int)status.StatusType * 5,
				(int)status.StatusType > 1,
				status,
				buildingId,
				string.Empty,
				DlcManager.AVAILABLE_ALL_VERSIONS));
		}
	}
}
