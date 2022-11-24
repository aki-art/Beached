using KSerialization;

namespace Beached.Content.Scripts
{
    // Extended Identity/resuma
    public class BeachedMinionStorage : KMonoBehaviour
    {
        [Serialize]
        public string hat;

        [MyCmpReq]
        MinionResume resume;

        [MyCmpReq]
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
