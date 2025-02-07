using Beached.Content.Scripts.Buildings;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class Beached_CrystalClusterSideScreen : ReceptacleSideScreen
	{

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<CrystalGrower>() != null;

		public override void SetResultDescriptions(GameObject go)
		{
			string text = "";

			if (go.TryGetComponent(out InfoDescription description))
				text += description.description;

			descriptionLabel.SetText(text);
		}

		public override bool RequiresAvailableAmountToDeposit() => false;

		public override Sprite GetEntityIcon(Tag prefabTag)
		{
			return Def.GetUISprite(Assets.GetPrefab(prefabTag)).first;
		}

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
		}
	}
}
