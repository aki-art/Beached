using Beached.Content.Defs.Buildings;
using Beached.Content.ModDb;
using FUtility;
using HarmonyLib;
using static FUtility.CONSTS;
using static FUtility.CONSTS.SUB_BUILD_CATEGORY;

namespace Beached.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddMenus();
				AddTechs();
			}

			public static void Postfix()
			{
				var planInfo = new PlanScreen.PlanInfo(
					BDb.poisBuildCategory,
					false,
					[
						ForceFieldGeneratorConfig.ID
					]);

				TUNING.BUILDINGS.PLANORDER.Add(planInfo);
				ModifyVanillaBuildings.Run();
				Recipes.AddRecipes();
			}

			private static void AddMenus()
			{
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.POWER, AmmoniaGeneratorConfig.ID, Power.GENERATORS, MethaneGeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.POWER, WaterGeneratorConfig.ID, Power.GENERATORS, GeneratorConfig.ID);
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
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.POWER, BioFuelGeneratorConfig.ID, Power.GENERATORS, GeneratorConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, BioFuelMakerConfig.ID, Refining.MATERIALS, EthanolDistilleryConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.UTILITIES, MossTerrariumConfig.ID, Utilities.OTHER_UTILITIES);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, AquaticFarmTileConfig.ID, Food.FARMING);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.FOOD, MirrorConfig.ID, Food.RANCHING, UnderwaterCritterCondoConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.BASE, DeconstructableRocketTileConfig.ID, Base.TILES, RocketWallTileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.UTILITIES, CrystalGrowerConfig.ID, Utilities.OTHER_UTILITIES);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, GemCutterConfig.ID, Refining.ADVANCED);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, CrystalSynthetizerConfig.ID, Refining.ADVANCED);
				ModUtil.AddBuildingToPlanScreen(BUILD_CATEGORY.REFINING, VulcanizerConfig.ID, Refining.MATERIALS, PolymerizerConfig.ID);
			}

			private static void AddTechs()
			{
				BuildingUtil.AddToResearch(ChimeConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SandBoxConfig.ID, TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(SmokingRackConfig.ID, TECH.FOOD.RANCHING);
				BuildingUtil.AddToResearch(SpinnerConfig.ID, TECH.SOLIDS.SMELTING);
				BuildingUtil.AddToResearch(CollarDispenserConfig.ID, TECH.SOLIDS.SMELTING);
				BuildingUtil.AddToResearch(BioFuelGeneratorConfig.ID, TECH.POWER.COMBUSTION);
				BuildingUtil.AddToResearch(AquaticFarmTileConfig.ID, TECH.FOOD.AGRICULTURE);
				BuildingUtil.AddToResearch(DeconstructableRocketTileConfig.ID, TECH.SOLIDS.REFINED_OBJECTS);
				BuildingUtil.AddToResearch(WaterGeneratorConfig.ID, TECH.POWER.PRETTY_GOOD_CONDUCTORS);
			}
		}
	}
}
