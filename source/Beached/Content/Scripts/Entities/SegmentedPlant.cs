using ImGuiNET;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beached.Content.Scripts.Entities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class SegmentedPlant : KMonoBehaviour, IImguiDebug
	{
		[SerializeField] public int maxLength;
		[SerializeField] public HashedString segmentAnimFile;
		[SerializeField] public HashedString segmentKillFxHash;
		[SerializeField] public SpawnFXHashes segmentDeathFx;

		[MyCmpReq] KBoxCollider2D collider;
		[MyCmpReq] KBatchedAnimController animController;
		[MyCmpReq] OccupyArea occupyArea;

		[Serialize] public List<SegmentInfo> segments;
		[Serialize] public int length;

		public int topCell;

		private int debugPreviousLength; // to check if the length was set from debug window

		public SegmentedPlant()
		{
			length = 1;
			debugPreviousLength = 1;
			segments = new List<SegmentInfo>();
		}

		public void GrowOne()
		{
			if (length == maxLength) return;

			var targetCell = Grid.OffsetCell(Grid.PosToCell(this), 0, length + 1);
			if (IsValidTile(targetCell))
			{
				SetLength(length++);
			}
		}

		public virtual void SetLength(int length)
		{
			if (length <= 0)
			{
				Util.KDestroyGameObject(this);
				return;
			}

			this.length = length;
			topCell = Grid.OffsetCell(Grid.PosToCell(this), 0, length);
			Trim();
			UpdatePlant();
		}

		public override void OnCleanUp()
		{

			foreach (var segment in segments)
			{
				DestroySegment(segment);
			}

			segments.Clear();
			length = 0;

			base.OnCleanUp();
		}

		private void UpdatePlant()
		{
			UpdateSegmentAnims();
			UpdateArea();
			collider.size = new Vector2(1, length);
			collider.offset = new Vector2(0, length / 2f);
		}

		private void UpdateArea()
		{
			var cellOffsets = new CellOffset[length];
			for (int y = 0; y < length; y++)
			{
				cellOffsets[y] = new CellOffset(0, y);
			}

			occupyArea.SetCellOffsets(cellOffsets);
		}

		public virtual bool IsValidTile(int cell)
		{
			return Grid.IsValidCell(cell) && !Grid.Solid[cell];
		}

		public virtual int GetRandomLength() => Random.Range(1, maxLength);

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.NewGameSpawn, OnNewGameSpawn);
		}

		public override void OnSpawn()
		{
			segments ??= new List<SegmentInfo>();
			base.OnSpawn();

			UpdatePlant();
		}

		private void OnNewGameSpawn(object _) => SetLength(GetRandomLength());

		private void Trim()
		{
			if (length < 1)
			{
				Log.Warning("Somehow this segmented plant broke. length = " + length);
				return;
			}

			for (int i = segments.Count - 1; i >= length; i--)
			{
				DestroySegment(segments[i]);
				segments.RemoveAt(i);
			}
		}

		protected virtual void DestroySegment(SegmentInfo segment)
		{
			if (segmentDeathFx != SpawnFXHashes.None)
			{
				Game.Instance.SpawnFX(segmentDeathFx, Grid.PosToCell(segment.animController.gameObject), 0);
			}

			segment.animLink.Unregister();
			Destroy(segment.animController);
		}

		private void UpdateSegmentAnims()
		{
			for (var i = 0; i < length; i++)
			{
				if (i >= segments.Count)
				{
					segments.Add(CreateSegment(i));
				}

				UpdateSegment(segments[i]);
			}
		}

		protected virtual void UpdateSegment(SegmentInfo segment)
		{
			if (segment.animController == null)
			{
				segment.animController = CreateAnimController(segment);
				segment.animLink = new KAnimLink(animController, segment.animController);
			}

			segment.animController.Play(GetSegmentAnim(segment, length));
		}

		protected abstract string GetSegmentAnim(SegmentInfo segment, int totalDistance);

		protected abstract int MaxVariationCount();

		protected virtual SegmentInfo CreateSegment(int i)
		{
			var info = new SegmentInfo()
			{
				distanceFromRoot = i,
				variationIdx = Random.Range(0, MaxVariationCount()),
			};

			info.animController = CreateAnimController(info);
			info.animLink = new KAnimLink(animController, info.animController);

			return info;
		}

		private KBatchedAnimController CreateAnimController(SegmentInfo info)
		{
			var go = new GameObject(name + "Segment");
			go.SetActive(false);

			go.transform.parent = transform;
			go.transform.position = transform.position + Vector3.up * info.distanceFromRoot;

			var animController = go.AddOrGet<KBatchedAnimController>();
			animController.AnimFiles = new[]
			{
				Assets.GetAnim( segmentAnimFile)
			};

			animController.initialAnim = GetSegmentAnim(info, length);

			go.SetActive(true);

			return animController;
		}

		public void OnImguiDraw()
		{
			ImGui.DragInt("Length", ref length, 1, 1, maxLength);
			if (length != debugPreviousLength)
			{
				SetLength(length);
				UpdatePlant();

				debugPreviousLength = length;
			}

			if (ImGui.Button("Grow to random length"))
			{
				SetLength(GetRandomLength());
			}
		}

		public struct SegmentInfo
		{
			public int distanceFromRoot;
			public int variationIdx;
			[NonSerialized] public KBatchedAnimController animController;
			[NonSerialized] public KAnimLink animLink;
		}
	}
}
