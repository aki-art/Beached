using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using UnityEngine;
using UnityEngine.Assertions;
using static Beached.STRINGS.CREATURES.SPECIES;

namespace Beached.Content.Defs.Entities.Critters
{
	internal class SlickShellConfig : IEntityConfig
	{
		public const string ID = "Beached_SlickShell";
		public const string EGG_ID = "Beached_SlickShell_Egg";
		public const string BASE_TRAIT_ID = "Beached_SlickShellTrait";

		public GameObject CreatePrefab()
		{
			Log.Debug("CREATE SNAIL PREFAB -----------------------------");
			var prefab = CreateBasePrefab();

			EntityTemplates.ExtendEntityToWildCreature(prefab, CrabTuning.PEN_SIZE_PER_CREATURE);
			ConfigureBaseTrait(BEACHED_SLICKSHELL.NAME);
			ExtendToFertileCreature(prefab);

			var moistureMonitor = prefab.AddOrGetDef<MoistureMonitor.Def>();
			moistureMonitor.lubricant = Elements.mucus;
			moistureMonitor.lubricantMassKg = 0.1f;
			moistureMonitor.lubricantTemperatureKelvin = 300;

			prefab.AddTag(GameTags.OriginalCreature); // gravitas critter manipulator

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		private static GameObject CreateBasePrefab()
		{
			return BaseSnailConfig.CreatePrefab(
				ID,
				BEACHED_SLICKSHELL.NAME,
				BEACHED_SLICKSHELL.DESC,
				"beached_snail_kanim",
				BASE_TRAIT_ID);
		}

		private void ExtendToFertileCreature(GameObject prefab)
		{
			EntityTemplates.ExtendEntityToFertileCreature(
				prefab,
				EGG_ID,
				BEACHED_SLICKSHELL.EGG_NAME,
				BEACHED_SLICKSHELL.DESC,
				"beached_egg_slickshell_kanim",
				CrabTuning.EGG_MASS,
				BabySlickShellConfig.ID,
				60f,
				20f,
				SlickShellTuning.EGG_CHANCES_BASE,
				CrabConfig.EGG_SORT_ORDER);
		}

		private void ConfigureBaseTrait(string name)
		{
			var db = Db.Get();

			Assert.IsNotNull(db, "db is null");
			Assert.IsNotNull(db.traits, "traits is null");
			Assert.IsNotNull(db.Amounts, "amounts is null");
			Assert.IsNotNull(BAmounts.Moisture, "bamounts is null");

			var trait = db.CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

			trait.Add(new AttributeModifier(db.Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name));
			trait.Add(new AttributeModifier(db.Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name));
			trait.Add(new AttributeModifier(db.Amounts.HitPoints.maxAttribute.Id, 25f, name));
			trait.Add(new AttributeModifier(db.Amounts.Age.maxAttribute.Id, 100f, name));
			trait.Add(new AttributeModifier(BAmounts.Moisture.maxAttribute.Id, 100f, name));
			trait.Add(new AttributeModifier(BAmounts.Moisture.deltaAttribute.Id, -1000f / 600f, name));
		}

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
