using Beached.Content.DefBuilders;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.SlickShells
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class SlickShellConfig : BaseSnailConfig, IEntityConfig
	{
		public const string ID = "Beached_SlickShell";
		public const string EGG_ID = "Beached_SlickShellEgg";

		protected override string AnimFile => "beached_snail_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.Drops(SlickShellShellConfig.ID, 1f)
				.Traits()
					.Add(BAmounts.Moisture.deltaAttribute.Id, -100f / CONSTS.CYCLE_LENGTH, false, STRINGS.CREATURES.SPECIES.BEACHED_SLICKSHELL.NAME)
					.Add(BAmounts.Mucus.deltaAttribute.Id, SlickShellTuning.MUCUS_PER_CYCLE / CONSTS.CYCLE_LENGTH, false, STRINGS.CREATURES.SPECIES.BEACHED_SLICKSHELL.NAME)
					.Done()
				.Tag(GameTags.OriginalCreature)
				.Egg(BabySlickShellConfig.ID, "beached_egg_slickshell_kanim")
					.Mass(0.3f)
					.Fertility(15)
					.Incubation(20)
					.EggChance(EggId, 100f)
					.EggChance(IronShellConfig.EGG_ID, 0f)
					.Done();
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			var moistureMonitor = prefab.AddOrGetDef<MoistureMonitor.Def>();
			moistureMonitor.lubricant = Elements.mucus;
			moistureMonitor.defaultMucusRate = 150f / 600f;
			moistureMonitor.lubricantTemperatureKelvin = MiscUtil.CelsiusToKelvin(38.0f);

			prefab.AddOrGetDef<DesiccationMonitor.Def>();

			var diet = new Diet(SaltDiet());
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = SlickShellTuning.CALORIES_PER_KG_OF_ORE * SlickShellTuning.MIN_POOP_SIZE_IN_KG;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			var host = prefab.AddOrGetDef<LimpetHost.Def>();
			host.maxLevel = 3;
			host.defaultGrowthRate = LimpetHost.GROWTH_RATE_4_CYCLES;
			host.itemDroppedOnShear = SimHashes.Lime.CreateTag();
			host.massDropped = 10f;
			host.targetSymbol = "beached_limpetgrowth";
			host.limpetKanim = "beached_slickshell_limpetgrowth_kanim";
			host.metabolismModifier = 1.2f;
			//host.glandMass = 5f;

			var moltDropper = prefab.AddOrGetDef<MoltDropperMonitor.Def>();
			moltDropper.onGrowDropID = SlickShellShellConfig.ID;
			moltDropper.massToDrop = 10f;
			moltDropper.isReadyToMolt = IsReadyToMolt;

			return prefab;
		}

		public string[] GetDlcIds() => null;

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
					0),

				new (
					[CrystalConfig.SALT],
					SimHashes.Dirt.CreateTag(),
					SlickShellTuning.CALORIES_PER_KG_OF_ORE,
					TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL,
					null,
					0,
					food_type: Diet.Info.FoodType.EatPlantStorage)
			];
		}

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
