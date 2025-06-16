using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using System.Collections.Generic;

namespace Beached.Content.Scripts.ClassExtensions
{
	public class CavityInfoExtension(CavityInfo original)
	{
		private CavityInfo original = original;
		public List<KPrefabID> pois = [];
		public List<CollarDispenser> collarDispensers = [];
		public List<Mirror> mirrors = [];
	}
}
