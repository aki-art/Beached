using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class WaterBatteryConfig : IBuildingConfig
	{
		public const string ID = "Beached_WaterBattery";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				4,
				"shower_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Anywhere,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.RequiresPowerInput = true;
			def.RequiresPowerOutput = true;
			def.PowerInputOffset = new CellOffset(0, 1);
			def.PowerOutputOffset = new CellOffset(1, 0);
			def.ElectricalArrowOffset = new CellOffset(1, 0);
			def.ExhaustKilowattsWhenActive = 0.05f;
			def.SelfHeatKilowattsWhenActive = 0.05f;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = "Metal";
			def.Entombable = true;
			def.GeneratorWattageRating = 100_000f;
			def.GeneratorBaseCapacity = 100_000f;
			def.PermittedRotations = PermittedRotations.FlipH;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
			go.AddComponent<RequireInputs>();

			var def = go.GetComponent<Building>().Def;
			var battery = go.AddOrGet<Battery>();
			battery.powerSortOrder = 99999;
			battery.capacity = def.GeneratorWattageRating;
			battery.chargeWattage = def.GeneratorWattageRating;

			var waterBattery = go.AddComponent<WaterBattery>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var upperStorage = go.AddComponent<Storage>();
			var lowerStorage = go.AddComponent<Storage>();

			var waterBattery = go.AddOrGet<WaterBattery>();
			waterBattery.upperStorage = upperStorage;
			waterBattery.lowerStorage = lowerStorage;

			Object.DestroyImmediate(go.GetComponent<EnergyConsumer>());
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
