using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beached.Content.Defs.Buildings
{
    internal class DNAEggDeliverable : SingleEntityReceptacle
    {
        private KBatchedAnimTracker tracker;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            autoReplaceEntity = true;
            statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
            synchronizeAnims = false;
            GetComponent<KBatchedAnimController>().SetSymbolVisiblity("egg_target", false);
        }
    }
}
