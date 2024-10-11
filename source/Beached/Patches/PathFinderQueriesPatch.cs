using Beached.Content.Scripts.CellQueries;
using HarmonyLib;

namespace Beached.Patches
{
	public class PathFinderQueriesPatch
	{
		[HarmonyPatch(typeof(PathFinderQueries), "Reset")]
		public class PathFinderQueries_Reset_Patch
		{
			public static void Postfix()
			{
				BPathFinderQueries.Reset();
			}
		}
	}
}
