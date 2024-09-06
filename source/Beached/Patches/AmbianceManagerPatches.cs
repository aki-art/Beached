using Beached.Content;
using FMODUnity;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Beached.Patches
{
	public class AmbianceManagerPatches
	{
		[HarmonyPatch(typeof(AmbienceManager), "OnSpawn")]
		public class AmbienceManager_OnSpawn_Patch
		{
			public static void Prefix(AmbienceManager __instance)
			{
				if (!RuntimeManager.IsInitialized)
					return;

				foreach (var def in __instance.quadrantDefs)
				{
					// padding for the useless NumTypes
					if (def.solidSounds.Length == 19)
					{
						var dirt = def.solidSounds[(int)SolidAmbienceType.Ice];
						def.solidSounds = def.solidSounds.AddToArray(dirt); // 19
					}

					// adding my events
					Elements.crystalAmbiance = (SolidAmbienceType)def.solidSounds.Length; // 20
					var crystal = RuntimeManager.PathToEventReference("event:/beached/Environment/crystal_ambience");
					def.solidSounds = def.solidSounds.AddToArray(crystal);

					var substanceTable = Assets.instance.substanceTable;
					substanceTable.GetSubstance(Elements.aquamarine).audioConfig.solidAmbienceType = Elements.crystalAmbiance;
				}
			}
		}

		[HarmonyPatch(typeof(AmbienceManager.Quadrant), MethodType.Constructor, [typeof(AmbienceManager.QuadrantDef)])]
		public class AmbienceManager_Quadrant_Ctor_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var f_allLayers = AccessTools.DeclaredField(typeof(AmbienceManager.Quadrant), nameof(AmbienceManager.Quadrant.allLayers));

				var index = codes.FindIndex(ci => ci.StoresField(f_allLayers));

				if (index == -1)
				{
					Log.Warning("Could not patch AmbienceManager.Quadrant Constructor");
					return codes;
				}

				var m_AddNewLayers = AccessTools.DeclaredMethod(typeof(AmbienceManager_Quadrant_Ctor_Patch), nameof(AddNewLayers));

				codes.InsertRange(index + 1,
					[
						new CodeInstruction(OpCodes.Ldarg_0),
						new CodeInstruction(OpCodes.Call, m_AddNewLayers)
					]);

				return codes;
			}

			private static bool padForNumTypes;

			private static void AddNewLayers(AmbienceManager.Quadrant instance)
			{
				var additionalLayers = 1; // amount of elements i add

				// padding for NumTypes, but only if no other mod has done so yet, just need to skip specifically 19 (or the original hardcoded length of solidLayers)
				if (instance.solidLayers.Length == 19 || padForNumTypes)
				{
					padForNumTypes = true; // caching this so on game reload we remember
					additionalLayers += 1;
				}

				Array.Resize(ref instance.solidLayers, instance.solidLayers.Length + additionalLayers);
			}
		}
	}
}
