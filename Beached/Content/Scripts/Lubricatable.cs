using System.Collections.Generic;
using Beached.Content.ModDb;
using ImGuiNET;
using KSerialization;
using rail;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class Lubricatable : KMonoBehaviour, IImguiDebug
    {
        [MyCmpGet] public KPrefabID kPrefabID;
        [MyCmpGet] public KSelectable kSelectable;
        [MyCmpGet] public Door door;
        [SerializeField] public Storage mucusStorage;
        [SerializeField] public ManualDeliveryKG delivery;
        [SerializeField] public float massUsedEachTime;
        [Serialize] public bool mucusRequested;
        public float originalPoweredAnimSpeed;
        public float originalUnPoweredAnimSpeed;

        public override void OnSpawn()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);

            if (door != null)
            {
                originalPoweredAnimSpeed = door.poweredAnimSpeed;
                originalUnPoweredAnimSpeed = door.unpoweredAnimSpeed;
            }

            kSelectable.AddStatusItem(BStatusItems.lubricated, this);
            OnStorageChange(null);
            UpdateDelivery();
        }
        
        private void OnStorageChange(object o)
        {
            var lubricated = mucusStorage.GetMassAvailable(Elements.mucus) >= massUsedEachTime;

            if (lubricated)
            {
                kPrefabID.AddTag(BTags.lubricated);
                if (door != null)
                {
                    door.poweredAnimSpeed = originalPoweredAnimSpeed * 3f;
                    door.unpoweredAnimSpeed = originalUnPoweredAnimSpeed * 3f;
                }
            }
            else
            {
                kPrefabID.RemoveTag(BTags.lubricated);
                if (door != null)
                {
                    door.poweredAnimSpeed = originalPoweredAnimSpeed;
                    door.unpoweredAnimSpeed = originalUnPoweredAnimSpeed;
                }
            }
            
            kSelectable.ToggleStatusItem(BStatusItems.lubricated, lubricated, this);
            UpdateDelivery();
        }

        public void OnUse()
        {
            mucusStorage.ConsumeIgnoringDisease(Elements.mucus.Tag, massUsedEachTime);
        }

        public static void ConfigurePrefab(GameObject prefab, float capacityKg, float massUsedEachTime)
        {
            var storage = prefab.AddComponent<Storage>();
            storage.storageFilters = new() { Elements.mucus.Tag };
            storage.capacityKg = capacityKg;
            storage.allowItemRemoval = false;

            var delivery = prefab.AddComponent<ManualDeliveryKG>();
            delivery.storage = storage;
            delivery.RequestedItemTag = Elements.mucus.Tag;
            delivery.allowPause = true;
            delivery.minimumMass = massUsedEachTime;
            delivery.refillMass = massUsedEachTime;
            delivery.choreTypeIDHash = Db.Get().ChoreTypes.MachineTinker.IdHash;

            var lubricatable = prefab.AddComponent<Lubricatable>();
            lubricatable.delivery = delivery;
            lubricatable.mucusStorage = storage;
            lubricatable.massUsedEachTime = massUsedEachTime;
        }

        private void OnRefreshUserMenu(object _)
        {
            var text = mucusRequested ? "Disable lubrication" : "Enable lubrication";
            var toolTip = mucusRequested
                ? "Stop delivering Mucus to this buildings."
                : "Enchance this building by lubricating it with Mucus.";

            var button = new KIconButtonMenu.ButtonInfo("action_bottler_delivery", text, OnToggle, tooltipText: toolTip);

            Game.Instance.userMenu.AddButton(gameObject, button);
        }

        private void OnToggle()
        {
            mucusRequested = !mucusRequested;
            UpdateDelivery();
        }

        private void UpdateDelivery()
        {
            var shouldAcceptDelivery = mucusRequested && !mucusStorage.IsFull();
            delivery.Pause(!shouldAcceptDelivery, "user toggle");

            if (!mucusRequested)
            {
                mucusStorage.DropAll();
            }
        }

        public int GetUsesRemaining() => Mathf.FloorToInt(mucusStorage.capacityKg / massUsedEachTime);

        public void OnImguiDraw()
        {
            ImGui.Text("Imgui from interface");
        }
    }
}
