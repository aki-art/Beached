using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class WoodTileConfigPatch
	{
		[HarmonyPatch(typeof(WoodTileConfig), "CreateBuildingDef")]
		public class WoodTileConfig_CreateBuildingDef_Patch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				var currentCat = __result.MaterialCategory[0];
				if (currentCat == SimHashes.WoodLog.ToString())
				{
					__result.MaterialCategory = [GameTags.BuildingWood.ToString()];
				}
				else
				{
					var element = ElementLoader.FindElementByName(currentCat);
					var isElement = element != null;
					var isItem = Assets.TryGetPrefab(currentCat) != null;

					if (!isElement && !isItem)
					{
						var bamboo = ElementLoader.FindElementByHash(Elements.bambooStem);
						bamboo.oreTags = bamboo.oreTags.AddToArray(currentCat);
					}
					else
					{
						Log.Warning($"Someone has changes Wood Tile's construction material to {currentCat}, cannot add Bamboo as an option :(.");
					}
				}
			}
		}
	}
}
