using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class RoomPatch
	{
		// not actually used for anything in the base game code, just here to inform other mods potentially
		[HarmonyPatch(typeof(Room), nameof(Room.GetPrimaryEntities))]
		public class Room_GetPrimaryEntities_Patch
		{
			// need to prefix skip because the game code here is actually bugged, and crashes with buildingless room types
			public static bool Prefix(Room __instance, ref List<KPrefabID> __result)
			{
				if (__instance.roomType == BRoomTypes.NatureVista)
				{
					var pois = __instance.cavity.GetNaturePOIs();

					if (pois == null)
						return true;

					__instance.primary_buildings ??= new();

					foreach (var poi in pois)
					{
						if (poi != null)
							__instance.primary_buildings.Add(poi);
					}

					__result = __instance.primary_buildings;

					return false;
				}

				return true;
			}
		}
	}
}
