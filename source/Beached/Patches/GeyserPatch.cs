using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	internal class GeyserPatch
	{
		[HarmonyPatch(typeof(Geyser), "GenerateName")]
		public class Geyser_GenerateName_Patch
		{
			public static void Postfix(Geyser __instance)
			{
				if (__instance.TryGetComponent(out Beached_GeyserTraits traits))
					traits.ModifyName();
			}
		}
	}
}
