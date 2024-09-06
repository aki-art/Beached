using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class Plushie(string id, string name, string animFile, string effect, Vector3 offset) : Resource(id, null, name)
	{
		public readonly string animFile = animFile;
		public readonly string effect = effect;
		public readonly Vector3 offset = offset;
		private OnSleptWithDelegate onSleepingWithPlush;
		private CanPlacePlushieDelegate canPlacePlushie;

		public delegate bool CanPlacePlushieDelegate(GameObject bed);
		public delegate void OnSleptWithDelegate(GameObject minion);

		public Plushie OnSleptWith(OnSleptWithDelegate fn)
		{
			onSleepingWithPlush = fn;
			return this;
		}

		public Plushie PlaceCheck(CanPlacePlushieDelegate fn)
		{
			canPlacePlushie = fn;
			return this;
		}

		public bool CanPlaceOnBed(GameObject bed) => canPlacePlushie == null || canPlacePlushie(bed);

		public void OnSleptWith(GameObject minion)
		{
			if (!effect.IsNullOrWhiteSpace())
			{
				if (minion.TryGetComponent(out Effects effects))
					effects.Add(effect, true);
			}

			if (onSleepingWithPlush == null)
				return;

			onSleepingWithPlush(minion);
		}
	}
}
