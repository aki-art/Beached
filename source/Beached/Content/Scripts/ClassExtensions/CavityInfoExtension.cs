using Beached.Content.Scripts.Entities;
using System.Collections.Generic;

namespace Beached.Content.Scripts.ClassExtensions
{
	public class CavityInfoExtension(CavityInfo original)
	{
		private CavityInfo original = original;
		public List<KPrefabID> pois = new();
		public List<CollarDispenser> collarDispensers = new();
	}
}
