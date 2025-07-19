using Beached.Content.Overlays;
using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class WaterGeneratorConfig : IBuildingConfig
	{
		public const string ID = "Beached_WaterGenerator";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"farmtile_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Anywhere,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.GeneratorWattageRating = 300f;
			def.GeneratorBaseCapacity = 1000f;
			def.ExhaustKilowattsWhenActive = 2f;
			def.SelfHeatKilowattsWhenActive = 2f;

			def.ViewMode = ElementInteractionsOverlayMode.ID;
			def.Floodable = false;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();

			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();


			go.AddOrGet<WaterGenerator>();


			Tinkerable.MakePowerTinkerable(go);
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
