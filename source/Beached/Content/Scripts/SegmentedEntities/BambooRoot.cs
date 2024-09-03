using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.SegmentedEntities
{
	public class BambooRoot : SegmentedEntityRoot
	{
		private static readonly List<HashedString> liveSegmentAnims =
		[
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
		];

		private static readonly HashedString top = "top";

		public override EntitySegment SpawnSegment(Vector3 position)
		{
			var segment = base.SpawnSegment(position);

			segment.GetComponent<KBatchedAnimController>().Play(top);

			var length = GetLength();

			if (GetLength() >= 2)
				GetSegment(length - 2).GetComponent<KBatchedAnimController>().Play(liveSegmentAnims.GetRandom());

			return segment;
		}

		public override int Grow(int times = 1)
		{
			var result = base.Grow(times);
			Beached_Mod.Instance.tallestBambooGrown = Mathf.Max(GetLength(), Beached_Mod.Instance.tallestBambooGrown);
			return result;
		}
	}
}
