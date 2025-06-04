using Beached.Content.Defs.Equipment;
using Database;

namespace Beached.Content.ModDb
{
	public class BEquippableFacades
	{
		public static class BEACHSHIRTS
		{
			public const string
				GREEN = "Beached_Beachshirt_Green",
				BLUE = "Beached_Beachshirt_Blue",
				BLACK = "Beached_Beachshirt_Black",
				RETRO = "Beached_Beachshirt_Retro";
		}

		[DbEntry]
		public static void Register(EquippableFacades __instance)
		{
			AddShirt(__instance, BEACHSHIRTS.GREEN, "beached_beachshirt_green_kanim", "beached_beachshirt_green_item_kanim");
			AddShirt(__instance, BEACHSHIRTS.BLUE, "beached_beachshirt_blue_kanim", "beached_beachshirt_blue_item_kanim");
			AddShirt(__instance, BEACHSHIRTS.BLACK, "beached_beachshirt_black_kanim", "beached_beachshirt_black_item_kanim");
			AddShirt(__instance, BEACHSHIRTS.RETRO, "beached_beachshirt_retro_kanim", "beached_beachshirt_retro_item_kanim");
		}

		private static void AddShirt(EquippableFacades __instance, string id, string anim, string itemAnim)
		{
			string partialId = id.Replace("Beached_Beachshirt_", "");

			__instance.Add(
				id,
				Strings.Get($"STRINGS.EQUIPMENT.PREFABS.BEACHED_BEACHSHIRT.FACADES.{partialId.ToUpperInvariant()}"),
				Strings.Get($"STRINGS.EQUIPMENT.PREFABS.BEACHED_BEACHSHIRT.FACADES.{partialId.ToUpperInvariant()}_DESCRIPTION"),
				PermitRarity.Universal,
				BeachShirtConfig.ID,
				anim,
				itemAnim,
				null,
				null);

			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(id);
		}
	}
}
