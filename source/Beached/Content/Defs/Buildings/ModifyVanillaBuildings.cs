using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class ModifyVanillaBuildings
	{
		public static void Run()
		{
			ConfigureLubricatableBuildingPrefabs();
			Desalinator();
			Shower();
			ConfigureSoapHolder("SgtImalas_BathTub");
			WoodGasGenerator();

			AddPOIUnlockable(IceCooledFanConfig.ID);
			AddPOIUnlockable(IceMachineConfig.ID);
			AddPOIUnlockable(BeachChairConfig.ID);
		}

		private static void AddPOIUnlockable(string ID) => Db.Get().TechItems.Get(ID).isPOIUnlock = true;


		// make it also convert murky brine to p.water and salt
		private static void Desalinator()
		{
			var def = Assets.GetBuildingDef(DesalinatorConfig.ID);

			ApplyToTemplate(def, go =>
			{
				var murkyBrineToPollutedWater = go.AddComponent<ElementConverter>();

				murkyBrineToPollutedWater.consumedElements =
				[
					new ElementConverter.ConsumedElement(Elements.murkyBrine.CreateTag(), 5f)
				];

				murkyBrineToPollutedWater.outputElements =
				[
					new ElementConverter.OutputElement(3.5f, SimHashes.DirtyWater, 0f, storeOutput: true),
					new ElementConverter.OutputElement(1.5f, SimHashes.Salt, 0f, storeOutput: true)
				];
			});
		}

		// add byproduct of ash to wood burning
		private static void WoodGasGenerator()
		{
			if (!TryGetComplete(WoodGasGeneratorConfig.ID, out var go))
				return;

			if (go.TryGetComponent(out EnergyGenerator generator))
			{
				var outputs = new List<EnergyGenerator.OutputItem>(generator.formula.outputs)
					{
						new(Elements.ash,0.5f,false)
					};

				generator.formula.outputs = [.. outputs];
			}
		}

		// remove mucus effects on shower + add soap holder
		public static void Shower()
		{
			if (!global::Shower.EffectsRemoved.Contains(BEffects.STEPPED_IN_MUCUS))
			{
				MiscUtil.AddToStaticReadonlyArray<string, Shower>(
					nameof(global::Shower.EffectsRemoved),
					BEffects.STEPPED_IN_MUCUS, BEffects.SUBMERGED_IN_MUCUS);
			}

			// add soap deliverable
			ConfigureSoapHolder(ShowerConfig.ID);
		}

		// add soap deliverable
		private static void ConfigureSoapHolder(string buildingId)
		{
			if (!TryGetComplete(buildingId, out var go))
				return;

			var refillAmount = 50f;
			var use = 10f;

			var soapStorage = go.AddComponent<Storage>();
			soapStorage.capacityKg = refillAmount;
			soapStorage.showInUI = true;

			var manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
			manualDeliveryKg.SetStorage(soapStorage);
			manualDeliveryKg.RequestedItemTag = SoapConfig.ID;
			manualDeliveryKg.capacity = refillAmount;
			manualDeliveryKg.refillMass = use;
			manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
			manualDeliveryKg.ShowStatusItem = false;

			var soapy = go.AddComponent<Beached_SoapHolder>();
			soapy.minimumSoap = use;
			soapy.soapTag = SoapConfig.ID;
			soapy.soapStorage = soapStorage;
		}

		// deliverable mucus for increased output/work speed/opening speed
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
		private static bool TryGetComplete(string id, out GameObject completePrefab)
		{
			var def = Assets.GetBuildingDef(id);

			if (def == null)
			{
				completePrefab = null;
				return false;
			}

			completePrefab = def.BuildingComplete;
			return true;
		}

		private static void ApplyToTemplate(BuildingDef def, Action<GameObject> fn)
		{
			if (def == null)
				return;

			fn(def.BuildingComplete);
			fn(def.BuildingPreview);
			fn(def.BuildingUnderConstruction);
		}
	}
}
