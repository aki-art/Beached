using Klei.AI;
using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedMinionStorage : KMonoBehaviour
    {
        [Serialize]
        public string hat;

        [MyCmpGet]
        MinionResume resume;

        [MyCmpGet]
        KBatchedAnimController kbac;


        public void RestoreHat()
        {
            if (!hat.IsNullOrWhiteSpace())
            {
                MinionResume.ApplyHat(hat, kbac);
                hat = "";
            } 
        }
    }
}
