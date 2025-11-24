using Beached.Content.ModDb;
using HarmonyLib;

namespace Beached.Patches
{
	public class InventoryOrganizationPatch
	{
		[HarmonyPatch(typeof(InventoryOrganization), "GenerateSubcategories")]
		public class InventoryOrganization_GenerateSubcategories_Patch
		{
			public static void Postfix()
			{
				BFacades.ConfigureSubCategories();
			}
		}
	}
}
