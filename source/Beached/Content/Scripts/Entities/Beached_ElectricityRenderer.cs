using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Beached_ElectricityRenderer : KMonoBehaviour
	{
		private GameObject overlay;
		public Material material;

		public static Beached_ElectricityRenderer Instance { get; set; }

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

		public override void OnSpawn()
		{
			base.OnSpawn();

			overlay = Instantiate(ModAssets.Fx.electricOverlay);
			material = overlay.GetComponent<MeshRenderer>().material;

			overlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
			overlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

			OnShadersReloaded();
			ShaderReloader.Register(OnShadersReloaded);

			Beached_ElectricityRenderer.Instance.material.SetTexture("_Electricity", Beached_Grid.electricityTexture);

			overlay.SetActive(true);
		}

		private void OnShadersReloaded()
		{
			Beached_ElectricityRenderer.Instance.material.SetTexture("_Electricity", Beached_Grid.electricityTexture);
		}
	}
}
