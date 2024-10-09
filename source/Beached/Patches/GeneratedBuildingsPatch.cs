using Beached.Content;
using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using System;

namespace Beached.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
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
