using Beached.Content.Defs.Items;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class BioFuelMakerConfig : IBuildingConfig
	{
		public const string ID = "Beached_BioFuelMaker";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				3,
				"beached_biofuelreactor_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.ALL_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.NONE,
				default);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 60f;
			def.SelfHeatKilowattsWhenActive = 12f;

			def.UtilityOutputOffset = new CellOffset(0, 0);
			def.OutputConduitType = ConduitType.Liquid;

			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			def.ViewMode = OverlayModes.Power.ID;

			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = AUDIO.SIZE.LARGE;

			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

			var complexFabricator = go.AddOrGet<ComplexFabricator>();
			complexFabricator.choreType = Db.Get().ChoreTypes.Compound;
			complexFabricator.fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;
			complexFabricator.duplicantOperated = false;
			complexFabricator.heatedTemperature = MiscUtil.CelsiusToKelvin(60);
			complexFabricator.showProgressBar = true;
			complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();

			BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

			go.AddOrGet<FabricatorIngredientStatusManager>();

			var dispenser = go.AddOrGet<ConduitDispenser>();
			dispenser.conduitType = ConduitType.Liquid;
			dispenser.elementFilter = [Elements.bioFuel];
			dispenser.storage = complexFabricator.outStorage;
			dispenser.alwaysDispense = true;

			ConfigureRecipes();
			Prioritizable.AddRef(go);
		}

		private void ConfigureRecipes()
		{
			var bioFuelToLeafRatio = 50f;
			var ethanolToLeafRatio = 50f;
			var inputLeafMass = 1f;
			RecipeBuilder.Create(ID, "TODO", 40)
				.Input(PalmLeafConfig.ID, inputLeafMass)
				.Output(Elements.bioFuel.CreateTag(), inputLeafMass * bioFuelToLeafRatio, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
				.Output(SimHashes.Ethanol.CreateTag(), inputLeafMass * ethanolToLeafRatio, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
				.SortOrder(0)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Ingredient)
				.Build();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			SymbolOverrideControllerUtil.AddToPrefab(go);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
