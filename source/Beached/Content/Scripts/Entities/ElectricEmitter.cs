using ImGuiNET;
using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ElectricEmitter : KMonoBehaviour, IImguiDebug
	{
		[SerializeField] public int maxCells;
		[SerializeField] public float powerLossMultiplier;
		[Serialize] public bool isZapping;
		[Serialize] public bool changesPath;
		[Serialize] public float minPathSecs, maxPathSecs;
		[Serialize] public List<int> targetCells = [];
		private readonly Dictionary<int, float> affectedCells = [];
		private float remainingPower;
		public static List<CellOffset> offsets = [.. MiscUtil.MakeCellOffsetsFromMap(false,
			"XXX",
			"XOX",
			"XXX")];

		private List<NeighborEntry> neighborCells;
		private int currentCell;
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
		}

		public void Pulse(float duration, float power, int strokeCount)
		{
			for (var i = 0; i < strokeCount; i++)
			{
				affectedCells.Clear();
				GenerateStroke(power);
				var lineRenderer = AddSimpleLineRenderer(transform, Color.white, Color.blue, 0.05f, 0);

				foreach (var cell in affectedCells)
				{
					Beached_Grid.Instance.electricity[cell.Key] = Mathf.Max(Beached_Grid.Instance.electricity[cell.Key], cell.Value);
				}
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

		private bool Step()
		{
			neighborCells.Clear();
			offsets.Shuffle();

			foreach (var offset in offsets)
			{
				var cell = Grid.OffsetCell(currentCell, offset);
				var conduction = Beached_Grid.GetElectricConduction(cell);

				if (conduction > 0 && !affectedCells.ContainsKey(cell))
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

			// TODO: this is a very dumb was to make electicity spread
			var power = bestCell.conduction * powerLossMultiplier;
			power = Mathf.Min(remainingPower, 1f);

			affectedCells[bestCell.cell] = power;

			currentCell = bestCell.cell;
			remainingPower -= power;

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
			ImGui.InputInt("Strokes", ref debugStrokeCount);

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
