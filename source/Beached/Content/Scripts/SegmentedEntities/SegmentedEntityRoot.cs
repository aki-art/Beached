using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.SegmentedEntities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SegmentedEntityRoot : KMonoBehaviour, IImguiDebug
	{
		[Tooltip("Distance at which the segments are placed. ")]
		[Serialize]
		[SerializeField]
		public float distancePerSegment;

		[Tooltip("What angle to place the segments in. Does not control the rotation of the segment itself, just the position.")]
		[Serialize]
		[SerializeField]
		public float angle;

		[Tooltip("Max number of segments, self included")]
		[Serialize]
		[SerializeField]
		public int maxLength;

		[SerializeField]
		public Tag segmentPrefab;

		private List<EntitySegment> segments = [];

		[Serialize]
		private readonly List<Ref<EntitySegment>> segmentsSerialized = [];

		private Vector3 growthVector;
		private int debugLength;

		public SegmentedEntityRoot()
		{
			distancePerSegment = 1.0f;
			angle = 0.0f;
			maxLength = 32;
		}

		protected EntitySegment GetSegment(int index) => segments[index];

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (segments.Count == 0)
				segments.Add(GetComponent<EntitySegment>());

			if (segmentsSerialized != null)
			{
				foreach (var segment in segmentsSerialized)
					ConnectSegment(segment?.Get(), true);
			}
		}

		public override void OnPrefabInit()
		{
			segments = [];
			UpdateGrowthVector();
			Subscribe((int)GameHashes.NewGameSpawn, OnNewGameSpawn);
		}

		protected virtual bool CanOccupyCell(int cell)
		{
			return Grid.IsValidBuildingCell(cell)
				&& Grid.WorldIdx[cell] == this.GetMyWorldId()
				&& (Grid.IsGas(cell) || Grid.Mass[cell] == 0);
		}

		private void OnNewGameSpawn(object _)
		{
			if (segments.Count == 0)
				segments.Add(GetComponent<EntitySegment>());
			GrowToRandomLength();
		}

		protected void UpdateGrowthVector()
		{
			var q = Quaternion.Euler(0, 0, angle);
			growthVector = q * (Vector3.up * distancePerSegment);
		}

		public virtual void SetGrowth(float angle, float distancePerSegment)
		{
			this.angle = angle;
			this.distancePerSegment = distancePerSegment;
			UpdateGrowthVector();
		}

		public virtual EntitySegment SpawnSegment(Vector3 position)
		{
			var segment = FUtility.Utils.Spawn(segmentPrefab, position).GetComponent<EntitySegment>();
			ConnectSegment(segment, false);

			return segment;
		}

		public void ConnectSegment(EntitySegment segment, bool isDeserializing)
		{
			segments.Add(segment);
			segment.SetRoot(this);

			if (!isDeserializing && segment != this)
				segmentsSerialized.Add(new Ref<EntitySegment>(segment));
		}

		public virtual int GetLength() => segments.Count;

		public virtual void SetLength(int newLength)
		{
			newLength = Mathf.Min(newLength, maxLength);
			var oldLength = GetLength();

			if (oldLength == newLength)
				return;

			if (oldLength > newLength)
				Snip(oldLength - newLength);
			else
				Grow(newLength - oldLength);

			debugLength = newLength;
		}

		public virtual void GrowToRandomLength()
		{
			var y = 0;
			var pos = transform.position;

			for (y = 0; y <= maxLength; y++)
			{
				pos = pos + growthVector;
				var cell = Grid.PosToCell(pos);
				if (!CanOccupyCell(cell))
					break;
			}

			SetLength(Random.Range(1, y + 1));
		}

		protected Vector3 GetNextSegmentPosition() => GetLastSegmentPosition();

		protected Vector3 GetSegmentPosition(int index) => index * growthVector;

		public Vector3 GetLastSegmentPosition() => segments.Last().transform.position + growthVector;

		public virtual int Grow(int times = 1)
		{
			for (int i = 0; i < times; i++)
			{
				var pos = GetNextSegmentPosition();

				if (!CanOccupyCell(Grid.PosToCell(pos)))
					return i;

				SpawnSegment(GetNextSegmentPosition());
			}

			return times;
		}

		public virtual void Snip(int times)
		{
			for (int i = 0; i < times; i++)
				SnipLast();
		}

		public virtual void SnipAt(int index) => Snip(GetLength() - index);

		public virtual void SnipLast()
		{
			if (GetLength() == 0)
				return;

			var delete = segments.Last();
			segments.RemoveAt(GetLength() - 1);

			if (!delete.IsNullOrDestroyed())
			{
				delete.notifyRootOnDestroy = false;
				Util.KDestroyGameObject(delete);
			}
		}

		public virtual void SnipAll() => Snip(GetLength());

		public override void OnCleanUp()
		{
			if (!Game.IsQuitting() && !App.IsExiting)
				SnipAll();

			base.OnCleanUp();
		}

		public virtual void OnSegmentCleanup(EntitySegment entitySegment)
		{
			var index = segments.IndexOf(entitySegment);

			if (index == -1)
			{
				Log.Warning($"{name} was notified of loosing a segment, but the segment was never attached!");
				return;
			}

			// self
			if (index == 0)
				return;

			Snip(index);
		}

		public void OnImguiDraw()
		{
			ImGui.LabelText("Length", GetLength().ToString());
			ImGui.LabelText("Growth Vector", growthVector.ToString());

			if (ImGui.DragInt("Length", ref debugLength))
				SetLength(debugLength);

			if (ImGui.Button("Set Random Length"))
				GrowToRandomLength();

			if (ImGui.Button("Grow"))
				Grow();
		}
	}
}
