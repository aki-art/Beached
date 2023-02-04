namespace Beached.Content.Scripts
{
    public class Vista : KMonoBehaviour
    {
        [MyCmpGet]
        private KPrefabID kPrefabID;

        private CavityInfo currentCavity;

        public override void OnPrefabInit()
        {
            gameObject.AddTag(BTags.FastTrack.RegisterRoom);
            gameObject.AddTag(BTags.Vista);

            if(Mod.isFastTrackHere)
            {
                Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
            }
        }

        private void OnUpdateRoom(object obj)
        {
            if(obj is Room room)
            {
                UpdateRoom(room?.cavity);
            }
        }

        public override void OnCleanUp()
        {
            RemoveVista();
        }

        public void UpdateRoom(CavityInfo cavity)
        {
            if (Game.IsQuitting())
            {
                return;
            }

            if (cavity == currentCavity)
            {
                return;
            }

            RemoveVista();

            if (cavity != null)
            {
                cavity.AddNaturePOI(kPrefabID);
            }

            currentCavity = cavity;
        }

        private void RemoveVista()
        {
            if (currentCavity != null)
            {
                currentCavity.RemoveNaturePOI(kPrefabID);
            }
        }
    }
}
