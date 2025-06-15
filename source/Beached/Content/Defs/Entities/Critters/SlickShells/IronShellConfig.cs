using Beached.Content.DefBuilders;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.SlickShells
{
	[EntityConfigOrder(CONSTS.CRITTER_LOAD_ORDER.ADULT)]
	public class IronShellConfig : BaseSnailConfig, IEntityConfig
	{
		public const string ID = "Beached_IronShell";
		public const string EGG_ID = "Beached_IronShellEgg";

		protected override string AnimFile => "beached_snail_kanim";

		protected override string Id => ID;

		public GameObject CreatePrefab() => CreatePrefab(this);

		protected override CritterBuilder ConfigureCritter(CritterBuilder builder)
		{
			return base.ConfigureCritter(builder)
				.TemperatureCelsius(50, 70, 270, 310)
				.Drops(SeaShellConfig.ID, RawSnailConfig.ID)
				.SymbolPrefix("iron_")
				.Traits()
					.Add(BAmounts.Moisture.maxAttribute.Id, 100f)
					.Add(BAmounts.Moisture.deltaAttribute.Id, -1000f / CONSTS.CYCLE_LENGTH)
					.Done()
				.Egg(BabyIronShellConfig.ID, "beached_egg_slickshell_kanim")
					.Mass(0.3f)
					.Fertility(10)
					.Incubation(20)
					.EggChance(EggId, 100f)
					.Done();
		}

		public override GameObject CreatePrefab(BaseCritterConfig config)
		{
			var prefab = base.CreatePrefab(config);

			var moistureMonitor = prefab.AddOrGetDef<MoistureMonitor.Def>();
			moistureMonitor.lubricant = Elements.mucus;
			moistureMonitor.defaultMucusRate = 300f / 600f;
			moistureMonitor.lubricantTemperatureKelvin = 300;

			var diet = new Diet(SulfurDiet());
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = SlickShellTuning.CALORIES_PER_KG_OF_ORE * SlickShellTuning.MIN_POOP_SIZE_IN_KG;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			var host = prefab.AddOrGetDef<LimpetHost.Def>();
			host.maxLevel = 3;
			host.defaultGrowthRate = LimpetHost.GROWTH_RATE_4_CYCLES;
			host.itemDroppedOnShear = SimHashes.FoolsGold.CreateTag();
			host.massDropped = 10f;
			host.targetSymbol = "beached_limpetgrowth";
			host.limpetKanim = "beached_ironshell_limpetgrowth_kanim";
			host.metabolismModifier = 1.2f;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public static Diet.Info[] SulfurDiet()
		{
			return
			[
				new(
					[SimHashes.Sulfur.CreateTag()],
					SimHashes.Obsidian.CreateTag(),
					SlickShellTuning.CALORIES_PER_KG_OF_ORE,
					TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL,
					null,
					0)
			];
		}

		public void OnPrefabInit(GameObject prefab) { }

		public void OnSpawn(GameObject inst) { }
	}
}
