using UnityEngine;

[ExecuteInEditMode]
public class SpriteSwapTest : MonoBehaviour
{
	[SerializeField] Texture2D texture;
	[SerializeField] bool dirty = false;

	void Update()
	{
		if (dirty)
		{
			if (texture != null)
			{
				var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector3(0.5f, 1f));
				GetComponent<SpriteRenderer>().sprite = sprite;
			}
			dirty = false;
		}
	}
}
