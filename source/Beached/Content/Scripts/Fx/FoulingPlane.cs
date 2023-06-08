using UnityEngine;
using UnityEngine.Rendering;

namespace Beached.Content.Scripts.Fx
{
	public class FoulingPlane : KMonoBehaviour
    {
		public GameObject plane;
		public Material material;

		public void Init()
        {
            CreatePlane();
        }

        private void CreatePlane()
		{
			if (plane != null)
				return;

			plane = new GameObject("Beached_FoulingOverlayPlane");

			plane.transform.parent = WaterCubes.Instance.cubes.transform;

			var meshFilter = plane.AddComponent<MeshFilter>();

			material = new Material(Shader.Find("Klei/FallingWater"))
			{
				mainTexture = Texture2D.redTexture,
				renderQueue = RenderQueues.Liquid
			};

			var meshRenderer = plane.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

			meshFilter.sharedMesh = WaterCubes.Instance.CreateNewMesh();
			meshFilter.sharedMesh.name = "Beached_FoulingOverlayPlane";
			meshRenderer.gameObject.layer = 0; // LayerMask.NameToLayer("TransparentFX");
			meshRenderer.gameObject.transform.SetPosition(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)));

			plane.SetActive(true);

			Log.Debug("Created Beached_FoulingOverlayPlane");
		}
    }
}
