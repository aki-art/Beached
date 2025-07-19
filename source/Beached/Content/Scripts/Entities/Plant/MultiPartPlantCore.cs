using KSerialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{

	public abstract class MultiPartPlantCore : KMonoBehaviour
	{
		[Serialize] public List<Ref<MultiPartPlantPiece>> plantPieces;
		[SerializeField] public bool destroyPartsOnCleanup;

		protected Dictionary<int, MultiPartPlantPiece> runtimePlantPieces;

		[OnDeserialized]
		public void OnDeserialized()
		{
			if (plantPieces == null)
				return;

			runtimePlantPieces = plantPieces
				.Select(piece => piece.Get())
				.Where(piece => piece != null)
				.ToDictionary(Grid.PosToCell, piece => piece);

			plantPieces = null;
		}

		public override void OnPrefabInit()
		{
			runtimePlantPieces ??= [];
		}

		public int Size() => runtimePlantPieces == null ? 0 : runtimePlantPieces.Count;

		public bool AttachPart(MultiPartPlantPiece piece, int towardsRoot)
		{
			if (!CanAttach(piece))
				return false;

			var id = Grid.PosToCell(piece);

			runtimePlantPieces ??= [];
			if (runtimePlantPieces.TryGetValue(id, out var existingPiece))
			{
				return existingPiece.GetInstanceID() == piece.GetInstanceID();
			}

			runtimePlantPieces[id] = piece;
			piece.towardsRoot = towardsRoot;

			Trigger(ModHashes.multiPartPlant_Joined, piece);

			return true;
		}

		public bool RemovePart(MultiPartPlantPiece piece)
		{
			var id = Grid.PosToCell(piece);
			if (runtimePlantPieces.TryGetValue(id, out var existingPiece))
			{
				if (existingPiece.GetInstanceID() != piece.GetInstanceID())
					return false;
			}

			return RemovePart(id);
		}

		protected bool RemovePart(int id)
		{
			Trigger(ModHashes.multiPartPlant_Joined, runtimePlantPieces[id]);
			return runtimePlantPieces.Remove(id);
		}

		protected virtual bool CanAttach(MultiPartPlantPiece piece)
		{
			return true;
		}

		[OnSerializing]
		public void OnSerializing()
		{
			if (runtimePlantPieces == null)
				return;

			plantPieces = [.. runtimePlantPieces.Select(piece => new Ref<MultiPartPlantPiece>(piece.Value))];
		}

		public override void OnCleanUp()
		{
			if (destroyPartsOnCleanup)
				foreach (var piece in runtimePlantPieces.Values)
					Destroy(piece.gameObject);
		}

		public virtual void TriggerAllConnected(int eventHash, object data)
		{
			if (runtimePlantPieces == null)
				return;

			foreach (var piece in runtimePlantPieces)
				piece.Value.Trigger(eventHash, data);
		}

		public virtual bool CanGrowIntoCell(int cell)
		{
			if (!Grid.IsValidCell(cell))
				return false;

			if (Grid.IsSolidCell(cell) || Grid.HasDoor[cell])
				return false;

			if (runtimePlantPieces != null && runtimePlantPieces.ContainsKey(cell))
				return false;

			if (Grid.Objects[cell, (int)ObjectLayer.Building] != null)
				return false;

			return Grid.Objects[cell, (int)ObjectLayer.Plants] == null;
		}

		public abstract bool TryGrow();
	}
}
