using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class StackablePlantSegment : FMonoBehaviour, ISim4000ms
	{
		[SerializeField] public Direction growthDirection;
		[SerializeField] public Tag stackTag;
		[SerializeField] public float lethalHighTemperatureK;

		[Serialize] public Ref<StackablePlantBase> root;
		private bool markedForDeleteByRoot;
		private int foundationCell;

		public StackablePlantBase Root() => root?.Get();

		public override void OnSpawn()
		{
			base.OnSpawn();
			foundationCell = Grid.GetCellInDirection(Grid.PosToCell(this), MiscUtil.GetOpposite(growthDirection));
		}

		public override void OnCleanUp()
		{
			if (!markedForDeleteByRoot)
			{
				var root = Root();
				if (root != null)
					root.Trigger(ModHashes.stackableSegmentDestroyed, Grid.PosToCell(this));

			}

			base.OnCleanUp();
		}

		public void Sim4000ms(float dt)
		{
			if (!CanSurvive())
				Util.KDestroyGameObject(this);
		}

		public virtual bool CanSurvive()
		{
			if (Root() == null) return false;

			if (!(Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(foundationCell, out var go) && go.IsPrefabID(stackTag)))
				return false;

			if (Grid.IsSolidCell(Grid.PosToCell(this))) return false;

			if (Grid.Temperature[Grid.PosToCell(this)] > lethalHighTemperatureK)
				return false;

			return true;
		}

		public void DeleteWithoutNotify()
		{
			markedForDeleteByRoot = true;
			Util.KDestroyGameObject(gameObject);
		}

		internal void UpdateAllAbove()
		{
			throw new NotImplementedException();
		}
	}
}
