/*using Beached.Content.Defs.Buildings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Patches
{
	public class ElectricalUtilityNetworkPatch
	{
		public static Dictionary<int, List<Wire>> additionalWireLists = new();


		[HarmonyPatch(typeof(ElectricalUtilityNetwork), MethodType.Constructor)]
		public class ElectricalUtilityNetwork_Ctor_Patch
		{
			public static void Postfix(ElectricalUtilityNetwork __instance)
			{
				if (__instance == null)
					return;

				if (additionalWireLists.ContainsKey(__instance.id))
					return;

				additionalWireLists.Add(__instance.id, []);
			}
		}


		[HarmonyPatch(typeof(ElectricalUtilityNetwork), "UpdateOverloadTime")]
		public class ElectricalUtilityNetwork_UpdateOverloadTime_Patch
		{
			public static void Prefix(ElectricalUtilityNetwork __instance, float dt, float watts_used, List<WireUtilityNetworkLink>[] bridgeGroups)
			{
				if (additionalWireLists == null || !additionalWireLists.TryGetValue(__instance.id, out var wireGroup))
					return;


				List<WireUtilityNetworkLink> bridgeGroup = bridgeGroups[rating];
				float num = Wire.GetMaxWattageAsFloat((Wire.WattageRating)rating) + TUNING.POWER.FLOAT_FUDGE_FACTOR;
				if ((double)watts_used > (double)num && (bridgeGroup != null && bridgeGroup.Count > 0 || wireGroup != null && wireGroup.Count > 0))
				{
					flag = true;
					wireList = wireGroup;
					utilityNetworkLinkList = bridgeGroup;
					break;
				}
		}

		[HarmonyPatch(typeof(ElectricalUtilityNetwork), "GetMaxSafeWattage")]
		public class ElectricalUtilityNetwork_GetMaxSafeWattage_Patch
		{
			public static void Postfix(ElectricalUtilityNetwork __instance, ref float __result)
			{
				if (__result < MediumWattageWireConfig.WATTAGE
					&& additionalWireLists != null
					&& additionalWireLists.TryGetValue(__instance.id, out var wireList))
				{
					if (wireList != null && wireList.Count > 0)
						__result = Wire.GetMaxWattageAsFloat(MediumWattageWireConfig.W4000);
				}
			}
		}



		[HarmonyPatch(typeof(ElectricalUtilityNetwork), "AddItem")]
		public class ElectricalUtilityNetwork_AddItem_Patch
		{
			public static bool Prefix(ElectricalUtilityNetwork __instance, object item)
			{
				if (item is Wire wire && wire.MaxWattageRating == MediumWattageWireConfig.W4000)
				{
					if (additionalWireLists.TryGetValue(__instance.id, out var wireList))
					{
						wireList.Add(wire);
					}

					__instance.allWires.Add(wire);
					__instance.timeOverloaded = Mathf.Max(__instance.timeOverloaded, wire.circuitOverloadTime);

					return false;
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(ElectricalUtilityNetwork), "Reset")]
		public class ElectricalUtilityNetwork_Reset_Patch
		{
			public static void Postfix(ElectricalUtilityNetwork __instance, UtilityNetworkGridNode[] grid)
			{
				var wires = additionalWireLists.Values.ToList();

				foreach (var wireGroup in wires)
				{
					foreach (var wire in wireGroup)
					{
						if (wire != null)
						{
							wire.circuitOverloadTime = __instance.timeOverloaded;
							var cell = Grid.PosToCell(wire.transform.GetPosition());
							var utilityNetworkGridNode = grid[cell] with
							{
								networkIdx = -1
							};

							grid[cell] = utilityNetworkGridNode;
						}
					}

					wireGroup.Clear();
				}

				additionalWireLists.Clear();
			}
		}
	}
}
*/