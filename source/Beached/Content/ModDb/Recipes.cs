using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Entities;
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

		private const float TIME_STANDARD = 40f;

		public static void AddRecipes()
		{
			CreateFoodRecipes();
			CreateMedicineRecipes();
			CreateEquipmentRecipes();
			AddBismuthToLeadSuit();

			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.GREEN);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.BLUE);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.BLACK);
			AddBeachShirtCostumeRecipe(BEquippableFacades.BEACHSHIRTS.RETRO);

			RecipeBuilder.Create(MudStomperConfig.ID, STRINGS.ITEMS.MISC.BEACHED_SOAP.DESC, TIME_STANDARD)
				.Input([Elements.ambergris.CreateTag(), SimHashes.Tallow.CreateTag()], 25f)
				.Input(Elements.ash.CreateTag(), 25f)
				.Output(SoapConfig.ID, 5f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();


			JuiceRecipe(Elements.gnawBerryJuice, GnawicaBerryConfig.ID, 10f);
			JuiceRecipe(Elements.gristleBerryJuice, PrickleFruitConfig.ID, 12f / 16f);
			if (Mod.integrations.IsModPresent(Integration.Integrations.ROLLER_SNAKES))
				JuiceRecipe(Elements.cactusJuice, "CactusFlesh", 10f);

			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, TIME_STANDARD)
				.Input(Elements.aquamarine.CreateTag(), 100f)
				.Output(Elements.beryllium.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			ZincAndCopperToBrass();
			BoneToCalcium();
			RockCrusher();
			Kiln();
		}

		private static void Kiln()
		{
			/*			RecipeBuilder.Create(KilnConfig.ID, global::STRINGS.ELEMENTS.SULFUR.DESC, TIME_STANDARD)
							.Input(SulfurGlandConfig.ID, 20f)
							.Output(SimHashes.Sulfur.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
							.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
							.Build();*/
		}

		private static void RockCrusher()
		{
			RecipeBuilder.Create(RockCrusherConfig.ID, global::STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION, TIME_STANDARD)
				.Input(Elements.aquamarine.CreateTag(), 100f)
				.Output(Elements.beryllium.CreateTag(), 50f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			RecipeBuilder.Create(RockCrusherConfig.ID, global::STRINGS.ELEMENTS.LIME.DESC, TIME_STANDARD)
				.Input(SlickShellShellConfig.ID, 0.5f)
				.Output(SimHashes.Lime.CreateTag(), 30f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			RecipeBuilder.Create(RockCrusherConfig.ID, global::STRINGS.ELEMENTS.FOOLSGOLD.DESC, TIME_STANDARD)
				.Input(IronShellShellConfig.ID, 1)
				.Output(SimHashes.FoolsGold.CreateTag(), 30f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();

			RecipeBuilder.Create(RockCrusherConfig.ID, global::STRINGS.ELEMENTS.LIME.DESC, TIME_STANDARD)
				.Input(SeaShellConfig.ID, 1)
				.Output(SimHashes.Lime.CreateTag(), 30f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();
		}

		private static void JuiceRecipe(SimHashes element, Tag material, float amount)
		{
			var tag = element.CreateTag();

			RecipeBuilder.Create(MilkPressConfig.ID, Elements.Description(element), TIME_STANDARD)
				.Input([material], amount)
				.Output(tag, 100f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Custom)
				.CustomName(tag.ProperNameStripLink())
				.IconPrefab(tag.name)
				.Build();
		}

		private static void AddBismuthToLeadSuit()
		{
			if (!DlcManager.FeatureRadiationEnabled())
				return;

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
			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, TIME_STANDARD)
				.Input(Elements.bone.CreateTag(), 100f)
				.Output(Elements.moltenCalcium.CreateTag(), 50f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.IngredientToResult)
				.Build();
		}

		private static void ZincAndCopperToBrass()
		{
			RecipeBuilder.Create(MetalRefineryConfig.ID, global::STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, TIME_STANDARD)
				.Input(BTags.Groups.zincs, 50f)
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
				.RequireTech(BTechs.MATERIALS1)
				.Build();
		}

		private static void CreateFoodRecipes()
		{
			jellyBarRecipeID = RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_JELLYBAR.DESC, TIME_STANDARD)
				.Input(BTags.Groups.jellies, 1f)
				.Output(JellyBarConfig.ID, 1f)
				.Visualizer("beached_jellybar_kanim")
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build()
				.id;

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_BERRYJELLY.DESC, TIME_STANDARD)
				.Input(BTags.Groups.jellies, 1f)
				.Input(BTags.Groups.berries, 1f)
				.Output(BerryJellyConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SALTRUBBEDJELLY.DESC, TIME_STANDARD)
				.Input(BTags.Groups.jellies, 1f)
				.Input(SimHashes.Salt.CreateTag(), 5f)
				.Output(SaltRubbedJellyConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_DRYNOODLES.DESC, TIME_STANDARD)
				.Input(BTags.Groups.grains, 4f)
				.Input(RawEggConfig.ID, 1f)
				.Output(DryNoodlesConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPAGHETTI.DESC, TIME_STANDARD)
				.Input(SpiceNutConfig.ID, 4f)
				// maybe deep grass? or some other herb
				.Input(DryNoodlesConfig.ID, 1f)
				.Output(SpaghettiConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SCRAMBLEDSNAILS.DESC, TIME_STANDARD)
				.Input(RawSnailConfig.ID, 3f)
				.Input(RawEggConfig.ID, 0.5f)
				.Output(ScrambledSnailsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPICYCRACKLINGS.DESC, TIME_STANDARD)
				.Input(CracklingsConfig.ID, 1f)
				.Input(SpiceNutConfig.ID, 3f)
				.Output(SpicyCracklingsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			if (DlcManager.IsContentSubscribed(DlcManager.DLC2_ID))
			{
				RecipeBuilder.Create(DeepfryerConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_CRABCAKES.DESC, TIME_STANDARD)
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
				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_CRABCAKES.DESC, TIME_STANDARD)
					.Input(ShellfishMeatConfig.ID, 1f)
					.Input(BTags.Groups.grains, 3f)
					.Input(RawEggConfig.ID, 3f)
					.Output(CrabCakesConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build();
			}

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_GLAZEDDEWNUT.DESC, TIME_STANDARD)
				.Input(DewPalmConfig.SEED_ID, 3f)
				.Input(SimHashes.Sucrose.CreateTag(), 3f)
				.Input(BTags.Groups.berryJuices, 5f)
				.Output(GlazedDewnutConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_ASPICLICE.DESC, TIME_STANDARD)
				.Input(BTags.Groups.jellies, 1f)
				.Input(BasicPlantFoodConfig.ID, 1f)
				.Output(AspicLiceConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SEAFOODPASTA.DESC, TIME_STANDARD)
				.Input(FishMeatConfig.ID, 1f)
				.Input(BTags.Groups.kelps, 1f)
				.Input(DryNoodlesConfig.ID, 1f)
				.Output(SeafoodPastaConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_STUFFEDSNAILS.DESC, TIME_STANDARD)
				.Input(RawSnailConfig.ID, 2f)
				.Input(BTags.Groups.kelps, 1f)
				.Input(BTags.Groups.snailShells, 1f)
				.Output(StuffedSnailsConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			var baseMeats = new Dictionary<Tag, float>()
			{
				{ MeatConfig.ID, 2f },
			};

			if (DlcManager.IsContentSubscribed(DlcManager.DLC4_ID))
				baseMeats.Add(DinosaurMeatConfig.ID, 2f);

			var seaFoods = new Dictionary<Tag, float>()
			{
				{ FishMeatConfig.ID, 1f },
				{ ShellfishMeatConfig.ID, 1f },
				{ RawSnailConfig.ID, 1f },
				{ CracklingsConfig.ID, 1f },
			};

			var meatPlatter = RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_MEATPLATTER.DESC, TIME_STANDARD)
				.Input(HighQualityMeatConfig.ID, 1f)
				.Input(baseMeats)
				.Input(seaFoods)
				.Output(MeatPlatterConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result);

			meatPlatter
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_ASTROBAR.DESC, TIME_STANDARD)
				.Input(SpongeCakeConfig.ID, 1f) // 1900 KCal
				.Input(SpiceNutConfig.ID, 4f)
				.Input(SimHashes.Sucrose.CreateTag(), 15f)
				.Output(AstrobarConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_LEGENDARYSTEAK.DESC, TIME_STANDARD)
				.Input(DryAgedMeatConfig.ID, 1f)
				.Output(LegendarySteakConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(BrinePoolConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_DRYAGEDMEAT.DESC, 600f * 10f)
				.Input(HighQualityMeatConfig.ID, 12)
				.Output(DryAgedMeatConfig.ID, 1)
				.SortOrder(0)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			var eggResult = string.Format(global::STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.NAME);
			var eggDescription = string.Format(global::STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, STRINGS.ITEMS.FOOD.BEACHED_INFERTILEEGG.NAME, eggResult);

			// manually add this one
			RecipeBuilder.Create(EggCrackerConfig.ID, eggDescription, 5f)
				.Input(InfertileEggConfig.ID, 2f)
				.Output(RawEggConfig.ID, 1f)
				.Output(EggShellConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Ingredient)
				.Build();

			// manually add this one
			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_VEGGIEBURGER.RECIPEDESC, TUNING.FOOD.RECIPES.STANDARD_COOK_TIME)
				.Input(ColdWheatBreadConfig.ID, 1f)
				.Input(LettuceConfig.ID, 1f)
				.Input(FriedMushroomConfig.ID, 1.5f)
				.Input(PickleConfig.ID, 1f)
				.Output(VeggieBurgerConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			if (DlcManager.IsExpansion1Active())
			{
				RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPONGECAKE.DESC, TIME_STANDARD)
					.Input(WashuSpongeConfig.SEED_ID, 3f)
					.Input(SimHashes.Sucrose.CreateTag(), 50f)
					.Output(SpongeCakeConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Visualizer()
					.Build();
			}
			else
			{
				RecipeBuilder.Create(MicrobeMusherConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_SPONGECAKE.DESC, TIME_STANDARD)
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
