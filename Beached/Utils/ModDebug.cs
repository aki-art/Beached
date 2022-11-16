using UnityEngine;

namespace Beached.Utils
{
    public class ModDebug
    {
        public static bool ShowCrystalOrigins = true;

        public static LineRenderer AddSimpleLineRenderer(Transform transform, Color start, Color end)
        {
            var gameObject = new GameObject("Beached_DebugLineRenderer");
            gameObject.transform.parent = transform;
            gameObject.SetActive(true);

            var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

            debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            debugLineRenderer.startColor = start;
            debugLineRenderer.endColor = end;
            debugLineRenderer.startWidth = debugLineRenderer.endWidth = 0.1f;
            debugLineRenderer.positionCount = 2;

            debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;

            return debugLineRenderer;
        }
    }
}
