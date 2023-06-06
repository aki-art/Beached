using System;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class Plushie : Resource
	{
		public readonly string animFile;
		public readonly string effect;
		public readonly Vector3 offset;
		public readonly Func<GameObject> onSleepingWithPlush;

		public Plushie(string id, string name, string animFile, string effect, Vector3 offset, Func<GameObject> onSleepingWithPlush = null) : base(id, null, name)
		{
			this.animFile = animFile;
			this.effect = effect;
			this.offset = offset;
			this.onSleepingWithPlush = onSleepingWithPlush;
		}
	}
}
