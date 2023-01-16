using System;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class BeachedWorldManager : KMonoBehaviour
    {
        public static BeachedWorldManager Instance { get; private set; }
        private GameObjectPool acidBubblesPool;

        public void WorldLoaded(string clusterId)
        {
            Log.Debug("WORLD HAS LOADED " + clusterId);

            IsBeachedContentActive = clusterId == "expansion1::clusters/TinyStartCluster";

            Elements.OnWorldReload(IsBeachedContentActive);
            //ElementInteractions.Instance.enabled = IsBeachedContentActive || Mod.Settings.CrossWorld.Elements.ElementInteractions;

        }

        // TODO: mod settings content enable || beached world loaded
        public bool IsBeachedContentActive { get; private set; } = true;

        private bool wasOnBeachedWorld;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            //acidBubblesPool = new GameObjectPool(InstantiateAcidBubbles, 16);
        }

        private GameObject InstantiateAcidBubbles()
        {
            throw new NotImplementedException();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }
    }
}
