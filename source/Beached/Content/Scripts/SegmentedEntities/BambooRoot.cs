using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.SegmentedEntities
{
	public class BambooRoot : SegmentedEntityRoot, ISim4000ms
	{
		[SerializeField] public float growthTimer;
		[Serialize] public float nextGrowth;

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

		public BambooRoot()
		{
			nextGrowth = -1;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (nextGrowth == -1)
				nextGrowth = GameClock.Instance.GetTime() + growthTimer;

			Subscribe((int)GameHashes.Uprooted, OnUprooted);

			StartCoroutine(OnNextFrame());
		}

		private IEnumerator OnNextFrame()
		{
			yield return SequenceUtil.waitForEndOfFrame;

			if (segments.Count > 0)
				segments.Last().GetComponent<KBatchedAnimController>().Play(top);
		}

		private void OnUprooted(object obj)
		{
			Util.KDestroyGameObject(gameObject);
		}

		public override EntitySegment SpawnSegment(Vector3 position)
		{
			var segment = base.SpawnSegment(position);

			segment.GetComponent<KBatchedAnimController>().Play(top);

			var length = GetLength();

			if (GetLength() >= 2)
			{
				var anim = liveSegmentAnims.GetRandom();
				var middleSegment = GetSegment(length - 2);

				middleSegment.GetComponent<KBatchedAnimController>().Play(anim);
				middleSegment.GetComponent<EntitySegment>().animation = anim;
			}

			return segment;
		}

		public override int Grow(int times = 1)
		{
			var result = base.Grow(times);
			Beached_Mod.Instance.tallestBambooGrown = Mathf.Max(GetLength(), Beached_Mod.Instance.tallestBambooGrown);
			return result; ;
		}

		public void Sim4000ms(float dt)
		{
			float time = GameClock.Instance.GetTime();
			if (time > nextGrowth)
			{
				Grow();
				nextGrowth = time + growthTimer;
			}
		}
	}
}
