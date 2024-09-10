using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using UnityEngine;
using static Beached.STRINGS.CREATURES.SPECIES;

namespace Beached.Content.Defs.Entities.Critters
{
	[EntityConfigOrder(0)]
	public class SlickShellConfig : IEntityConfig
	{
		public const string ID = "Beached_SlickShell";
		public const string EGG_ID = "Beached_SlickShell_Egg";
		public const string BASE_TRAIT_ID = "Beached_SlickShellTrait";

		public GameObject CreatePrefab()
		{
			var prefab = CreateBasePrefab();

			EntityTemplates.ExtendEntityToWildCreature(prefab, CrabTuning.PEN_SIZE_PER_CREATURE);
			ConfigureBaseTrait(BEACHED_SLICKSHELL.NAME);
			ExtendToFertileCreature(prefab);

			var moistureMonitor = prefab.AddOrGetDef<MoistureMonitor.Def>();
			moistureMonitor.lubricant = Elements.mucus;
			moistureMonitor.lubricantMassKg = 0.1f;
			moistureMonitor.lubricantTemperatureKelvin = 300;

			prefab.AddTag(GameTags.OriginalCreature); // gravitas critter manipulator

			var diet = new Diet(SaltDiet());
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = SlickShellTuning.CALORIES_PER_KG_OF_ORE * SlickShellTuning.MIN_POOP_SIZE_IN_KG;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

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
				BASE_TRAIT_ID,
				[SeaShellConfig.ID]);
		}

		private void ExtendToFertileCreature(GameObject prefab)
		{
			EntityTemplates.ExtendEntityToFertileCreature(
				prefab,
				EGG_ID,
				BEACHED_SLICKSHELL.EGG_NAME,
				BEACHED_SLICKSHELL.DESC,
				"beached_egg_slickshell_kanim",
				SlickShellTuning.EGG_MASS,
				BabySlickShellConfig.ID,
				60f,
				20f,
				SlickShellTuning.EGG_CHANCES_BASE,
				CrabConfig.EGG_SORT_ORDER);
		}

		private void ConfigureBaseTrait(string name)
		{
			var db = Db.Get();

			var trait = db.CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

			trait.Add(new AttributeModifier(db.Amounts.Calories.maxAttribute.Id, SlickShellTuning.STANDARD_STOMACH_SIZE, name));
			trait.Add(new AttributeModifier(db.Amounts.Calories.deltaAttribute.Id, -SlickShellTuning.STANDARD_CALORIES_PER_CYCLE / CONSTS.CYCLE_LENGTH, name));

			trait.Add(new AttributeModifier(db.Amounts.HitPoints.maxAttribute.Id, 25f, name));
			trait.Add(new AttributeModifier(db.Amounts.Age.maxAttribute.Id, 25f, name));
			trait.Add(new AttributeModifier(BAmounts.Moisture.maxAttribute.Id, 100f, name));
			trait.Add(new AttributeModifier(BAmounts.Moisture.deltaAttribute.Id, -1000f / 600f, name));
		}

		public static Diet.Info[] SaltDiet()
		{
			return
			[
				new(
					[SimHashes.Salt.CreateTag()],
					SimHashes.Dirt.CreateTag(),
					SlickShellTuning.CALORIES_PER_KG_OF_ORE,
					TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL,
					null,
					0)
			];
		}

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst)
		{
			// TODO: temporary fix because i animated the snail facing the wrong way
			inst.transform.localScale = new Vector3(
				inst.transform.localScale.x * -1f,
				inst.transform.localScale.y * 1,
				inst.transform.localScale.z * 1);
		}
	}
}
