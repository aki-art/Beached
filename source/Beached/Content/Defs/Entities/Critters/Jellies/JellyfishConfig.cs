using Beached.Content.DefBuilders;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using UnityEngine;
using static Beached.STRINGS.CREATURES.SPECIES;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	[EntityConfigOrder(0)]
	public class JellyfishConfig : BaseJellyfishConfig, IEntityConfig
	{
		public const string ID = "Beached_Jellyfish";
		public const string EGG_ID = "Beached_Jellyfish_Egg";
		public const string BASE_TRAIT_ID = "Beached_JellyfishTrait";

		protected override string AnimFile => "beached_jellyfish_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Size(1, 2)
				.Speed(0.25f)
				.Egg(BabyJellyfishConfig.ID, "beached_egg_slickshell_kanim")
					.Incubation(20)
					.Fertility(60)
					.NotRanchable()
					.EggChance(EGG_ID, 1)
					.Mass(1)
					.Done()
				.Tags([GameTags.OriginalCreature]);
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);
			var electricEmitter = prefab.AddComponent<ElectricEmitter>();
			electricEmitter.maxCells = 128;
			electricEmitter.powerLossMultiplier = 1;
			electricEmitter.minPathSecs = 0.3f;
			electricEmitter.maxPathSecs = 2f;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
