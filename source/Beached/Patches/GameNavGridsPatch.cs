using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class GameNavGridsPatch
	{
		[HarmonyPatch(typeof(GameNavGrids), MethodType.Constructor, typeof(Pathfinding))]
		public class GameNavGrids_Ctor_Patch
		{
			public static void Postfix(GameNavGrids __instance, Pathfinding pathfinding)
			{
				BNavGrids.CreateNavGrids(__instance, pathfinding);
			}
		}
	}
}
