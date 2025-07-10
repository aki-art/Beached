using HarmonyLib;

namespace Beached.Patches
{
	internal class GridSettingsPatch
	{

		[HarmonyPatch(typeof(GridSettings), "Reset")]
		public class GridSettings_Reset_Patch
		{
			public static void Postfix()
			{
				Beached_Grid.hasClimbable = new bool[Grid.CellCount];
			}
		}
	}
}
