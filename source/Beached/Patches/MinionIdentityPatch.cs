using Beached.Content.Defs.Duplicants;
using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class MinionIdentityPatch
	{
		//[HarmonyPatch(typeof(MinionIdentity), nameof(MinionIdentity.OnSpawn))]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static void Prefix(MinionIdentity __instance)
			{
				if (__instance == null) Log.Warning("dupe is null");

				var controller = __instance.GetComponent<SymbolOverrideController>();
				var accessorizer = __instance.GetComponent<Accessorizer>();

				Log.Debug("Accessories for: " + __instance.name);

				foreach(var accessories in accessorizer.accessories)
				{
					Log.Debug(accessories.Get().Id);
				}

				var accessory = accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadShape);
				if (accessory == null) Log.Warning("accessory is null");
				if (accessory.symbol == null) Log.Warning("accessory.symbol is null");

				var symbolName = HashCache.Get().Get(accessory.symbol.hash);
				Log.Debug("SYMBOL NAME: " + symbolName);

				string text = HashCache
					.Get()
					.Get(accessory.symbol.hash)
					.Replace("headshape", "cheek");
				Log.Debug("REPLCAED NAME: " + text);
			}

#if TRANSPILERS
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
			{
				var m_RemapSymbolName = AccessTools.Method(
					typeof(MinionIdentity_OnSpawn_Patch),
					"RemapAnimFileName",
					new[]
					{
						typeof(string),
						typeof(MinionIdentity)
					});

				var codes = orig.ToList();
				var index = codes.FindIndex(c => c.opcode == OpCodes.Ldstr && c.operand is string str && str == "head_swap_kanim");

				if (index == -1)
					return codes;

				codes.InsertRange(index + 1, new[]
				{
                    // string on stack
                    new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_RemapSymbolName)
				});

				return codes;
			}

			private static string RemapAnimFileName(string originalKanimFile, MinionIdentity identity)
			{
				if (identity != null
					&& BDuplicants.headKanims.TryGetValue(identity.personalityResourceId, out var anim)
					&& anim != null)
					return anim;

				return originalKanimFile;
			}
#endif
		}
	}
}
