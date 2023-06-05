using Beached.Content.Scripts.Entities;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Beached.Patches
{
	public class RoomProberPatch
	{
		// tracks vista pois and which rooms contain them
		// Fast Track skips this method!!
		// TODO: transpiler
		//[HarmonyPatch(typeof(RoomProber), "RebuildDirtyCavities")]
		public class RoomProber_RebuildDirtyCavities_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetComponent = AccessTools
					.Method(typeof(Component), "GetComponent")
					.MakeGenericMethod(typeof(Deconstructable));

				var index = codes.FindIndex(ci => ci.Calls(m_GetComponent));

				if (index == -1)
				{
					Log.Warning("Unable to patch RoomProber.RebuildDirtyCavities, skipping; Vista rooms will not work.");
					return codes;
				}

				var m_ReplacementMethod = AccessTools.DeclaredMethod(typeof(RoomProber_RebuildDirtyCavities_Patch), "ReplacementMethod");

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldloc_S, 6), // kPrefabID was 6
                    new CodeInstruction(OpCodes.Ldloc_S, 4), // cavityInfo
                    new CodeInstruction(OpCodes.Call, m_ReplacementMethod)
				});

				return codes;
			}

			public static void ReplacementMethod(KPrefabID kPrefabID, CavityInfo cavityInfo)
			{
				if (kPrefabID.TryGetComponent(out Vista vista))
				{
					vista.UpdateRoom(cavityInfo);
				}
			}
		}
	}
}
