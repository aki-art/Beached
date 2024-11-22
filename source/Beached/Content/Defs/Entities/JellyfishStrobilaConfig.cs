using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Scripts.Entities.AI.Strobila;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class JellyfishStrobilaConfig : IEntityConfig
	{
		public const string ID = "Beached_JellyfishStrobila";

		public GameObject CreatePrefab()
		{
			var anim = Assets.GetAnim("beached_jellyfish_strobila_kanim");

			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISHSTROBILA.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_JELLYFISHSTROBILA.DESCRIPTION,
				200f,
				anim,
				"idle_loop",
				Grid.SceneLayer.Building,
				1,
				2,
				TUNING.DECOR.BONUS.TIER2,
				default,
				SimHashes.Creature);

			// survival
			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<SubmersionMonitor>();
			prefab.AddOrGet<UprootedMonitor>();
			prefab.AddOrGet<TemperatureVulnerable>().ConfigureCelsius(10, 0, 65, 80);
			prefab.AddOrGet<WiltCondition>();
			prefab.AddOrGetDef<DeathMonitor.Def>();

			prefab.AddOrGet<Traits>();
			//prefab.AddOrGet<Health>();
			prefab.AddOrGet<CharacterOverlay>();
			//prefab.AddOrGet<RangedAttackable>();


			prefab.AddOrGetDef<AnimInterruptMonitor.Def>();

			prefab.AddTag(GameTags.Creature);

			var pressureVulnerable = prefab.AddOrGet<PressureVulnerable>();
			pressureVulnerable.Configure(CoralTemplate.ALL_WATERS);

			prefab.AddOrGet<Prioritizable>();
			prefab.AddOrGet<Uprootable>();

			prefab.AddOrGet<OccupyArea>().objectLayers =
			[
				ObjectLayer.Building
			];

			prefab.GetComponent<KPrefabID>().prefabInitFn += inst =>
			{
				if (inst.TryGetComponent(out PressureVulnerable pressureVulnerable2))
				{
					foreach (var safeElement in CoralTemplate.ALL_WATERS)
						pressureVulnerable2.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeElement));
				}
			};

			prefab.AddOrGet<Strobila>();

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
