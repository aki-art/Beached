using System;

namespace Beached.Utils.GlobalEvents
{
	// used for static events not belonging to a kmonobehavior, and not wanting to rely on components existing
	public class SubscribeAttribute(int globalEventHash) : Attribute
	{
		public readonly int globalEventHash = globalEventHash;
	}
}
