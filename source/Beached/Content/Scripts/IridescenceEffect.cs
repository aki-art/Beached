using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SkipSaveFileSerialization]
	public class IridescenceEffect : KMonoBehaviour
	{
#if ELEMENTS
		private Gradient rainbowGradient;

		private static Color pearl1 = Util.ColorFromHex("ca3b4c");
		private static Color pearl2 = Util.ColorFromHex("005ebe");

		public override void OnPrefabInit()
		{
			rainbowGradient = new Gradient();

			var colors = new List<Color> {
				new Color32(255, 71, 71, 150),
				new Color32(255, 183, 071, 150),
				new Color32(124, 230, 127, 150),
				new Color32(87, 255, 202, 150),
				new Color32(86, 158, 255, 150),
				new Color32(219, 148, 235, 150),
			};

			var colorKey = new GradientColorKey[colors.Count];

			for (var i = 0; i < colors.Count; i++)
			{
				colorKey[i] = new GradientColorKey(colors[i], (i + 1f) / colors.Count);
			}

			var alphaKey = new GradientAlphaKey[1];
			alphaKey[0].alpha = 1.0f;
			alphaKey[0].time = 0.0f;

			rainbowGradient.SetKeys(colorKey, alphaKey);
		}

		private void Update()
		{
			if (World.Instance == null)
			{
				return;
			}

			var materials = World.Instance.groundRenderer.elementMaterials;

			if (materials != null && materials.Count > 0)
			{
				var camera = Camera.main.transform.position;
				var scale = CameraController.Instance.zoomFactor;
				var t = (Mathf.Cos(camera.x / scale) + Mathf.Sin(camera.y / scale)) / 4f + 0.5f;
				var rainbowColor = rainbowGradient.Evaluate(t);

				UpdatePearl(materials, t);
				UpdateBismuth(materials, t);
				UpdateDiamond(materials, rainbowColor);
			}
		}

		private static void UpdatePearl(Dictionary<SimHashes, GroundRenderer.Materials> materials, float t)
		{
			var pearlEdgeColor = Color.Lerp(pearl1, pearl2, t);
			var pearlCenterColor = Color.Lerp(pearl2, pearl1, t);
			var pearlMat = materials[Elements.pearl];

			pearlMat.alpha.SetColor("_ShineColour", pearlEdgeColor);
			pearlMat.opaque.SetColor("_ShineColour", pearlCenterColor);
		}
		private static void UpdateBismuth(Dictionary<SimHashes, GroundRenderer.Materials> materials, float t)
		{
			var bismuthColor = Color.Lerp(Color.red, Color.green, t);
			var bismuthMat = materials[Elements.bismuthOre];
			bismuthMat.alpha.SetColor("_ShineColour", bismuthColor);
			bismuthMat.opaque.SetColor("_ShineColour", bismuthColor);
		}

		private static void UpdateDiamond(Dictionary<SimHashes, GroundRenderer.Materials> materials, Color rainbowColor)
		{
			var diamondMat = materials[SimHashes.Diamond];
			diamondMat.alpha.SetColor("_ShineColour", rainbowColor);
			diamondMat.opaque.SetColor("_ShineColour", rainbowColor);
		}
#endif
	}
}
