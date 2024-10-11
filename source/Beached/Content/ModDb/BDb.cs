using Beached.Content.Defs.Entities;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Foods;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BDb
	{
		public const NotificationType BeachedTutorialMessage = (NotificationType)351;
		public const string poisBuildCategory = "Beached_POIs";

		public static BPlushies plushies;
		public static BGeyserTraits geyserTraits;
		public static LootTables lootTables;

		public static void OnDbInit(Db db)
		{
			plushies = new BPlushies();
			geyserTraits = new BGeyserTraits();
			lootTables = new LootTables(db.Root);

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
					if (!building.Def.IsValidDLC())
						continue;

					building.AddTag(BTags.cookingStation);
				}
			}
		}


		internal static void OnPostEntitiesLoaded()
		{
			MerpipConfig.ConfigureEggChancesToMerpip();
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

			if (!Mod.settings.General.IsMealLiceMeat)
			{
			}

			foreach (var meat in meats)
			{
				var prefab = Assets.TryGetPrefab(meat);
				if (prefab != null)
					prefab.AddTag(BTags.meat);
			}

			var liceTag = Mod.settings.General.IsMealLiceMeat ? BTags.meat : BTags.nonVegetarian;

			foreach (var lice in liceFoods)
			{
				var prefab = Assets.TryGetPrefab(lice);
				if (prefab != null)
					prefab.AddTag(liceTag);
			}
		}
	}
}
