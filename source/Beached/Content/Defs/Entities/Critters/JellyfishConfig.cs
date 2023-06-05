using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using UnityEngine;
using static Beached.STRINGS.CREATURES.SPECIES;

namespace Beached.Content.Defs.Entities.Critters
{
	internal class JellyfishConfig : IEntityConfig
	{
		public const string ID = "Beached_Jellyfish";
		public const string EGG_ID = "Beached_Jellyfish_Egg";
		public const string BASE_TRAIT_ID = "Beached_JellyfishTrait";

		public GameObject CreatePrefab()
		{
			var prefab = CreateBasePrefab();

			EntityTemplates.ExtendEntityToWildCreature(prefab, CrabTuning.PEN_SIZE_PER_CREATURE);
			ConfigureBaseTrait(BEACHED_SLICKSHELL.NAME);
			ExtendToFertileCreature(prefab);

			var planktonDiet = new Diet(new Diet.Info(
				new() { PlanktonGerms.ID },
				null,
				1f));

			var creatureCalorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			creatureCalorieMonitor.diet = planktonDiet;

			var germConsumerMonitor = prefab.AddOrGetDef<GermConsumerMonitor.Def>();
			germConsumerMonitor.diet = planktonDiet;
			germConsumerMonitor.consumableGermIdx = Db.Get().Diseases.GetIndex(BDiseases.plankton.id);

			prefab.AddTag(GameTags.OriginalCreature); // for gravitas critter manipulator

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		private static GameObject CreateBasePrefab()
		{
			return BaseJellyfishConfig.CreatePrefab(
				ID,
				BEACHED_JELLYFISH.NAME,
				BEACHED_JELLYFISH.DESC,
				"beached_jellyfish_kanim",
				BASE_TRAIT_ID,
				null);
		}

		private void ExtendToFertileCreature(GameObject prefab)
		{
			EntityTemplates.ExtendEntityToFertileCreature(
				prefab,
				EGG_ID,
				BEACHED_JELLYFISH.EGG_NAME,
				BEACHED_JELLYFISH.DESC,
				"beached_egg_slickshell_kanim",
				CrabTuning.EGG_MASS,
				BabyJellyfishConfig.ID,
				60f,
				20f,
				JellyfishTuning.EGG_CHANCES_BASE,
				CrabConfig.EGG_SORT_ORDER);
		}

		private void ConfigureBaseTrait(string name)
		{
			var db = Db.Get();

			var trait = db.CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

			trait.Add(new AttributeModifier(db.Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name));
			trait.Add(new AttributeModifier(db.Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name));
			trait.Add(new AttributeModifier(db.Amounts.HitPoints.maxAttribute.Id, 25f, name));
			trait.Add(new AttributeModifier(db.Amounts.Age.maxAttribute.Id, 100f, name));
		}

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
