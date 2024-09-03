using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class SetPiece : KMonoBehaviour, IImguiDebug
	{
		public GameObject visualizer;

		[SerializeField] public string setPiecePrefabID;
		[SerializeField] public string sprite;
		[SerializeField] public int width;
		[SerializeField] public int height;

		public override void OnSpawn()
		{
			base.OnSpawn();

			this.transform.SetPosition(
				new Vector3(
					transform.position.x,
					transform.position.y,
					Grid.GetLayerZ(Grid.SceneLayer.Backwall) + 0.1f));

			if (ModAssets.Prefabs.setpieces.TryGetValue(setPiecePrefabID, out var prefab))
			{
				visualizer = Instantiate(prefab);

				var collider = GetComponent<KBoxCollider2D>();
				collider.size *= 0.8f;

				if (sprite != null)
				{
					var bg = visualizer.transform.Find("Bg");
					if (bg == null)
						Log.Warning("Tryng to set setpiece bg but Bg is null");
					else
					{
						bg.GetComponent<SpriteRenderer>().sprite = Assets.GetSprite(sprite);
					}
				}

				var position = new Vector3(
					transform.position.x,
					transform.position.y + height / 2f,
					transform.position.z);

				visualizer.transform.position = position;
				visualizer.transform.SetParent(transform, true);

				visualizer.SetActive(true);
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			if (visualizer != null)
				Destroy(visualizer);
		}

		private static float scale;

		public void OnImguiDraw()
		{
			if (ImGui.DragFloat("setpiece scale", ref scale))
			{
				visualizer.transform.localScale.Set(scale, scale, 1);
			}

			this.transform.SetPosition(
				new Vector3(
					transform.position.x,
					transform.position.y,
					Grid.GetLayerZ(Grid.SceneLayer.Backwall) + 0.1f));

			if (ImGui.Button("Fix layer"))
			{
				var position = new Vector3(
					visualizer.transform.position.x,
					visualizer.transform.position.y,
					transform.position.z);

				visualizer.transform.position = position;
			}
		}
	}
}
