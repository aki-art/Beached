using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class PropertyTexturesPatch
	{
		// set the liquid texture for the wavy shader
		[HarmonyPatch(typeof(PropertyTextures), nameof(PropertyTextures.OnReset))]
		public class PropertyTextures_OnReset_Patch
		{
			public static void Postfix(PropertyTextures __instance)
			{
				ModAssets.Materials.liquidRefractionMat.SetTexture(
					"_LiquidTex",
					__instance.externallyUpdatedTextures[(int)PropertyTextures.Property.Liquid]);
			}
		}

		// Makes the Salty Oxygen texture the lighter texture Oxygen uses
		[HarmonyPatch(typeof(PropertyTextures), nameof(PropertyTextures.UpdateDanger))]
		public static class PropertyTextures_UpdateDanger_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var index = codes.FindIndex(ci => ci.LoadsConstant((int)SimHashes.Oxygen)); /// 34 ldc.i4 SimHashes.Oxygen

				if (index == -1)
					return codes;

				var m_GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), nameof(GetDangerForElement), [typeof(int), typeof(int)]);

				codes.InsertRange(index + 3,
				[
					// byte.maxValue is loaded to the stack
					new CodeInstruction(OpCodes.Ldloc_2), // load num to the stack
					new CodeInstruction(OpCodes.Call, m_GetDangerForElement)
				]);

				return codes;
			}

			// Calling with existing value so there is a possibility for other mods to also add their own values
			private static byte GetDangerForElement(int existingValue, int cell)
			{
				return (Grid.Element[cell].id == Elements.saltyOxygen)
					? (byte)0
					: (byte)existingValue;
			}
		}
	}
}
