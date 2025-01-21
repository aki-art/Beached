using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class SteppedInMonitorPatch
	{
		[HarmonyPatch(typeof(SteppedInMonitor), nameof(SteppedInMonitor.IsOnCarpet))]
		public class SteppedInMonitor_IsOnCarpet_Patch
		{
			public static void Postfix(ref bool __result, SteppedInMonitor.Instance smi)
			{
				if (!__result)
				{
					var cell = Grid.CellBelow(Grid.PosToCell(smi));
					__result = Grid.IsValidCell(cell) && Grid.Element[cell].id == Elements.moss;
				}
			}
		}
	}
}
