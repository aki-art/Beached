using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Updatable : KMonoBehaviour
	{
		[SerializeField] public CellOffset offset;

		public int cell;

		public Updatable()
		{
			offset = CellOffset.none;
		}

		public override void OnSpawn()
		{
			cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
			TileUpdater.Instance.Add(this);
			UpdateNeighbors();
		}

		public void UpdateNeighbors()
		{
			TileUpdater.Instance.Trigger(ModHashes.updateNeighbors, this);
		}

		public override void OnCleanUp()
		{
			TileUpdater.Instance.Remove(cell, this);
			base.OnCleanUp();
		}
	}
}
