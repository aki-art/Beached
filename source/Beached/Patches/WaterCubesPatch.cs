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
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_AddComponent = typeof(GameObject)
					.GetMethod("AddComponent", new Type[] { })
					.MakeGenericMethod(typeof(MeshFilter));

				var index = codes.FindIndex(ci => ci.Calls(m_AddComponent));

				if (index == -1)
				{
					Log.Warning("Could not patch WaterCubes.Init");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(WaterCubes_Init_Patch), nameof(CacheMesh));

				// inject right at the found index
				codes.InsertRange(index, new[]
				{
					 new CodeInstruction(OpCodes.Dup),
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
				Log.Debug("postfix");
				mesh ??= __instance.cubes.GetComponentInChildren<MeshFilter>();

				Log.Debug("1");
				FUtility.FUI.Helper.ListChildren(__instance.cubes.transform);

				Log.Debug("2");
				if (mesh != null)
				{
					Log.Debug("3");
					mesh.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Water"));
					Log.Debug("3b");
				}
				else
					Log.Warning("WaterCubes mesh is null");

				// make the liquids a little more see-through
				//__instance.material.SetFloat("_BlendScreen", 0.5f);

				Log.Debug("4");
				var gameObject = new GameObject("Beached_WaterWobbleMesh");

				Log.Debug("4b");

				if (__instance.cubes == null)
					Log.Warning("cubes is null");

				gameObject.transform.parent = __instance.cubes.transform;

				Log.Debug("5");
				var meshFilter = gameObject.AddComponent<MeshFilter>();

				var meshRenderer = gameObject.AddComponent<MeshRenderer>();
				Log.Debug("6");
				meshRenderer.sharedMaterial = ModAssets.Materials.liquidRefractionMat;
				Log.Debug("7");
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.receiveShadows = false;
				meshRenderer.lightProbeUsage = LightProbeUsage.Off;
				meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

				Log.Debug("8");
				meshFilter.sharedMesh = __instance.CreateNewMesh();
				Log.Debug("9");
				meshRenderer.gameObject.layer = 0;
				meshRenderer.gameObject.transform.parent = __instance.transform;
				meshRenderer.gameObject.transform.position = new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.Liquid));

				Log.Debug("10");
				gameObject.SetActive(true);

				Log.Debug("postfix end");
			}
		}
#endif
	}
}
