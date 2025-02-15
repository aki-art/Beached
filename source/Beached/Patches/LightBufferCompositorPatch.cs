/*using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	internal class LightBufferCompositorPatch
	{
		[HarmonyPatch(typeof(LightBufferCompositor), "OnRenderImage")]
		public class LightBufferCompositor_OnRenderImage_Patch
		{
			public static void Postfix(LightBufferCompositor __instance, RenderTexture src, RenderTexture dest)
			{
				darkVeilPostFx.SetTexture("_LightOverlay", LightBuffer.Instance.Texture);
				darkVeilPostFx.SetFloat("_Zoom", CameraController.Instance.zoomFactor * zoomMultiplier);
				Graphics.Blit(src, dest, darkVeilPostFx);
			}
		}
	}
}
*/