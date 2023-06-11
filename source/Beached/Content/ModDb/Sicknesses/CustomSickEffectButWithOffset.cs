using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb.Sicknesses
{
	public class CustomSickEffectButWithOffset : Sickness.SicknessComponent
	{
		private readonly string kanim;
		private readonly string animName;
		private readonly Vector3 offset;

		public CustomSickEffectButWithOffset(string kanim, string animName, Vector3 offset)
		{
			this.kanim = kanim;
			this.animName = animName;
			this.offset = offset;
		}

		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			var effect = FXHelpers.CreateEffect(
				 kanim,
				 go.transform.GetPosition() + offset,
				 go.transform,
				 true);

			effect.Play(animName, KAnim.PlayMode.Loop);

			return effect;
		}

		public override void OnCure(GameObject go, object data) => ((Component)data).gameObject.DeleteObject();
	}
}
