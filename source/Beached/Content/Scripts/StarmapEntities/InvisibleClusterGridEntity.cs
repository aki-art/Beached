using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beached.Content.Scripts.StarmapEntities
{
    /// <summary>
    /// used by the meteor swarm poi entities
    /// </summary>
    internal class InvisibleClusterGridEntity : ClusterGridEntity
    {
        public string _name;
        public override string Name => _name;

        public override EntityLayer Layer => EntityLayer.POI; //potentially put on FX layer

        public override List<AnimConfig> AnimConfigs => new List<AnimConfig>()
        {
            new AnimConfig() //filler
            {
                animFile = Assets.GetAnim((HashedString) "temporal_tear_kanim"),
                initialAnim = "open_loop"
            }
        };

        public override bool IsVisible => true; //testing, set to false for finished product

        public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Hidden;
    }
}
