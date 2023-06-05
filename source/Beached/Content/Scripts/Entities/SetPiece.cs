using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class SetPiece : KMonoBehaviour
	{
		public GameObject visualizer;

		[SerializeField]
		public string setPiecePrefabID;

		[SerializeField]
		public Texture2D placeholderTexture;

		public override void OnSpawn()
		{
			base.OnSpawn();

			/*            if (ModAssets.Prefabs.setpieces.TryGetValue(setPiecePrefabID, out var prefab))
						{
							visualizer = Instantiate(prefab);

							if (placeholderTexture != null)
							{
								SetPlaceholder();
							}

							var collider = GetComponent<KBoxCollider2D>();

							var position = new Vector3(
								transform.position.x - collider.offset.x,
								transform.position.y - collider.offset.y,
								Grid.GetLayerZ(Grid.SceneLayer.Backwall) - 0.1f);

							visualizer.transform.position = position;
							visualizer.transform.SetParent(transform);
							visualizer.SetActive(true);
						}*/
		}

		private void SetPlaceholder()
		{
			Log.Warning($"{this.PrefabID()} is still using placeholder texture.");
			var spriteRenderer = visualizer.transform.Find("bg 1").GetComponent<SpriteRenderer>();
			var originalWidth = spriteRenderer.sprite.rect.width;
			var originalHeight = spriteRenderer.sprite.rect.height;

			spriteRenderer.sprite = Sprite.Create(placeholderTexture, new Rect(0, 0, placeholderTexture.width, placeholderTexture.height), Vector2.zero);

			var scale = placeholderTexture.width / originalWidth;
			spriteRenderer.transform.localScale = new Vector3(scale, scale, 1) * 2f;

			foreach (Transform child in visualizer.transform)
			{
				if (child.name != "bg 1")
				{
					child.gameObject.SetActive(false);
				}
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			if (visualizer != null)
			{
				Destroy(visualizer);
			}
		}
	}
}
