using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beached.Content.Defs.StarmapEntities
{
    internal class InvisibleClusterGridEntity : ClusterGridEntity
    {
        public string _name;
        public override string Name => _name;

        public override EntityLayer Layer => EntityLayer.POI;

        public override List<AnimConfig> AnimConfigs => new List<ClusterGridEntity.AnimConfig>()
        {
            new ClusterGridEntity.AnimConfig()
            {
                animFile = Assets.GetAnim((HashedString) "temporal_tear_kanim"),
                initialAnim = "open_loop"
            }
        };

        public override bool IsVisible => true; //testing

        public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Hidden;
    }
}
