using FUtility.FUI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace Beached.Patches
{
	public class WaterCubesPatch
	{
		[HarmonyPatch(typeof(WaterCubes), nameof(WaterCubes.Init))]
		public class WaterCubes_Init_Patch
		{
			public static void Postfix(WaterCubes __instance)
			{
				SetWaterLayer(__instance);
				FadeWaterOpaqueness(__instance);
				CreateDisplacementOverlayMesh(__instance);
			}

			private static void CreateDisplacementOverlayMesh(WaterCubes waterCubes)
			{
				var gameObject = new GameObject("Beached_WaterWobbleMesh");

				var meshFilter = gameObject.AddComponent<MeshFilter>();

				var meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = ModAssets.Materials.liquidRefractionMat;
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.receiveShadows = false;
				meshRenderer.lightProbeUsage = LightProbeUsage.Off;
				meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

				meshFilter.sharedMesh = waterCubes.CreateNewMesh();
				meshRenderer.gameObject.layer = 0;
				meshRenderer.gameObject.transform.parent = waterCubes.transform;
				meshRenderer.gameObject.transform.position = new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.Liquid));

				gameObject.SetActive(true);
			}

			private static void FadeWaterOpaqueness(WaterCubes waterCubes)
			{
				waterCubes.material.SetFloat("_BlendScreen", 0.5f);
			}

			private static void SetWaterLayer(WaterCubes waterCubes)
			{
				Log.Debug(waterCubes.name);
				var waterCubesMesh = waterCubes.transform.Find("WaterCubesMesh");

				if (waterCubesMesh != null)
					waterCubesMesh.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Water") | waterCubesMesh.gameObject.layer);
				else
				{
					Log.Warning("WATERCUBES IS NULL");
					Helper.ListChildren(waterCubes.transform);
				}
			}
		}
	}
}
