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
		[Serialize] public List<int> targetCells = [];
		private readonly List<int> affectedCells = [];
		private float remainingPower;
		public static List<CellOffset> offsets = MiscUtil.MakeCellOffsetsFromMap(false,
			"XXX",
			"XOX",
			"XXX").ToList();

		private List<NeighborEntry> neighborCells;
		private int currentCell;
		private List<LineRenderer> lineRenderers;
		private float debugStartPower = 10f;
		private int debugStrokeCount;

		public void Stop() => changesPath = false;

		public void ChangePath()
		{

		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			neighborCells = [];
			lineRenderers = [];
		}

		public void Pulse(float duration, float power, int strokeCount)
		{
			foreach(var renderer in lineRenderers)
				Util.KDestroyGameObject(renderer.gameObject);

			lineRenderers.Clear();

			for(var i = 0; i < strokeCount; i++)
			{
				affectedCells.Clear();
				GenerateStroke(power);
				var lineRenderer = AddSimpleLineRenderer(transform, Color.white, Color.blue, 0.05f, 0);

				if (affectedCells.Count > 0)
					DrawDebugStroke(lineRenderer);

				lineRenderers.Add(lineRenderer);
			}
		}

		public static LineRenderer AddSimpleLineRenderer(Transform transform, Color start, Color end, float startWidth, float endWidth)
		{
			var gameObject = new GameObject("Beached_DebugLineRenderer");
			gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX));
			transform.SetParent(transform);

			gameObject.SetActive(true);

			var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

			debugLineRenderer.material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				renderQueue = 3501
			};
			debugLineRenderer.startColor = start;
			debugLineRenderer.endColor = end;
			debugLineRenderer.startWidth = startWidth;
			debugLineRenderer.endWidth = endWidth;
			debugLineRenderer.positionCount = 2;

			debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;

			return debugLineRenderer;
		}

		private void GenerateStroke(float power)
		{
			currentCell = Grid.PosToCell(this);
			remainingPower = power;

			while (remainingPower > 0 && Step())
			{
				if (affectedCells.Count > maxCells)
					break;
			}
		}

		private void DrawDebugStroke(LineRenderer lineRenderer)
		{
			lineRenderer.positionCount = affectedCells.Count;
			lineRenderer.SetPositions(affectedCells.Select(c =>
			{
				var pos = Grid.CellToPos(c);
				pos += new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX));

				return pos;//Grid.CellToPosCCC(c, Grid.SceneLayer.FXFront2);
			}).ToArray());
		}

		private bool Step()
		{
			neighborCells.Clear();
			offsets.Shuffle();

			foreach (var offset in offsets)
			{
				var cell = Grid.OffsetCell(currentCell, offset);
				var conduction = Beached_Grid.GetElectricConduction(cell);

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
			ImGui.InputInt("Stokes", ref debugStrokeCount);

			if (ImGui.Button("Pulse"))
				Pulse(-1, debugStartPower, debugStrokeCount);

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
				if (other.Equals(this))
					return UnityEngine.Random.value > 0.5f ? 1 : -1;

				return conduction.CompareTo(other.conduction);
			}
		}
	}
}
