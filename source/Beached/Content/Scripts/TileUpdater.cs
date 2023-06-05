using Beached.Content.Scripts.Entities;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	public class TileUpdater : KMonoBehaviour
	{
		public static TileUpdater Instance { get; set; }

		private readonly Dictionary<int, HashSet<Updatable>> updateables = new();

		public override void OnPrefabInit() => Instance = this;

		public override void OnCleanUp() => Instance = null;

		public override void OnSpawn()
		{
			Subscribe(ModHashes.updateNeighbors, UpdateNeighbors);
		}

		private void UpdateNeighbors(object data)
		{
			if (data is Updatable updatable)
			{
				var cell = updatable.cell;
				UpdateCell(Grid.CellBelow(cell));
				UpdateCell(Grid.CellLeft(cell));
				UpdateCell(Grid.CellAbove(cell));
				UpdateCell(Grid.CellRight(cell));
			}
		}

		private void UpdateCell(int cell)
		{
			if (updateables.TryGetValue(cell, out var set))
			{
				foreach (var updatable in set)
				{
					updatable.Trigger(ModHashes.blockUpdate);
				}
			}
		}

		public void Add(Updatable updatable)
		{
			if (!updateables.ContainsKey(updatable.cell))
			{
				updateables[updatable.cell] = new HashSet<Updatable>();
			}

			updateables[updatable.cell].Add(updatable);
		}

		public void Remove(int cell, Updatable updatable)
		{
			if (updateables.TryGetValue(cell, out var set))
			{
				set.Remove(updatable);
			}
			else
			{
				Log.Warning($"Trying to remove entity \"{updatable.PrefabID()}\" from the updateables, but it was never added.");
			}
		}
	}
}
