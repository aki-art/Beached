using ImGuiNET;
using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class ElectricEmitter : KMonoBehaviour, IImguiDebug
	{
		[SerializeField] public int maxCells;
		[SerializeField] public float powerLossMultiplier;
		[Serialize] public bool isZapping;
		[Serialize] public bool changesPath;
		[Serialize] public float minPathSecs, maxPathSecs;
		[Serialize] public List<int> targetCells = new();
		private readonly List<int> affectedCells = new();
		private float remainingPower;
		public static CellOffset[] offsets = MiscUtil.MakeCellOffsetsFromMap(false,
			"XXX",
			"XOX",
			"XXX");

		private List<NeighborEntry> neighborCells;
		private int currentCell;
		private LineRenderer lineRenderer;
		private float debugStartPower = 10f;

		public void Stop() => changesPath = false;

		public void ChangePath()
		{

		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			neighborCells = new List<NeighborEntry>();
			lineRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.white, Color.blue, 0.1f);
		}

		public void Pulse(float duration, float power)
		{
			affectedCells.Clear();
			currentCell = Grid.PosToCell(this);
			remainingPower = power;

			while (remainingPower > 0 && Step())
			{
				if (affectedCells.Count > maxCells)
					break;
			}

			if (affectedCells.Count > 0)
				DrawDebugStroke();
		}

		private void DrawDebugStroke()
		{
			lineRenderer.positionCount = affectedCells.Count;
			lineRenderer.SetPositions(affectedCells.Select(c =>
			{
				var pos = Grid.CellToPos(c);
				pos += new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, 0);

				return pos;//Grid.CellToPosCCC(c, Grid.SceneLayer.FXFront2);
			}).ToArray());
		}

		private bool Step()
		{
			neighborCells.Clear();

			foreach (var offset in offsets)
			{
				var cell = Grid.OffsetCell(currentCell, offset);
				var conduction = BeachedGrid.GetElectricConduction(cell);

				if (conduction > 0 && !affectedCells.Contains(cell))
					neighborCells.Add(new NeighborEntry()
					{
						cell = cell,
						conduction = conduction,
					});
			}

			if (neighborCells.Count == 0)
				return false;

			neighborCells.Sort();

			var bestCell = neighborCells[0];
			affectedCells.Add(bestCell.cell);
			currentCell = bestCell.cell;
			remainingPower -= bestCell.conduction * powerLossMultiplier;

			return true;
		}

		private IEnumerator PathChangeCoroutine()
		{
			while (changesPath)
			{
				ChangePath();

				var wait = UnityEngine.Random.Range(minPathSecs, maxPathSecs);
				yield return new WaitForSeconds(wait);
			}

			yield return null;
		}

		public void OnImguiDraw()
		{
			ImGui.DragFloat("Power", ref debugStartPower);

			if (ImGui.Button("Pulse"))
				Pulse(-1, debugStartPower);

			foreach (var neighbor in neighborCells)
				ImGui.Text($"{neighbor.cell} : {neighbor.conduction}");
		}

		public struct NeighborEntry : IComparer<NeighborEntry>, IComparable<NeighborEntry>
		{
			public int cell;
			public float conduction;

			public readonly int Compare(NeighborEntry x, NeighborEntry y) => x.conduction.CompareTo(y.conduction);

			public int CompareTo(NeighborEntry other)
			{
				if(other.Equals(this)) 
					return UnityEngine.Random.value > 0.5f ? 1 : -1;

				return conduction.CompareTo(other.conduction);
			}
		}
	}
}
