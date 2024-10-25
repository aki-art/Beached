using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Buildings;
using FUtility;
using HarmonyLib;
using System;
using static FUtility.CONSTS;
using static FUtility.CONSTS.SUB_BUILD_CATEGORY;

namespace Beached.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Postfix()
			{
				var planInfo = new PlanScreen.PlanInfo(
					BDb.poisBuildCategory,
					false,
					[
						ForceFieldGeneratorConfig.ID
					]);

				TUNING.BUILDINGS.PLANORDER.Add(planInfo);

				RegisterBuildings();

				Recipes.AddRecipes();

				ConfigureLubricatableBuildingPrefabs();
			}

			private static void ConfigureLubricatableBuildingPrefabs()
			{
				// TODO: buildings from Chemical Processing

				// generators
				AddTimerLubricatable(HydrogenGeneratorConfig.ID);
				AddTimerLubricatable(ManualGeneratorConfig.ID);
				AddTimerLubricatable("StirlingEngine");

				// fabricators
				AddLubricatable(RockCrusherConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(SludgePressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(MilkPressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(UraniumCentrifugeConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(DiamondPressConfig.ID, ModTuning.standardLubricantUses);
				AddLubricatable(SpinnerConfig.ID, ModTuning.standardLubricantUses);

				// other
				AddLubricatable(OreScrubberConfig.ID, ModTuning.standardLubricantUses, workable => workable is OreScrubber);
				AddLubricatable(LiquidPumpingStationConfig.ID, ModTuning.standardLubricantUses, workable => workable is LiquidPumpingStation);

				// doors
				foreach (var buildingDef in Assets.BuildingDefs)
				{
					var isDoor = buildingDef.BuildingComplete.TryGetComponent(out Door _);
					var modDidntPrevent = !buildingDef.BuildingComplete.HasTag(BTags.preventLubrication);

					if (isDoor && modDidntPrevent)
					{
						AddLubricatable(buildingDef.PrefabID, ModTuning.standardLubricantUses);
						buildingDef.BuildingComplete.AddOrGet<Beached_DoorOpenTracker>();
					}
				}
			}

			private static void RegisterBuildings()
			{
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, Power.GENERATORS, MethaneGeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.UTILITIES, MossBedConfig.ID, Utilities.OTHER_UTILITIES, ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, MiniFridgeConfig.ID, Food.STORAGE, ExteriorWallConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, DNAInjectorConfig.ID, Food.RANCHING, EggIncubatorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, CollarDispenserConfig.ID, Food.RANCHING, DNAInjectorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.BASE, LaboratoryTileConfig.ID, Base.TILES, PlasticTileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, WoodCarvingConfig.ID, Furniture.DISPALY, MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, SandBoxConfig.ID, Furniture.DISPALY, WoodCarvingConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, ChimeConfig.ID, Furniture.RECREATION, FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FURNITURE, SmallAquariumConfig.ID, Furniture.DISPALY, FlowerVaseConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, MudStomperConfig.ID, Refining.MATERIALS);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, SmokingRackConfig.ID, Food.COOKING);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, SpinnerConfig.ID, Refining.MATERIALS, RockCrusherConfig.ID);

				BuildingUtil.AddToResearch(ChimeConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SandBoxConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SmokingRackConfig.ID, TECH.FOOD.RANCHING);
				BuildingUtil.AddToResearch(SpinnerConfig.ID, TECH.SOLIDS.SMELTING);

			}
			private static Lubricatable AddTimerLubricatable(string prefabId, float time = CONSTS.CYCLE_LENGTH)
			{
				return AddLubricatable(prefabId, 10f, 10f / time, true);
			}

			private static Lubricatable AddLubricatable(string prefabId, int times)
			{
				return AddLubricatable(prefabId, 10f, 10f / times, false);
			}

			private static Lubricatable AddLubricatable(string prefabId, int times, Func<object, bool> isUsedFn)
			{
				var result = AddLubricatable(prefabId, 10f, 10f / times, false);
				if (result == null)
					return null;

				result.consumeOnComplete += isUsedFn;
				return result;
			}

			private static Lubricatable AddLubricatable(string prefabId, float capacity, float kgUsedEachTime, bool isTimedUse)
			{
				var def = Assets.GetBuildingDef(prefabId);
				if (def == null)
					return null;

				return Lubricatable.ConfigurePrefab(def.BuildingComplete, capacity, kgUsedEachTime, isTimedUse);
			}
		}
	}
}
