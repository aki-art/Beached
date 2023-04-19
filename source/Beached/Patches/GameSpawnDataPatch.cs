using Beached.Content.BWorldGen;
using Beached.Content.Defs;
using HarmonyLib;
using ProcGenGame;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Patches
{
    public class GameSpawnDataPatch
    {

        //[HarmonyPatch(typeof(WorldGenSimUtil), "DoSettleSim")]
        public class WorldGenSimUtil_DoSettleSim_Patch
        {
            public static void Postfix(List<KeyValuePair<Vector2I, TemplateContainer>> templateSpawnTargets)
            {
                foreach (var templateSpawnTarget in templateSpawnTargets)
                {
                    var template = templateSpawnTarget.Value;
                    var position = templateSpawnTarget.Key;

                    if (template.info?.tags != null && template.info.tags.Contains(BWorldGenTags.Reefify))
                    {
                        var originCell = Grid.XYToCell(position.X, position.Y);

                        BeachedGrid.worldgenZoneTypes ??= new();

                        foreach (var offset in template.cells)
                        {
                            int cell = Grid.OffsetCell(Grid.XYToCell(position.x, position.y), offset.location_x, offset.location_y);
                            Log.Debug(cell);

                            if (Grid.IsValidCell(cell))
                            {
                                BeachedGrid.worldgenZoneTypes[Grid.CellToXY(cell)] = ZoneTypes.coralReef;
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(GameSpawnData), "AddTemplate")]
        public class GameSpawnData_AddTemplate_Patch
        {
            public static void Prefix(TemplateContainer template, Vector2I position, ref Dictionary<int, int> claimedCells)
            {
                Log.Debug("spawning template");
                if (template.info?.tags != null && template.info.tags.Contains(BWorldGenTags.Reefify))
                {
                    var originCell = Grid.XYToCell(position.X, position.Y);

                    BeachedGrid.worldgenZoneTypes ??= new();
                    
                    foreach (var offset in template.cells)
                    {
                        //var cell = Grid.PosToCell(new Vector2(offset.location_x + position.x, offset.location_y + position.y));
                        var cell = Grid.OffsetCell(originCell, offset.location_x, offset.location_y);
                        Log.Debug(cell);

                        if (!claimedCells.ContainsKey(cell))
                        {
                            BeachedGrid.worldgenZoneTypes[Grid.CellToXY(cell)] = ZoneTypes.coralReef;
                        }
                    }
                }
            }
        }
    }
}
