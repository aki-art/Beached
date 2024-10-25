using Beached.Content.BWorldGen;
using System.Collections.Generic;
using System.Linq;

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

					if (template != null
						&& template.info?.tags != null
						&& template.info.tags.Contains(BWorldGenTags.Reefify))
					{
						var originCell = Grid.XYToCell(position.X, position.Y);
						Beached_Grid.worldgenZoneTypes ??= [];

						foreach (var offset in template.cells)
						{
							int cell = Grid.OffsetCell(Grid.XYToCell(position.x, position.y), offset.location_x, offset.location_y);

							if (Grid.IsValidCell(cell))
								Beached_Grid.worldgenZoneTypes[Grid.CellToXY(cell)] = ZoneTypes.coralReef;
						}
					}
				}
			}
		}
	}
}
