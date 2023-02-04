using System.Collections.Generic;

namespace Beached.Content.Scripts.ClassExtensions
{
    public class CavityInfoExtension
    {
        private CavityInfo original;
        public List<KPrefabID> pois;

        public CavityInfoExtension(CavityInfo original)
        {
            this.original = original;
            pois = new List<KPrefabID>();
        }
    }
}
