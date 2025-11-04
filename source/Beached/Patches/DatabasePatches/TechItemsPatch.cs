using Beached.Content.ModDb;
using Database;
using HarmonyLib;

namespace Beached.Patches.DatabasePatches
{
	public class TechItemsPatch
	{
		[HarmonyPatch(typeof(TechItems), "Init")]
		public class TechItems_Init_Patch
		{
			public static void Postfix(TechItems __instance)
			{
				BTechItems.OnInit(__instance);
			}
		}
	}
}
