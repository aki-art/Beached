using KSerialization;

namespace Beached.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedMinionStorage : KMonoBehaviour
    {
        [Serialize]
        public string hat;

        [MyCmpGet]
        private MinionResume resume;

        [MyCmpGet]
        private KBatchedAnimController kbac;


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
