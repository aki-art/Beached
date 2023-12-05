using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.BABY - 1)]
	public class MuffinConfig : IEntityConfig
	{
		public const string ID = "Beached_Muffin";
		public const string EGG_ID = "Beached_Muffin_Egg";
		public const string BASE_TRAIT_ID = ID + "Original";

		public GameObject CreatePrefab()
		{
			var prefab = BaseMuffinConfig.CreatePrefab(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_MUFFIN.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_MUFFIN.DESC,
				"beached_muffin_kanim",
				false);

			EntityTemplates.ExtendEntityToWildCreature(prefab, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
			ExtendToFertileCreature(prefab);

			ConfigureBaseTrait(STRINGS.CREATURES.SPECIES.BEACHED_MUFFIN.NAME);
			var diet = ConfigureDiet(prefab);

			prefab.AddTags(
				GameTags.OriginalCreature,
				BTags.Creatures.doNotTargetMeByCarnivores);

			prefab.AddOrGetDef<PreyMonitor.Def>().allyTags = new[]
			{
				BTags.Creatures.doNotTargetMeByCarnivores
			};

			prefab.AddComponent<CollarWearer>();

			return prefab;
		}

		// TODO: implement muffin whelps
		private void ExtendToFertileCreature(GameObject prefab)
		{
			EntityTemplates.ExtendEntityToFertileCreature(
				prefab,
				EGG_ID,
				STRINGS.CREATURES.SPECIES.BEACHED_MUFFIN.EGG_NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_MUFFIN.DESC,
				"beached_egg_slickshell_kanim",
				1f,
				BabyJellyfishConfig.ID,
				60f,
				20f,
				JellyfishTuning.EGG_CHANCES_BASE,
				CrabConfig.EGG_SORT_ORDER);
		}

		private Diet ConfigureDiet(GameObject prefab)
		{
			var foods = Assets
				.GetPrefabsWithTag(BTags.meat)
				.Select(meat => meat.PrefabID())
				.ToHashSet();

			foods.Add(RawEggConfig.ID);
			foods.Add(CookedEggConfig.ID);

			var diet = new Diet(
				new Diet.Info(
					foods,
					SimHashes.DirtyIce.CreateTag(),
					400_000f)); // TODO: meat to ice balance

			var creatureCalorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			creatureCalorieMonitor.diet = diet;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return diet;
		}

		private void ConfigureBaseTrait(string name)
		{
			var db = Db.Get();

			var trait = db.CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

			trait.Add(new AttributeModifier(db.Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name));
			trait.Add(new AttributeModifier(db.Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / CONSTS.CYCLE_LENGTH, name));
			trait.Add(new AttributeModifier(db.Amounts.HitPoints.maxAttribute.Id, 100f, name));
			trait.Add(new AttributeModifier(db.Amounts.Age.maxAttribute.Id, 150f, name));
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
