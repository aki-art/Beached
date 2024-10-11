using Beached.Content.Scripts.Entities;
using HarmonyLib;

namespace Beached.Patches
{
	public class FertilityMonitor_InstancePatch
	{
		[HarmonyPatch(typeof(FertilityMonitor.Instance), "LayEgg")]
		public class FertilityMonitor_Instance_LayEgg_Patch
		{
			public static void Prefix(FertilityMonitor.Instance __instance)
			{
				// TODO. not hardcoded

				if (__instance.master == null || __instance.master.gameObject == null)
				{
					Log.Debug("master is null");
					return;
				}

				if (__instance.master.gameObject.TryGetComponent(out Beached_HatchlingCanReachLandChecker checker))
					checker.UpdateChances();

				// remove chance for laying a pip egg if there is no dry land nearby. the baby squirrel can be stuck but
				// we just assume merpips are dumb like a fish
				/*	if (__instance.gameObject.PrefabID() == MerpipConfig.ID)
					{
						Log.Debug("merpip laying egg");
						var navigator = __instance.gameObject.GetComponent<Navigator>();

						var cellQuery = BPathFinderQueries.dryLandQuery.Reset(1);
						navigator.RunQuery(cellQuery);

						var flags = PathFinder.PotentialPath.Flags.None;

						// find floor cell under where egg will drop
						var floorCell = Grid.PosToCell(__instance);

						while (!Grid.IsSolidCell(floorCell))
							floorCell = Grid.CellBelow(floorCell);

						var cell = Grid.CellAbove(floorCell);

						var grid = Pathfinding.Instance.GetNavGrid(CONSTS.NAV_GRID.PIP);

						PathFinder.Run(
							grid,
							navigator.GetCurrentAbilities(),
							new PathFinder.PotentialPath(cell, NavType.Floor, flags),
							cellQuery);

						if (cellQuery.results.Count == 0)
						{
							SetBreedingChance(__instance, SquirrelConfig.EGG_ID, 0f);
							SetBreedingChance(__instance, MerpipConfig.EGG_ID, 1f);
						}*/
			}
		}

		private static void SetBreedingChance(FertilityMonitor.Instance monitor, string eggId, float value)
		{
			foreach (var breedingChance in monitor.breedingChances)
			{
				if (breedingChance.egg == eggId)
				{
					breedingChance.weight = value;
					break;
				}
			}

			monitor.NormalizeBreedingChances();
			monitor.master.Trigger((int)GameHashes.BreedingChancesChanged, monitor.breedingChances);
		}
	}
}
