using HarmonyLib;
using KMod;

namespace Beached.Patches
{
	public class ModPatch
	{
		[HarmonyPatch(typeof(KMod.Mod), "LoadAnimation")]
		public class KMod_Mod_LoadAnimation_Patch
		{
			public static void Postfix(KMod.Mod __instance)
			{
				Log.Debug($"loading mod anims " + __instance.staticID);
			}
		}


		[HarmonyPatch(typeof(Manager), "Load")]
		public class Manager_Load_Patch
		{
			public static void Postfix(KMod.Content content)
			{
				Log.Debug($"loading {content}");
				if (content == KMod.Content.Animation)
				{
					Log.Debug("loading animations");
					ModAssets.LoadKAnims();
				}
			}
		}
	}
}
