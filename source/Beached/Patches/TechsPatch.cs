using Beached.Content.ModDb;
using Database;
using HarmonyLib;

namespace Beached.Patches
{
	public class TechsPatch
	{
		[HarmonyPatch(typeof(Techs), "Init")]
		public class Techs_Init_Patch
		{
			public static void Prefix(Techs __instance)
			{
				BTechs.Register(__instance);
			}
		}
	}
}
