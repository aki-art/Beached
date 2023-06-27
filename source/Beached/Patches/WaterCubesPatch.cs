using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Rendering;

namespace Beached.Patches
{
	public class WaterCubesPatch
	{
		private static MeshFilter mesh;
#if TRANSPILERS
		[HarmonyPatch(typeof(WaterCubes), nameof(WaterCubes.Init))]
		public class WaterCubes_Init_Patch
		{
/*			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_AddComponent = typeof(GameObject)
					.GetMethod("AddComponent", new Type[] { })
					.MakeGenericMethod(typeof(MeshFilter));

				var m_layer_setter = typeof(GameObject).GetProperty("layer").GetSetMethod();

				var index = codes.FindIndex(ci => ci.Calls(m_layer_setter));

				if (index == -1)
				{
					Log.Warning("Could not patch WaterCubes.Init");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(WaterCubes_Init_Patch), nameof(GetLayer));

				// inject right at the found index
				codes.InsertRange(index, new[]
				{
					 new CodeInstruction(OpCodes.Dup),
					 new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static int GetLayer(int layer)
			{
				return layer | LayerMask.NameToLayer("Water");
			}
*/
			public static void Postfix(WaterCubes __instance)
			{
				var waterCubesMesh = __instance.transform.Find("WaterCubesMesh");

				if (waterCubesMesh != null)
					waterCubesMesh.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Water")); // | waterCubesMesh.gameObject.layer);

				Log.Debug(waterCubesMesh.transform.parent.name);

				// make the liquids a little more see-through
				__instance.material.SetFloat("_BlendScreen", 0.5f);

				var gameObject = new GameObject("Beached_WaterWobbleMesh");

				if (__instance.cubes == null)
					Log.Warning("cubes is null");

				gameObject.transform.parent = __instance.cubes.transform;

				var meshFilter = gameObject.AddComponent<MeshFilter>();

				var meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = ModAssets.Materials.liquidRefractionMat;
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.receiveShadows = false;
				meshRenderer.lightProbeUsage = LightProbeUsage.Off;
				meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

				meshFilter.sharedMesh = __instance.CreateNewMesh();
				meshRenderer.gameObject.layer = 0;
				meshRenderer.gameObject.transform.parent = __instance.transform;
				meshRenderer.gameObject.transform.position = new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.Liquid));

				gameObject.SetActive(true);
			}
		}
#endif
	}
}
