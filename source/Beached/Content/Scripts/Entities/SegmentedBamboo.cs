using System.Collections.Generic;

namespace Beached.Content.Scripts.Entities
{
	public class SegmentedBamboo : SegmentedPlant
	{
		private static readonly List<string> liveSegmentAnims = new()
		{
			"idle0_0",
			"idle0_1",
			"idle0_2",
			"idle0_3",
			"idle0_4",
			"idle0_5",
			"idle0_6",
			"idle0_7",
			"idle0_8",
			"idle0_9",
			"idle0_10",
			"idle0_11"
		};

		private const string liveTopPiece = "top";
		private const string shoot = "shoot";

		[MyCmpReq] protected Ladder ladder;

		protected override string GetSegmentAnim(SegmentInfo segment, int totalDistance)
		{
			if (totalDistance == 1)
			{
				return shoot;
			}

			if (segment.distanceFromRoot == totalDistance - 1)
			{
				return liveTopPiece;
			}

			return liveSegmentAnims[segment.variationIdx];
		}

		protected override int MaxVariationCount() => segments.Count;

		protected override SegmentInfo CreateSegment(int i)
		{
			var segment = base.CreateSegment(i);

			var go = segment.animController.gameObject;

			var ladder = segment.animController.gameObject.AddOrGet<Ladder>();
			ladder.isPole = true;
			ladder.downwardsMovementSpeedMultiplier = 1.5f;
			ladder.upwardsMovementSpeedMultiplier = 0.5f;

			return segment;
		}
	}
}
