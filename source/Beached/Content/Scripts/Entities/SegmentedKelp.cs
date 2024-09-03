using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class SegmentedKelp : SegmentedPlant
	{
		[MyCmpReq] private GermCultivator germCultivator;
		[SerializeField] public float minimumWaterMass;
		[SerializeField] public Tag dropTag;

		private static readonly List<string> liveSegmentAnims =
		[
			"idle0",
			"idle1",
			"idle2",
			"idle3",
		];

		private static readonly string liveTopPiece = "top";

		public override int GetRandomLength()
		{
			var baseCell = Grid.PosToCell(this);
			var y = 0;

			while (y++ < maxLength)
			{
				var cell = Grid.OffsetCell(baseCell, 0, y);
				if (!KelpSubmersionMonitor.IsCellSubmerged(cell))
				{
					break;
				}
			}

			return Random.Range(1, y);
		}

		public override void SetLength(int length)
		{
			base.SetLength(length);
			germCultivator.SetCells(GetCells());
		}

		public override void OnCleanUp()
		{
			if (dropTag.IsValid)
			{
				if (DebugHandler.InstantBuildMode)
					return;

				FUtility.Utils.Spawn(dropTag, gameObject);
			}
		}

		protected override string GetSegmentAnim(SegmentInfo segment, int totalLength)
		{
			if (segment.distanceFromRoot == totalLength - 1)
			{
				return liveTopPiece;
			}

			return liveSegmentAnims[segment.variationIdx];
		}

		protected override int MaxVariationCount() => liveSegmentAnims.Count;
	}
}
