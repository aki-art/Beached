using HarmonyLib;
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
		//[HarmonyPatch(typeof(WaterCubes), nameof(WaterCubes.Init))]
		public class WaterCubes_Init_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				// find injection point
				var index = codes.FindIndex(ci => ci.opcode == OpCodes.Stloc_0);

				if (index == -1)
				{
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(WaterCubes_Init_Patch), "InjectedMethod");

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					 new CodeInstruction(OpCodes.Ldloc_1),
					 new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static void CacheMesh(MeshFilter mesh)
			{
				WaterCubesPatch.mesh = mesh;
			}

			public static void Postfix(WaterCubes __instance)
			{
				if (mesh != null)
				{
					mesh.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Water"));
					mesh.gameObject.layer = LayerMask.NameToLayer("Water");
				}

				// make the liquids a little more see-through
				__instance.material.SetFloat("_BlendScreen", 0.5f);

				var gameObject = new GameObject("Beached_WaterWobbleMesh");


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
				meshRenderer.gameObject.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));

				gameObject.SetActive(true);
			}
		}
#endif
	}
}
