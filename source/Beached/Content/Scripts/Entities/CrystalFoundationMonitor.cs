using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{

	public class CrystalFoundationMonitor : KMonoBehaviour
	{
		private int position;

		[Serialize]
		public bool needsFoundation = true;

		[Serialize]
		public bool hasFoundation = true;

		[Serialize]
		public bool hasMatchingFoundation = true;

		[SerializeField]
		public SimHashes myElement;

		[MyCmpGet]
		private KPrefabID kPrefabID;

		public CellOffset monitorCell = new CellOffset(0, -1);

		private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

		public override void OnSpawn()
		{
			position = Grid.PosToCell(gameObject);

			var cell = Grid.OffsetCell(position, monitorCell);
			if (Grid.IsValidCell(position) && Grid.IsValidCell(cell))
			{
				partitionerEntries.Add(GameScenePartitioner.Instance.Add("CrystalFoundationMonitor.OnSpawn", gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, OnGroundChanged));
			}

			OnGroundChanged(null);
		}

		public override void OnCleanUp()
		{
			for (var i = 0; i < partitionerEntries.Count; i++)
			{
				var handle = partitionerEntries[i];
				GameScenePartitioner.Instance.Free(ref handle);
			}

			base.OnCleanUp();
		}

		public void SetFoundationAndStartMonitoring(CellOffset offset)
		{
			monitorCell = offset;
			needsFoundation = true;
			OnGroundChanged(null);
		}

		public Element GetFoundation(int cell)
		{
			if (!Grid.IsCellOffsetValid(cell, monitorCell))
			{
				return null;
			}

			var foundationCell = Grid.OffsetCell(cell, monitorCell);

			return Grid.Element[foundationCell];
		}

		public void OnGroundChanged(object _)
		{
			if (!needsFoundation)
			{
				return;
			}

			var foundationElement = GetFoundation(position);
			var isValidFoundation = foundationElement != null && foundationElement.IsSolid;

			if (!hasFoundation && isValidFoundation)
			{
				hasFoundation = true;
				kPrefabID.RemoveTag(GameTags.Creatures.HasNoFoundation);
			}
			else if (hasFoundation && !isValidFoundation)
			{
				hasFoundation = false;
				kPrefabID.AddTag(GameTags.Creatures.HasNoFoundation);
			}

			hasFoundation = isValidFoundation;
			hasMatchingFoundation = foundationElement != null && foundationElement.id == myElement;

			Trigger((int)GameHashes.FoundationChanged, foundationElement);
		}
	}
}
