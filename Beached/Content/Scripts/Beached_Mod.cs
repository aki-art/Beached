using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Beached_Mod : KMonoBehaviour
    {
        public static Beached_Mod Instance;

        // a bunch of singleton components i don't want to spam on SaveGame
        // these do not serialize, put serialialized data here
        public IridescenceEffect iridescenceEffect;
        public ElementInteractions elementInteractions;
        public Tutorials tutorials;
        public Treasury treasury;
        public WishingStarEvent wishingStarEvent;

        public override void OnPrefabInit()
        {
            Instance = this;

            var childGo = new GameObject("BeachedStuff");
            DontDestroyOnLoad(childGo);
            childGo.transform.SetParent(transform);
            childGo.SetActive(true);

            iridescenceEffect = childGo.AddOrGet<IridescenceEffect>();
            elementInteractions = childGo.AddOrGet<ElementInteractions>();
            tutorials = childGo.AddOrGet<Tutorials>();
            wishingStarEvent = childGo.AddOrGet<WishingStarEvent>();

            treasury = childGo.AddOrGet<Treasury>();
            treasury.Configure();

            childGo.AddOrGet<TileUpdater>();
        }

        public override void OnCleanUp()
        {
            Instance = null;

            if (iridescenceEffect != null)
            {
                Util.KDestroyGameObject(iridescenceEffect.gameObject);
            }
        }
    }
}
