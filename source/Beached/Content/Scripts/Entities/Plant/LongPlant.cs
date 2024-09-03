using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class LongPlant : FMonoBehaviour, IImguiDebug
	{
		[SerializeField] public Tag connectTag;
		[SerializeField] public Direction growthDirection;
		[SerializeField] public int maxLength;
		[SerializeField] public Tag growPrefab;

		private LongPlantSegment[] segments;

		[Serialize] public int cachedLength;

		public override void OnSpawn()
		{
			base.OnSpawn();
			segments = new LongPlantSegment[maxLength];

			Subscribe(ModHashes.stackableSegmentDestroyed, OnSegmentDeath);
		}

		private void OnSegmentDeath(object obj)
		{
			if (obj is LongPlantSegment segment)
			{
				UpdateSegments(segment.distance - 1);
			}
			else
				UpdateSegments(0);
		}

		public void Attach(LongPlantSegment segment)
		{
			if (Grid.PosToXY(segment.transform.position).x != Grid.PosToXY(transform.position).x)
				return;

			var segmentCell = Grid.PosToCell(segment);

			var distance = Grid.GetCellDistance(Grid.PosToCell(this), segmentCell);
			if (distance > maxLength)
				return;

			if (distance > 0 && segments[distance] != null)
			{
				Util.KDestroyGameObject(segment.gameObject);
				return;
			}

			segment.OnAttach(this, distance);
			segments[distance] = segment;
		}

		public void SegmentDestroyed(LongPlantSegment segment)
		{
			if (segment != null)
			{
				cachedLength = Mathf.Min(segment.distance, cachedLength);
				segments[segment.distance] = null;
				UpdateSegments(segment.distance);
			}
		}

		public void Grow()
		{
			if (growPrefab == null)
				return;

			var cell = Beached_Grid.GetCellTowards(Grid.PosToCell(this), growthDirection, cachedLength);

			if (IsValidGrowthCell(cell))
			{
				var newSegmentGo = MiscUtil.Spawn(growPrefab, Grid.CellToPos(cell));
				if (newSegmentGo.TryGetComponent(out LongPlantSegment newSegment))
					Attach(newSegment);

				cachedLength++;
			}
		}

		private bool IsValidGrowthCell(int cell)
		{
			return Grid.IsValidCellInWorld(cell, this.GetMyWorldId())
				&& (Grid.IsGas(cell) || Grid.Mass[cell] == 0);
		}

		public void GrowToRandomLength(int min, int max)
		{
			var length = Random.Range(min, max + 1);
			for (int i = 0; i < length; i++)
				Grow();
		}

		public void UpdateSegments(int fromIndex)
		{
			fromIndex = Mathf.Max(fromIndex, 0);
			var newLength = maxLength;
			var broken = false;
			for (int i = fromIndex; i <= cachedLength; i++)
			{
				var segment = segments[i];
				if (segment == null)
				{
					newLength = Mathf.Min(i, cachedLength);
					broken = true;
					continue;
				}

				if (broken)
					segment.Snip(false);
				else
					segment.Trigger(ModHashes.stackableChanged, this);
			}

			cachedLength = newLength;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			for (int i = 1; i <= cachedLength; i++)
			{
				var segment = segments[i];
				Util.KDestroyGameObject(segment.gameObject);
			}
		}

		public void OnImguiDraw()
		{
			if (ImGui.Button("Grow"))
				Grow();

			if (ImGui.Button("Random length"))
				GrowToRandomLength(1, 32);
		}
	}
}
