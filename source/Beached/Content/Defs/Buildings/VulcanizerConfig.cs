using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class VulcanizerConfig : IBuildingConfig
	{
		public const string ID = "Beached_Vulcanizer";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				3,
				"plasticrefinery_kanim",
				30,
				30f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.ALL_METALS,
				1600f,
				BuildLocationRule.OnFloor,
				TUNING.BUILDINGS.DECOR.NONE,
				NOISE_POLLUTION.NOISY.TIER3);

			BuildingTemplates.CreateElectricalBuildingDef(def);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 100f;
			def.SelfHeatKilowattsWhenActive = 12f;

			def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 2));

			def.ViewMode = OverlayModes.Power.ID;

			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = AUDIO.SIZE.LARGE;

			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			def.PermittedRotations = PermittedRotations.FlipH;

			def.AddSearchTerms(STRINGS.SEARCH_TERMS.RUBBER);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

			var complexFabricator = go.AddOrGet<ComplexFabricator>();
			complexFabricator.choreType = Db.Get().ChoreTypes.Compound;
			complexFabricator.fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;
			complexFabricator.duplicantOperated = true;
			complexFabricator.heatedTemperature = MiscUtil.CelsiusToKelvin(60);
			complexFabricator.showProgressBar = true;
			complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();

			BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

			go.AddOrGet<FabricatorIngredientStatusManager>();

			var dispenser = go.AddOrGet<ConduitDispenser>();
			dispenser.conduitType = ConduitType.Liquid;
			dispenser.elementFilter = [Elements.rubber];
			dispenser.storage = complexFabricator.outStorage;
			dispenser.alwaysDispense = true;

			go.AddOrGet<ElementConverter>().outputElements =
			[
				new ElementConverter.OutputElement(
					0.025f,
					SimHashes.ContaminatedOxygen,
					MiscUtil.CelsiusToKelvin(70),
					outputElementOffsety: 2f)
			];

			ConfigureRecipes();
			Prioritizable.AddRef(go);
		}

		private void ConfigureRecipes()
		{
			RecipeBuilder.Create(ID, Strings.Get("STRINGS.ELEMENTS.BEACHED_RUBBER.DESCRIPTION"), 60f)
			.Input(BTags.Groups.elastomers, 50f)
			.Input(BTags.Groups.sulfurs)
			.Output(Elements.rubber.CreateTag(), 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
			.SortOrder(0)
			.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
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
