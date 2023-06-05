using Beached.Content.BWorldGen;
using HarmonyLib;
using System.Linq;
using UnityEngine;
namespace Beached.Patches
{
	internal class TemplateLoaderPatch
	{

		[HarmonyPatch(typeof(TemplateLoader), nameof(TemplateLoader.Stamp))]
		public class TemplateLoader_Stamp_Patch
		{
			public static void Postfix(TemplateContainer template, Vector2 rootLocation)
			{
				if (template.info?.tags != null && template.info.tags.Contains(BWorldGenTags.Reefify))
				{
					foreach (var offset in template.cells)
					{
						var cell = Grid.PosToCell(new Vector2(offset.location_x + rootLocation.x, offset.location_y + rootLocation.y));
						BeachedGrid.Instance.zoneTypeOverrides[cell] = ZoneTypes.coralReef;
					}

					BeachedGrid.Instance.RegenerateBackwallTexture();
				}
			}
		}
	}
}
