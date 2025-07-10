using Beached.Content.Defs.Entities;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Foods;
using System.Collections.Generic;
using System.Linq;

namespace Beached.Content.ModDb
{
	public class BDb
	{
		public const NotificationType BeachedTutorialMessage = (NotificationType)351;
		public const string poisBuildCategory = "Beached_POIs";

		public static BPlushies plushies;
		public static BGeyserTraits geyserTraits;
		public static LootTables lootTables;
		public static BKaracooSkins karacooSkins;

		public static class FoodTypes
		{
			public static Diet.Info.FoodType GermDiet = (Diet.Info.FoodType)Hash.SDBMLower("Beached_GermDiet");
		}

		public static void OnDbInit(Db db)
		{
			plushies = new BPlushies();
			geyserTraits = new BGeyserTraits();
			lootTables = new LootTables(db.Root);
			karacooSkins = new BKaracooSkins(db.Root);

			BGeyserTraits.Register();

			TUNING.CREATURES.SORTING.CRITTER_ORDER[SlickShellConfig.ID] = BaseSnailConfig.SORTING_ORDER;
			TUNING.CREATURES.SORTING.CRITTER_ORDER[JellyConfig.ID] = BaseJellyfishConfig.SORTING_ORDER;

			HashSet<string> cookingStations =
				[
					MicrobeMusherConfig.ID,
					CookingStationConfig.ID,
					GourmetCookingStationConfig.ID,
					DeepfryerConfig.ID,
					FoodDehydratorConfig.ID,
					"CrittersDropBones_Cooker",
					SmokerConfig.ID,
				];

			foreach (var cookingStation in cookingStations)
			{
				var prefab = Assets.TryGetPrefab(cookingStation);
				if (prefab != null && prefab.TryGetComponent(out BuildingComplete building))
				{
					if (!Game.IsCorrectDlcActiveForCurrentSave(building.Def))
						continue;

					building.AddTag(BTags.cookingStation);
				}
			}
		}


		internal static void OnPostEntitiesLoaded()
		{
			BEntities.OnPostEntitiesLoaded();
		}

		public class WearableTypes
		{
			public static readonly WearableAccessorizer.WearableType jewellery = (WearableAccessorizer.WearableType)Hash.SDBMLower("Beached_jewellery");
		}

		public static void SetMeatTags()
		{
			HashSet<Tag> meats =
				[
					CookedMeatConfig.ID,
					CookedFishConfig.ID,
					BurgerConfig.ID,
					MeatConfig.ID,
					FishMeatConfig.ID,
					ShellfishMeatConfig.ID,
					SurfAndTurfConfig.ID,
					DeepFriedShellfishConfig.ID,
					DeepFriedFishConfig.ID,
					DeepFriedMeatConfig.ID,

					DinosaurMeatConfig.ID,
					SmokedDinosaurMeatConfig.ID,
					SmokedFish.ID,
					// TODO rest of the meats

					// Twitch Integration
					"ONITwitch.GlitterMeatConfig",
					// Canned Food
					"CF_CannedBBQ",
					"CF_CannedTuna",
					// Vietnamese Food
					"GrilledCutlet",
					// OBJN RECIPE THAI FOOD

					// more food
					// Dupes Cuisine
					// 川菜 -K14
				];

			HashSet<Tag> nonVegetarians =
				[
					// Vietnamese Food
					"NuocLeo",
					"Pho",
					"MiQuang",
				];

			HashSet<Tag> liceFoods =
				[

				BasicPlantFoodConfig.ID,
				PickledMealConfig.ID,
				BasicPlantBarConfig.ID,
				// Canned Lice
				"SpaceFood"
				];

			HashSet<Tag> mimilletFoods =
				[
				ButterflyFoodConfig.ID
				];

			foreach (var meat in meats)
			{
				var prefab = Assets.TryGetPrefab(meat);
				if (prefab != null)
					prefab.AddTag(BTags.meat);
			}

			if (Mod.settings.General.IsMealLiceMeat)
			{
				meats = [.. meats.Union(liceFoods)];
			}

			foreach (var recipe in ComplexRecipeManager.Get().recipes)
			{
				if (recipe.ingredients.Any(ingredient => IsMeat(ingredient, meats)))
				{
					foreach (var result in recipe.results)
					{
						if (meats.Contains(result.material))
							continue;

						var foodInfo = EdiblesManager.GetFoodInfo(result.material.name);
						if (foodInfo != null && foodInfo.CaloriesPerUnit > 0)
						{
							Log.Debug($"Automatically added {result.material} to meaty items");
							meats.Add(result.material);
						}
					}
				}
			}

			if (!Mod.settings.General.IsMealLiceMeat)
			{
				foreach (var lice in liceFoods)
				{
					var prefab = Assets.TryGetPrefab(lice);
					if (prefab != null)
						prefab.AddTag(BTags.nonVegetarian);
				}
			}

			if (!Mod.settings.General.IsMimilletVegetarian)
			{
				foreach (var food in mimilletFoods)
				{
					var prefab = Assets.TryGetPrefab(food);
					if (prefab != null)
						prefab.AddTag(BTags.nonVegetarian);
				}
			}

			foreach (var food in nonVegetarians)
			{
				var prefab = Assets.TryGetPrefab(food);
				if (prefab != null)
					prefab.AddTag(BTags.nonVegetarian);
			}
		}

		private static bool IsMeat(ComplexRecipe.RecipeElement ingredient, HashSet<Tag> meats)
		{
			if (ingredient.possibleMaterials != null && ingredient.possibleMaterials.Length > 0)
			{
				foreach (var material in ingredient.possibleMaterials)
				{
					if (meats.Contains(material))
						return true;
				}
			}

			return meats.Contains(ingredient.material);
		}

	}
}
