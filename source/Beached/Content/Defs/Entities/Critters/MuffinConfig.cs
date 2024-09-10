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
		public const int SORTING_ORDER = 48;

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

			prefab.AddTags(
				GameTags.OriginalCreature,
				BTags.Creatures.doNotTargetMeByCarnivores);

			prefab.AddOrGetDef<PreyMonitor.Def>().allyTags =
			[
				BTags.Creatures.doNotTargetMeByCarnivores
			];

			prefab.AddComponent<CollarWearer>();

			prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			prefab.AddOrGetDef<SolidConsumerMonitor.Def>();

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
				"beached_egg_muffin_kanim",
				1f,
				BabyJellyfishConfig.ID,
				60f,
				20f,
				JellyfishTuning.EGG_CHANCES_BASE,
				CrabConfig.EGG_SORT_ORDER);
		}

		private static Diet ConfigureDiet(GameObject prefab)
		{
			var meats = Assets
				.GetPrefabsWithTag(BTags.meat)
				.Where(prefab => !prefab.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
				.Select(meat => meat.PrefabID())
				.ToHashSet();

			var eggs = Assets
				.GetPrefabsWithTag(GameTags.Egg)
				.Where(prefab => !prefab.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
				.Select(egg => egg.PrefabID())
				.ToHashSet();

			var diet = new Diet(
				new Diet.Info(
					meats,
					Elements.bone.CreateTag(),
					400_000f),
				new Diet.Info(
					[RawEggConfig.ID, CookedEggConfig.ID, QuicheConfig.ID],
					null,
					calories_per_kg: 400_000),
				new Diet.Info(
					eggs,
					EggShellConfig.ID,
					calories_per_kg: 400_000));

			var creatureCalorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			creatureCalorieMonitor.diet = diet;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return diet;
		}

		public static void OnPostEntitiesLoaded()
		{
			Assets.GetPrefab(EGG_ID).AddTag(BTags.Creatures.doNotTargetMeByCarnivores);
			ConfigureDiet(Assets.GetPrefab(ID));
		}

		// define it later than usual so items can be collected by tags
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
