using Beached.Content.ModDb;
using Database;
using HarmonyLib;

namespace Beached.Patches.DatabasePatches
{
	public class ArtableStagesPatch
	{
		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_TargetMethod_Patch
		{
			public static void Postfix(ArtableStages __instance)
			{
				BArtableStages.Register(__instance);
			}
		}
	}
}
