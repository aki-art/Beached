using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedMod : KMonoBehaviour
    {
        public static BeachedMod Instance;

        public IridescenceEffect iridescenceEffect;
        public ElementInteractions elementInteractions;
        public Tutorials tutorials;
        public Treasury treasury;

        public override void OnPrefabInit()
        {
            Instance = this;

            // a bunch of singleton components i don't want to spam on SaveGame
            // these do not serialize, put serialialized data here
            var childGo = new GameObject("BeachedStuff");
            DontDestroyOnLoad(childGo);
            childGo.transform.SetParent(transform);
            childGo.SetActive(true);

            iridescenceEffect = childGo.AddOrGet<IridescenceEffect>();
            elementInteractions = childGo.AddOrGet<ElementInteractions>();
            tutorials = childGo.AddOrGet<Tutorials>();
            treasury = childGo.AddOrGet<Treasury>();
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
