using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class SpinnerConfig : IBuildingConfig
	{
		public const string ID = "Beached_Spinner";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				4,
				"beached_spinner_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.ALL_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.NONE,
				default);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 240f;
			def.SelfHeatKilowattsWhenActive = 16f;

			def.ViewMode = OverlayModes.Power.ID;

			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = AUDIO.SIZE.LARGE;

			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();

			var complexFabricator = go.AddOrGet<Spinner>();

			BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

			go.AddOrGet<ComplexFabricatorWorkable>();
			go.AddOrGet<FabricatorIngredientStatusManager>();

			ConfigureRecipes();
		}

		private void ConfigureRecipes()
		{
			RecipeBuilder.Create(ID, STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.BEACHED_SLAGWOOL.DESC, 40)
				.Input(Elements.slag.CreateTag(), 50)
				.Output(SlagWoolConfig.ID, 1)
				.SortOrder(0)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			RecipeBuilder.Create(ID, STRINGS.ITEMS.FOOD.BEACHED_COTTONCANDY.RECIPEDESC, 40)
				.Input(SimHashes.Sucrose.CreateTag(), 30)
				.Output(CottonCandyConfig.ID, 1)
				.SortOrder(99)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			SymbolOverrideControllerUtil.AddToPrefab(go);
		}
	}
}
