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
                hat = null;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if(resume.identity.nameStringKey == "VAHANO")
            {
                kbac.animScale *= 0.9f;
            }
        }
    }
}
