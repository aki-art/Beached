using Beached.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class IridescenceEffect : KMonoBehaviour
    {
        public static Dictionary<SimHashes, MockStructs.Materials> groundRendererMaterials;
        public static IridescenceEffect Instance;

        private Gradient rainbowGradient;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;

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
            if (groundRendererMaterials != null && groundRendererMaterials.Count > 0)
            {
                var camera = Camera.main.transform.position;
                var scale = CameraController.Instance.zoomFactor;

                var t = (Mathf.Cos(camera.x / scale) + Mathf.Sin(camera.y / scale)) / 4f + 0.5f;

                var pearlColor = Color.Lerp(Color.yellow, Color.cyan, t);
                var pearlMat = groundRendererMaterials[Elements.Pearl];
                pearlMat.alpha.SetColor("_ShineColour", pearlColor);
                pearlMat.opaque.SetColor("_ShineColour", pearlColor);

                var bismuthColor = Color.Lerp(Color.red, Color.green, t);
                var bismuthMat = groundRendererMaterials[Elements.BismuthOre];
                bismuthMat.alpha.SetColor("_ShineColour", bismuthColor);
                bismuthMat.opaque.SetColor("_ShineColour", bismuthColor);

                var rainbowColor = rainbowGradient.Evaluate(t);

                var seleniteMat = groundRendererMaterials[Elements.Selenite];
                seleniteMat.alpha.SetColor("_ShineColour", rainbowColor);
                seleniteMat.opaque.SetColor("_ShineColour", rainbowColor);

                var diamondMat = groundRendererMaterials[SimHashes.Diamond];
                diamondMat.alpha.SetColor("_ShineColour", rainbowColor);
                diamondMat.opaque.SetColor("_ShineColour", rainbowColor);
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
