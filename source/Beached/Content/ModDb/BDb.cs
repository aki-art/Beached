﻿using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BDb
	{
		public const NotificationType BeachedTutorialMessage = (NotificationType)351;
		public const string poisBuildCategory = "Beached_POIs";

		public static BPlushies plushies;

		public static void OnDbInit()
		{
			plushies = new BPlushies();
		}

		public class WearableTypes
		{
			public static readonly WearableAccessorizer.WearableType jewellery = (WearableAccessorizer.WearableType)Hash.SDBMLower("Beached_jewellery");
		}

		public static void AddRecipes()
		{
			CreateFoodRecipes();
			CreateEquipmentRecipes();
			AddBismuthRecipes();

			if (DlcManager.FeatureRadiationEnabled())
			{
				RecipeBuilder.Create(
					SuitFabricatorConfig.ID, global::STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC, TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME)
				.Input(Elements.bismuth.Tag, 200f)
				.Input(SimHashes.Glass.CreateTag(), 10f)
				.Output(LeadSuitConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.ResultWithIngredient)
				.RequireTech(Db.Get().TechItems.leadSuit.parentTechId)
				.SortOrder(6)
				.Build();
			}
		}

		private static void CreateEquipmentRecipes()
		{
			RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_RUBBERBOOTS.DESCRIPTION, 60f)
				.Input(Elements.rubber.Tag, 30f)
				.Output(RubberBootsConfig.ID, 1f)
				.Build();

			RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_ZEOLITEPENDANT.DESCRIPTION, 60f)
				.Input(Elements.heulandite.Tag, 30f)
				.Output(ZeolitePendantConfig.ID, 1f)
				.Build();
		}

		private static void CreateFoodRecipes()
		{
			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_BERRYJELLY.DESC, 40f)
				.Input(JellyConfig.ID, 1f)
				.Input(PrickleFruitConfig.ID, 2f)
				.Output(BerryJellyConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_ASPICLICE.DESC, 40f)
				.Input(JellyConfig.ID, 1f)
				.Input(BasicPlantFoodConfig.ID, 2f)
				.Output(AspicLiceConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.Build();

			RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.BEACHED_LEGENDARYSTEAK.DESC, 40f)
				.Input(HighQualityMeatConfig.ID, 1f)
				.Input(TableSaltConfig.ID, 1f)
				.Output(LegendarySteakConfig.ID, 1f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
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
					// TODO
					.Output(SpongeCakeConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Visualizer()
					.Build();
			}
		}

		// Tries to find all recipes that use "starter metals", and then inserts Bismuth
		private static void AddBismuthRecipes()
		{
			var refinedStarters = new List<string>()
			{
				SimHashes.Copper.ToString(),
				SimHashes.Cobalt.ToString(),
				SimHashes.Aluminum.ToString(),
				SimHashes.Iron.ToString(),
			};

			var oreStarters = new List<string>()
			{
				SimHashes.Cuprite.ToString(),
				SimHashes.Cobaltite.ToString(),
				SimHashes.AluminumOre.ToString(),
				SimHashes.IronOre.ToString(),
			};

			CloneRecipes(refinedStarters, Elements.bismuth.ToString());
			CloneRecipes(oreStarters, Elements.bismuthOre.ToString());
		}

		private static void CloneRecipes(List<string> starters, string newElement)
		{
			var referenceElement = starters[0];
			var manager = ComplexRecipeManager.Get();

			for (int i = 0; i < manager.recipes.Count; i++)
			{
				var recipe = manager.recipes[i];
				var newRecipeId = recipe.id.Replace(referenceElement.ToString(), newElement);

				// already exists
				if (manager.GetRecipe(newRecipeId) != null) continue;

				var index = GetIndexForElement(starters, referenceElement, manager, recipe);

				// Does not have required starter elements as ingredients
				if (index == -1) continue;

				// create new input list
				var inputs = new List<ComplexRecipe.RecipeElement>(recipe.ingredients) { }.ToArray();
				var referenceIngredient = inputs[index];

				inputs[index] = new ComplexRecipe.RecipeElement(newElement, referenceIngredient.amount)
				{
					temperatureOperation = referenceIngredient.temperatureOperation,
					storeElement = referenceIngredient.storeElement,
					facadeID = referenceIngredient.facadeID,
					inheritElement = referenceIngredient.inheritElement
				};

				// create new recipe
				var referenceName = ElementLoader.GetElement(referenceElement).name;
				var newElementName = ElementLoader.GetElement(newElement).name;
				var description = recipe.description.Replace(referenceName, newElementName);

				foreach (var fabricator in recipe.fabricators) // usually just one but just to be safe
				{
					var id = ComplexRecipeManager.MakeRecipeID(fabricator.ToString(), inputs, recipe.results);
					new ComplexRecipe(id, inputs, recipe.results)
					{
						time = recipe.time,
						nameDisplay = recipe.nameDisplay,
						description = description,
						fabricators = new List<Tag>()
						{
							fabricator
						}
					};

					Log.Info($"Automatically added {newElementName} to {fabricator} recipes with id {id}");

					var obsoleteId = ComplexRecipeManager.MakeObsoleteRecipeID(fabricator.ToString(), newRecipeId);
					manager.AddObsoleteIDMapping(obsoleteId, id);
				}
			}
		}

		private static int GetIndexForElement(List<string> starters, string referenceElement, ComplexRecipeManager manager, ComplexRecipe recipe)
		{
			var index = -1;

			for (int i = 0; i < starters.Count; i++)
			{
				if (!HasRecipeWith(starters[i], referenceElement, recipe.id, manager))
				{
					return -1;
				}

				if (i == 0)
				{
					index = FindIndexOfMaterial(recipe.ingredients, starters[i]);
				}
			}

			return index;
		}

		private static bool HasRecipeWith(string element, string original, string recipe, ComplexRecipeManager manager)
		{
			return manager.GetRecipe(recipe.Replace(original, element)) != null;
		}

		private static int FindIndexOfMaterial(ComplexRecipe.RecipeElement[] ingredients, Tag material)
		{
			for (int i = 0; i < ingredients.Length; i++)
			{
				if (ingredients[i].material == material)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
