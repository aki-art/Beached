using Beached.Content.Defs.Foods;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class SmokingRackConfig : IBuildingConfig
	{
		public const string ID = "Beached_SmokingRack";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				3,
				"beached_smoking_rack_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.BuildingAttachPoint,
				DECOR.NONE,
				default);

			def.AttachmentSlotTag = BTags.buildingAttachmentSmoker;
			def.attachablePosition = new CellOffset(2, 0);
			def.ObjectLayer = ObjectLayer.AttachableBuilding;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<Prioritizable>();
			Prioritizable.AddRef(go);

			var fabricator = go.AddOrGet<ComplexFabricator>();
			fabricator.heatedTemperature = 318.15f;
			fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
			fabricator.duplicantOperated = false;
			fabricator.showProgressBar = true;

			BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);

			fabricator.inStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);
			fabricator.buildStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);
			fabricator.outStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);

			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();
			go.AddOrGet<ComplexFabricatorWorkable>();

			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);

			ConfigureRecipes();
		}

		private void ConfigureRecipes()
		{
			var fabricationTime = 240f;
			var amountAtOnce = 4f;

			CreateStandardSmokedRecipe(fabricationTime, amountAtOnce, MeatConfig.ID, SmokedMeatConfig.ID);
			CreateStandardSmokedRecipe(fabricationTime, amountAtOnce, BasicPlantFoodConfig.ID, SmokedLiceConfig.ID);
			CreateStandardSmokedRecipe(fabricationTime, amountAtOnce, TofuConfig.ID, SmokedTofuConfig.ID);
			CreateStandardSmokedRecipe(fabricationTime, amountAtOnce, FishMeatConfig.ID, SmokedFishConfig.ID);
			CreateStandardSmokedRecipe(fabricationTime, amountAtOnce, RawSnailConfig.ID, SmokedSnailConfig.ID);
		}

		private static void CreateStandardSmokedRecipe(float fabricationTime, float amountAtOnce, Tag input, Tag output)
		{
			RecipeBuilder
				.Create(ID, fabricationTime)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Input(input, amountAtOnce)
				.Output(output, amountAtOnce, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
				.Build();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
