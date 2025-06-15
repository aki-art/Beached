using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.Defs.Medicines;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class Recipes
	{
		public static string jellyBarRecipeID;

		public static void AddRecipes()
		{
			CreateFoodRecipes();
			CreateMedicineRecipes();
			CreateEquipmentRecipes();
			AddBismuthToLeadSuit();

			RecipeBuilder.Create(MudStomperConfig.ID, STRINGS.ITEMS.MISC.BEACHED_SOAP.DESC, 40f)
				.Input(Elements.ambergris.CreateTag(), 25f)
				.Input(Elements.ash.CreateTag(), 25f)
				.Output(SoapConfig.ID, 5f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.GREEN);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.BLUE);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.BLACK);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.RETRO);

			RecipeBuilder.Create(MudStomperConfig.ID, STRINGS.ITEMS.MISC.BEACHED_SOAP.DESC, 40f)
				.Input(Elements.ambergris.CreateTag(), 25f)
				.Input(Elements.ash.CreateTag(), 25f)
				.Output(SoapConfig.ID, 5f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			if (DlcManager.FeatureRadiationEnabled())
			{
				RecipeBuilder.Create(
					SuitFabricatorConfig.ID, global::STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC, TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME)
				.Input(Elements.bismuth.CreateTag(), 200f)
				.Input(SimHashes.Glass.CreateTag(), 10f)
				.Output(LeadSuitConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.ResultWithIngredient)
				.RequireTech(Db.Get().TechItems.leadSuit.parentTechId)
				.SortOrder(6)
				.Build();
			}

			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, 40f)
				.Input(Elements.aquamarine.CreateTag(), 100f)
				.Output(Elements.beryllium.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			RecipeBuilder.Create(RockCrusherConfig.ID, global::STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION, 40f)
				.Input(Elements.aquamarine.CreateTag(), 100f)
				.Output(Elements.beryllium.CreateTag(), 50f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();


			ZincAndCopperToBrass();
			BoneToCalcium();
		}

		private static void AddBismuthToLeadSuit()
		{
			var leadSuitRecipe = ComplexRecipeManager.Get().GetRecipe($"SuitFabricator_I_Lead_Glass_O_Lead_Suit");

			if (leadSuitRecipe != null)
			{
				var ingredients = new List<Tag>();
				List<float> amounts = null;

				var ingredient = leadSuitRecipe.ingredients[0];

				if (ingredient.possibleMaterials != null)
					ingredients.AddRange(ingredient.possibleMaterials);

				ingredient.material = null;

				// if another mod messed with this
				if (ingredient.possibleMaterialAmounts != null && ingredient.possibleMaterialAmounts.Length == ingredient.possibleMaterials.Length)
				{
					amounts = [.. ingredient.possibleMaterialAmounts, 200f];
				}

				ingredients.Add(Elements.bismuth.CreateTag());

				ingredient.possibleMaterials = ingredients.ToArray();

				if (amounts != null)
					ingredient.possibleMaterialAmounts = amounts.ToArray();

				leadSuitRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			}
			else
			{
				Log.Warning("Could not find Lead Suit recipe entry, cannot add Bismuth as an option.");
			}
		}

		private static void CreateMedicineRecipes()
		{
			RecipeBuilder.Create(ApothecaryConfig.ID, STRINGS.ITEMS.PILLS.BEACHED_SUPERALLERGYMEDICATION.RECIPEDESC, 100f)
				.Input(SwampLilyFlowerConfig.ID, 3f)
				.Input(PoffConfig.GetRawId(Elements.nitrogen), 1f)
				.Output(SuperAllergyMedicationConfig.ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
				.SortOrder(11)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

		}

		private static void BoneToCalcium()
		{
			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, 40f)
				.Input(Elements.bone.CreateTag(), 100f)
				.Output(Elements.moltenCalcium.CreateTag(), 50f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();
		}

		private static void ZincAndCopperToBrass()
		{
			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, 40f)
				.Input(Elements.zinc.CreateTag(), 50f)
				.Input(SimHashes.Copper.CreateTag(), 100f)
				.Output(Elements.brass.CreateTag(), 150f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();
		}

		private static void CreateEquipmentRecipes()
		{
			RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_RUBBERBOOTS.DESCRIPTION, 60f)
				.Input(Elements.rubber.CreateTag(), 30f)
				.Output(RubberBootsConfig.ID, 1f)
				.Build();

			RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_ZEOLITEPENDANT.DESCRIPTION, 60f)
				.Input(Elements.zeolite.CreateTag(), 30f)
				.Output(ZeolitePendantConfig.ID, 1f)
				.Build();
		}

		private static void CreateFoodRecipes()
		{
			jellyBarRecipeID = RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_JELLYBAR.DESC, 40f)
				.Input(BTags.Groups.jellies, 1f)
				.Output(JellyBarConfig.ID, 1f)
				.Visualizer("beached_jellybar_kanim")
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build()
				.id;

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_BERRYJELLY.DESC, 40f)
				.Input(BTags.Groups.jellies, 1f)
				.Input(PrickleFruitConfig.ID, 1f)
				.Output(BerryJellyConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
			{
				RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_BERRYJELLY.DESC, 40f)
					.Input(BTags.Groups.jellies, 1f)
					.Input(HardSkinBerryConfig.ID, 0.5f)
					.Output(BerryJellyConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build();
			}

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SALTRUBBEDJELLY.DESC, 40f)
				.Input(BTags.Groups.jellies, 1f)
				.Input(SimHashes.Salt.CreateTag(), 5f)
				.Output(SaltRubbedJellyConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_DRYNOODLES.DESC, 40f)
				.Input(BTags.Groups.grains, 4f)
				.Input(RawEggConfig.ID, 1f)
				.Output(DryNoodlesConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPAGHETTI.DESC, 40f)
				.Input(SpiceNutConfig.ID, 4f)
				// maybe deep grass? or some other herb
				.Input(DryNoodlesConfig.ID, 1f)
				.Output(SpaghettiConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SCRAMBLEDSNAILS.DESC, 40f)
				.Input(RawSnailConfig.ID, 3f)
				.Input(RawEggConfig.ID, 0.5f)
				.Output(ScrambledSnailsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPICYCRACKLINGS.DESC, 40f)
				.Input(CracklingsConfig.ID, 1f)
				.Input(SpiceNutConfig.ID, 3f)
				.Output(SpicyCracklingsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
			{
				RecipeBuilder.Create(DeepfryerConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_CRABCAKES.DESC, 40f)
					.Input(ShellfishMeatConfig.ID, 1f)
					.Input(BTags.Groups.grains, 3f)
					.Input(RawEggConfig.ID, 3f)
					.Input(SimHashes.Tallow.CreateTag(), 1f)
					.Output(CrabCakesConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build();
			}
			else
			{
				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_CRABCAKES.DESC, 40f)
					.Input(ShellfishMeatConfig.ID, 1f)
					.Input(BTags.Groups.grains, 3f)
					.Input(RawEggConfig.ID, 3f)
					.Output(CrabCakesConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build();
			}

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_GLAZEDDEWNUT.DESC, 40f)
				.Input(DewPalmConfig.SEED_ID, 3f)
				.Input(SimHashes.Sucrose.CreateTag(), 3f)
				.Output(GlazedDewnutConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_ASPICLICE.DESC, 40f)
				.Input(BTags.Groups.jellies, 1f)
				.Input(BasicPlantFoodConfig.ID, 1f)
				.Output(AspicLiceConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SEAFOODPASTA.DESC, 40f)
				.Input(FishMeatConfig.ID, 1f)
				.Input(RawKelpConfig.ID, 1f)
				.Input(DryNoodlesConfig.ID, 1f)
				.Output(SeafoodPastaConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_STUFFEDSNAILS.DESC, 40f)
				.Input(RawSnailConfig.ID, 2f)
				.Input(RawKelpConfig.ID, 1f)
				.Input(SimHashes.MilkFat.CreateTag(), 10f)
				.Output(StuffedSnailsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_ASTROBAR.DESC, 40f)
				.Input(SpongeCakeConfig.ID, 1f) // 1900 KCal
				.Input(SpiceNutConfig.ID, 4f)
				.Input(SimHashes.Sucrose.CreateTag(), 15f)
				.Output(AstrobarConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_LEGENDARYSTEAK.DESC, 40f)
				.Input(HighQualityMeatConfig.ID, 1f)
				.Input(TableSaltConfig.ID, 1f)
				.Output(LegendarySteakConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			var eggResult = string.Format(global::STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.NAME);
			var eggDescription = string.Format(global::STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.NAME, eggResult);

			// manually add this one
			RecipeBuilder.Create(EggCrackerConfig.ID, eggDescription, 5f)
				.Input(InfertileEggConfig.ID, 2f)
				.Input(RawEggConfig.ID, 1f)
				.Output(EggShellConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Ingredient)
				.Build();

			if (DlcManager.IsExpansion1Active())
			{
				RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPONGECAKE.DESC, 40f)
					.Input(WashuSpongeConfig.SEED_ID, 3f)
					.Input(SimHashes.Sucrose.CreateTag(), 50f)
					.Output(SpongeCakeConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Visualizer()
					.Build();
			}
			else
			{
				RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPONGECAKE.DESC, 40f)
					.Input(WashuSpongeConfig.SEED_ID, 3f)
					.Input(SimHashes.Sucrose.CreateTag(), 30f)
					.Output(SpongeCakeConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Visualizer()
					.Build();
			}
		}

		private static void AddBeachShirtCostumeRecipe(string facadeID)
		{
			RecipeBuilder
				.Create(ClothingFabricatorConfig.ID, global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC, TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)

				.Input(GameTags.Fabrics, 3f)

				.FacadeOutput(BeachShirtConfig.ID, 1f, facadeID)

				.Build(facadeID);
		}
	}
}
