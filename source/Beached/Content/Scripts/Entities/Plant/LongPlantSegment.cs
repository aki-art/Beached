using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	public class LongPlantSegment : FMonoBehaviour
	{
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField] public string animFileRoot;
		[SerializeField] public Direction checkDirectionOnSpawn;
		[SerializeField] public Tag connectTag;
		[SerializeField] public bool isRoot;

		[Serialize] public int distance;
		[Serialize] private LongPlant root;

		private bool alertRoot = true;

		private KAnimFile[] tipAnim;
		private KAnimFile[] middleAnim;
		private KAnimFile[] frustumAnim;
		private KAnimFile[] baseAnim;
		private KAnimFile[] singleAnim;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModHashes.stackableChanged, OnChanged);

			tipAnim = new[] { Assets.GetAnim($"{animFileRoot}_tip_kanim") };
			middleAnim = new[] { Assets.GetAnim($"{animFileRoot}_middle_kanim") };
			frustumAnim = new[] { Assets.GetAnim($"{animFileRoot}_frustum_kanim") };
			baseAnim = new[] { Assets.GetAnim($"{animFileRoot}_base_kanim") };
			singleAnim = new[] { Assets.GetAnim($"{animFileRoot}_single_kanim") };

			TryToAttachToExisting();
		}

		private void TryToAttachToExisting()
		{
			var cellToCheck = Grid.GetCellInDirection(Grid.PosToCell(this), checkDirectionOnSpawn);
			if (Beached_Grid.TryGetPlant(cellToCheck, out KMonoBehaviour component) && component.HasTag(connectTag))
			{
				if (component.TryGetComponent(out LongPlantSegment otherSegment) && otherSegment.root != null)
					otherSegment.root.Attach(this);
			}
		}

		private void OnChanged(object obj)
		{
			UpdateAnimation();
		}

		public void SetDistance(int distance)
		{
			this.distance = distance;
			UpdateAnimation();
		}

		private void UpdateAnimation()
		{
			KAnimFile[] animName;

			if (distance == root.cachedLength)
			{
				animName = root.cachedLength == 1 ? singleAnim : tipAnim;
			}
			else if (root.cachedLength == 2)
			{
				animName = distance == 1 ? frustumAnim : tipAnim;
			}
			else if (distance == 1)
			{
				animName = baseAnim;
			}
			else if (distance == root.cachedLength - 1)
			{
				animName = frustumAnim;
			}
			else
			{
				animName = middleAnim;
			}

			kbac.SwapAnims(animName);
			kbac.Play("idle");
		}

		public void OnAttach(LongPlant longPlant, int distance)
		{
			root = longPlant;
			SetDistance(distance);
		}

		public void Snip(bool alertRoot)
		{
			this.alertRoot = alertRoot;
			Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			if (!isRoot && alertRoot && root != null)
				root.Trigger(ModHashes.stackableSegmentDestroyed, this);

			base.OnCleanUp();
		}
	}
}
