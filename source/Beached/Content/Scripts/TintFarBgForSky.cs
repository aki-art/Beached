using UnityEngine;

namespace Beached.Content.Scripts
{
	public class TintFarBgForSky : KMonoBehaviour, ISim200ms
	{
		private SpriteRenderer sprite;

		public override void OnSpawn()
		{
			var go = transform.Find("far bg/Square");

			if (go != null)
				sprite = go.GetComponent<SpriteRenderer>();
			else
				FUtility.FUI.Helper.ListChildren(transform);
		}

		public void Sim200ms(float dt)
		{
			if (sprite != null)
				sprite.color = Shader.GetGlobalColor("_Beached_TimeOfDayColor");
		}
	}
}
