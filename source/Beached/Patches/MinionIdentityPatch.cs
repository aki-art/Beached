using HarmonyLib;

namespace Beached.Patches
{
	public class MinionIdentityPatch
	{
		[HarmonyPatch(typeof(MinionIdentity), nameof(MinionIdentity.OnSpawn))]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static void Prefix(MinionIdentity __instance)
			{
				var controller = __instance.GetComponent<SymbolOverrideController>();
				var accessorizer = __instance.GetComponent<Accessorizer>();

				var symbolName = HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadShape).symbol.hash);
				Log.Debug("SYMBOL NAME: " + symbolName);

				string text = HashCache
					.Get()
					.Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadShape).symbol.hash)
					.Replace("headshape", "cheek");
				Log.Debug("REPLCAED NAME: " + text);
			}

#if TRANSPILERS
/*			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
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
				return identity != null && identity.personalityResourceId == (HashedString)MinnowConfig.ID ? "minnow_head_kanim" : originalKanimFile;
			}*/
#endif
		}
	}
}
