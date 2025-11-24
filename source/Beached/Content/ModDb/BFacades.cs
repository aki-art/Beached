using Database;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BFacades
	{
		public static Dictionary<string, string> categories = new();

		public static HashSet<string> myFacades = new();

		public class SUB_CATEGORIES
		{
			public const string
				BEACHED_AQUA_WALLS = "BEACHED_WALLPAPERS_AQUA";
		}

		public class WALLS
		{
			public const string
				BEACHED_WALL_SKIN_AQUA_SILT = "beached_wall_skin_aqua_silt",
				BEACHED_WALL_SKIN_AQUA_WATER = "beached_wall_skin_aqua_water",
				BEACHED_WALL_SKIN_AQUA_KELP_TOPPER = "beached_wall_skin_aqua_kelp_topper",
				BEACHED_WALL_SKIN_AQUA_BUBBLES = "beached_wall_skin_aqua_bubbles",
				BEACHED_WALL_SKIN_AQUA_KELP = "beached_wall_skin_aqua_kelp",
				BEACHED_WALL_SKIN_AQUA_KELP_END = "beached_wall_skin_aqua_kelp_end",
				BEACHED_WALL_SKIN_AQUA_JELLY_BLUE = "beached_wall_skin_aqua_jelly_blue",
				BEACHED_WALL_SKIN_AQUA_JELLY_PINK = "beached_wall_skin_aqua_jelly_pink",
				BEACHED_WALL_SKIN_AQUA_JELLY_GREEN = "beached_wall_skin_aqua_jelly_green",
				BEACHED_WALL_SKIN_AQUA_TROPICAL_PACU = "beached_wall_skin_aqua_tropical_pacu",
				BEACHED_WALL_SKIN_CORAL_SILT = "beached_wall_skin_coral_silt",
				BEACHED_WALL_SKIN_CORAL_BUBBLEGUM = "beached_wall_skin_coral_bubblegum",
				BEACHED_WALL_SKIN_CORAL_PINK = "beached_wall_skin_coral_pink",
				BEACHED_WALL_SKIN_CORAL_DEAD = "beached_wall_skin_coral_dead",
				BEACHED_WALL_SKIN_CORAL_PURPLE = "beached_wall_skin_coral_purple",
				BEACHED_WALL_SKIN_CORAL_YELLOW = "beached_wall_skin_coral_yellow",
				BEACHED_WALL_SKIN_CORAL_RED = "beached_wall_skin_coral_red",
				BEACHED_WALL_SKIN_CORAL_BLUE = "beached_wall_skin_coral_blue",
				BEACHED_WALL_SKIN_BAMBOO_GREEN = "beached_wall_skin_bamboo_green",
				BEACHED_WALL_SKIN_BAMBOO_RED = "beached_wall_skin_bamboo_red",
				BEACHED_WALL_SKIN_WAVES = "beached_wall_skin_waves",
				BEACHED_WALL_SKIN_SKYBLUE = "beached_wall_skin_skyblue";
		}

		public static void ConfigureSubCategories()
		{
			var aquaWalls = GetOrCreateSubCategory(
				SUB_CATEGORIES.BEACHED_AQUA_WALLS,
				InventoryOrganization.InventoryPermitCategories.WALLPAPERS,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("beached_wall_skin_coral_silt_kanim")));

			aquaWalls.AddRange([
				WALLS.BEACHED_WALL_SKIN_AQUA_WATER,
				WALLS.BEACHED_WALL_SKIN_AQUA_KELP_TOPPER,
				WALLS.BEACHED_WALL_SKIN_AQUA_BUBBLES,
				WALLS.BEACHED_WALL_SKIN_AQUA_SILT,
				WALLS.BEACHED_WALL_SKIN_AQUA_KELP,
				WALLS.BEACHED_WALL_SKIN_AQUA_KELP_END,
				WALLS.BEACHED_WALL_SKIN_AQUA_JELLY_BLUE,
				WALLS.BEACHED_WALL_SKIN_AQUA_JELLY_PINK,
				WALLS.BEACHED_WALL_SKIN_AQUA_JELLY_GREEN,
				WALLS.BEACHED_WALL_SKIN_AQUA_TROPICAL_PACU,
				WALLS.BEACHED_WALL_SKIN_CORAL_SILT]);


			var patternedWalls = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDING_WALLPAPER_PRINTS];

			patternedWalls.AddRange([
				WALLS.BEACHED_WALL_SKIN_CORAL_BUBBLEGUM,
				WALLS.BEACHED_WALL_SKIN_CORAL_PINK,
				WALLS.BEACHED_WALL_SKIN_CORAL_DEAD,
				WALLS.BEACHED_WALL_SKIN_CORAL_PURPLE,
				WALLS.BEACHED_WALL_SKIN_CORAL_YELLOW,
				WALLS.BEACHED_WALL_SKIN_CORAL_RED,
				WALLS.BEACHED_WALL_SKIN_CORAL_BLUE,
				WALLS.BEACHED_WALL_SKIN_BAMBOO_GREEN,
				WALLS.BEACHED_WALL_SKIN_BAMBOO_RED,
				WALLS.BEACHED_WALL_SKIN_WAVES,
				WALLS.BEACHED_WALL_SKIN_SKYBLUE]);
		}


		public static void Register(ResourceSet<BuildingFacadeResource> resource)
		{
			HashSet<string> walls = [
				"beached_wall_skin_aqua_water",
				"beached_wall_skin_aqua_kelp_topper",
				"beached_wall_skin_aqua_bubbles",
				"beached_wall_skin_aqua_silt",
				"beached_wall_skin_aqua_kelp",
				"beached_wall_skin_aqua_kelp_end",
				"beached_wall_skin_aqua_jelly_blue",
				"beached_wall_skin_aqua_jelly_pink",
				"beached_wall_skin_aqua_jelly_green",
				"beached_wall_skin_aqua_tropical_pacu",
				"beached_wall_skin_coral_silt",
				"beached_wall_skin_coral_bubblegum",
				"beached_wall_skin_coral_pink",
				"beached_wall_skin_coral_dead",
				"beached_wall_skin_coral_purple",
				"beached_wall_skin_coral_yellow",
				"beached_wall_skin_coral_red",
				"beached_wall_skin_coral_blue",
				"beached_wall_skin_bamboo_green",
				"beached_wall_skin_bamboo_red",
				"beached_wall_skin_waves",
			];

			foreach (var wall in walls)
			{
				AddFacade(resource, ExteriorWallConfig.ID, wall, $"{wall}_kanim");
			}
		}

		private static List<string> GetOrCreateSubCategory(string subCategory, string mainCategory, Sprite icon)
		{
			if (!InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subCategory))
			{
				InventoryOrganization.AddSubcategory(
					subCategory,
					icon,
					900,
					[]);

				InventoryOrganization.categoryIdToSubcategoryIdsMap[mainCategory].Add(subCategory);
			}

			return InventoryOrganization.subcategoryIdToPermitIdsMap[subCategory];
		}

		private static void AddFacade(ResourceSet<BuildingFacadeResource> resource, string buildingId, string id, string anim)
		{
			var name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.NAME")?.String;
			var desc = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.DESC")?.String;

			Add(resource,
				id,
				name,
				desc + STRINGS.UI.BEACHED_MISC.DESIGN_BY_BEACHED,
				PermitRarity.Universal,
				buildingId,
				anim);

			Log.Debug($"adding facade {id}");
			myFacades.Add(id);
		}

		public static void Add(
				ResourceSet<BuildingFacadeResource> set,
				string id,
				LocString name,
				LocString description,
				PermitRarity rarity,
				string prefabId,
				string animFile,
				Dictionary<string, string> workables = null)
		{
			set.resources.Add(new BuildingFacadeResource(id, name, description, rarity, prefabId, animFile, null, workables));
		}

	}
}
