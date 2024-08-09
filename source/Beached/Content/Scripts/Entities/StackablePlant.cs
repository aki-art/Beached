using UnityEngine;

namespace Beached.Content.Scripts.Entities;

public class StackablePlant : KMonoBehaviour
{
	[MyCmpGet] private Harvestable harvestable;

	[SerializeField] public ObjectLayer objectLayer;
	[SerializeField] public Tag validFoundationTag;

	public override void OnSpawn()
	{
		Subscribe(ModHashes.blockUpdate, OnUpdate);
		CheckFoundation();
	}

	private void OnUpdate(object _)
	{
		CheckFoundation();
	}

	private void CheckFoundation()
	{
		var cell = Grid.CellBelow(Grid.PosToCell(this));
		if (!CanGrowOn(cell))
		{
			harvestable.Harvest();
			TileUpdater.Instance.Trigger(ModHashes.updateNeighbors, this);
		}
	}

	public bool CanGrowOn(int cell)
	{
		if (Grid.Solid[cell])
		{
			return true;
		}

		return Grid.ObjectLayers[(int)objectLayer].TryGetValue(cell, out var go) && go.HasTag(validFoundationTag);
	}
}
