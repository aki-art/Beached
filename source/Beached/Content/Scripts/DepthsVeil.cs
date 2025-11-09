using HarmonyLib;
using ImGuiNET;
using System.Collections;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Beached.Content.Scripts
{
	public class DepthsVeil : KMonoBehaviour
	{
		public static DepthsVeil Instance;

		private GameObject veil;
		private Material material;
		private Material zoneTypeMaterial;
		public RenderTexture renderTexture;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		[HarmonyPatch(typeof(SubworldZoneRenderData), "OnActiveWorldChanged")]
		public class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Postfix(Texture2D ___indexTex)
			{
				if (Instance != null && Instance.material != null)
				{
					Instance.material.SetTexture("_ZoneTypeTex", ___indexTex);

					var color = ___indexTex.GetPixel(0, 0);
				}
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			return;

			veil = Instantiate(ModAssets.Fx.darkVeilOverlay);
			zoneTypeMaterial = ModAssets.Materials.zoneTypeMaskMaterial;
			material = veil.GetComponent<MeshRenderer>().material;

			veil.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
			veil.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

			OnShadersReloaded();
			ShaderReloader.Register(OnShadersReloaded);

			StartCoroutine(SetIndexNextFrame());
			veil.SetActive(true);
		}

		private IEnumerator SetIndexNextFrame()
		{
			yield return SequenceUtil.waitForEndOfFrame;
			SetZoneType(BWorldGen.ZoneTypes.depths);
		}

		public void OnShadersReloaded()
		{
			var indexTex = World.Instance.zoneRenderData.indexTex;
			renderTexture = new RenderTexture(indexTex.width, indexTex.height, 0);
			zoneTypeMaterial = ModAssets.Materials.zoneTypeMaskMaterial;
			zoneTypeMaterial.SetTexture("_ZoneIndices", World.Instance.zoneRenderData.indexTex);

			material.renderQueue = RenderQueues.WorldTransparent;
			material.SetTexture("_ZoneTypes", renderTexture);
			material.SetTexture("_LightBuffer", LightBuffer.Instance.Texture);
			material.SetFloat("_LightFuzziness", 0.3f);

			SetZoneType(BWorldGen.ZoneTypes.depths);
		}

		private void LateUpdate()
		{
			if (renderTexture != null && zoneTypeMaterial != null)
				Graphics.Blit(World.Instance.zoneRenderData.indexTex, renderTexture, zoneTypeMaterial, 0);
		}

		private static int debugZoneIndex = -1;
		private static float lightFuzzyness = 1.06f;
		private static float lightRange = 1f;
		private static float strength = 1f;

		public static void OnImguiDebug()
		{
			if (ImGui.InputInt("Zone Type Index", ref debugZoneIndex) && debugZoneIndex != -1)
			{
				Instance.material.SetInt("_DepthsIndex", debugZoneIndex);
				Instance.zoneTypeMaterial.SetInt("_DepthsIndex", debugZoneIndex);
			}

			//0.3f
			if (ImGui.DragFloat("LightFuzziness###DetphsVeilFuzzy", ref lightFuzzyness))
				Instance.material.SetFloat("_LightFuzziness", lightFuzzyness);

			if (ImGui.DragFloat("LightRange###DetphsVeilLightRange", ref lightRange))
				Instance.material.SetFloat("_LightRange", lightRange);

			if (ImGui.DragFloat("Strenth###DetphsVeil", ref strength))
				Instance.material.SetFloat("_Strength", strength);

		}

		public void SetZoneType(ZoneType zoneType)
		{
			var indices = World.Instance.zoneRenderData.zoneTextureArrayIndices;
			var zone = (int)zoneType;

			if (indices.Length <= zone)
			{
				Log.Debug($"no such zone index {zoneType} ({zone}");
				return;
			}

			Log.Debug($"setting index to : zone: {(int)zoneType}  index: {indices[zone]}");
			material.SetInt("_DepthsIndex", indices[zone]);
			zoneTypeMaterial.SetInt("_DepthsIndex", indices[zone]);
		}
	}
}
