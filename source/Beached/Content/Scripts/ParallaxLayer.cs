using UnityEngine;

namespace Beached.Content.Scripts
{
	public class ParallaxLayer : KMonoBehaviour
	{
		[SerializeField] public float distance = 1f;

		private MaterialPropertyBlock properties;

		public void SetDistance(float distance)
		{
			var renderer = GetComponent<SpriteRenderer>();
			properties = new MaterialPropertyBlock();
			renderer.GetPropertyBlock(properties);
			properties.SetFloat("_Distance", distance);
			renderer.SetPropertyBlock(properties);
		}
	}
}
