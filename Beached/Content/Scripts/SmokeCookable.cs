using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class SmokeCookable : KMonoBehaviour, ISim4000ms
    {
        public const float DEFAULT_TEMPERATURE = 343.15f; // 70 °C

        [MyCmpReq] private KSelectable kSelectable;

        [SerializeField] public float cyclesToSmoke;
        [SerializeField] public Tag smokedItemTag;
        [SerializeField] public float requiredTemperature;

        [Serialize] public float progress;

        private float totalSeconds;

        /// <summary>
        /// Modders: to add <see cref="ModAPI.AddSmokeableFood(string, string, float)"/> 
        /// and to remove <see cref="ModAPI.RemoveSmokeableFood(string)"/>
        /// </summary>
        public static Dictionary<Tag, SmokableInfo> smokables = new()
        {
            { MeatConfig.ID, new SmokableInfo(SmokedMeatConfig.ID, 0.4f) },
            { BasicPlantFoodConfig.ID, new SmokableInfo(SmokedLiceConfig.ID, 0.4f) },
            { TofuConfig.ID, new SmokableInfo(SmokedTofuConfig.ID, 0.4f) },
            { FishMeatConfig.ID, new SmokableInfo(SmokedFishConfig.ID, 0.4f) },
        };

        public struct SmokableInfo
        {
            public float timeToSmokeInCycles;
            public float requiredTemperature;
            public Tag smokedItem;

            public SmokableInfo(Tag smokedItem, float timeToSmoke = 0.4f, float requiredTemperature = DEFAULT_TEMPERATURE)
            {
                this.timeToSmokeInCycles = timeToSmoke;
                this.smokedItem = smokedItem;
                this.requiredTemperature = requiredTemperature;
            }
        }

        public override void OnSpawn()
        {
            totalSeconds = cyclesToSmoke * CONSTS.CYCLE_LENGTH;
            kSelectable.AddStatusItem(BStatusItems.smoking);
        }

        public void Sim4000ms(float dt)
        {
            var isInSmoke = false;

            if (IsInCO2())
            {
                progress += dt;
                isInSmoke = true;

                if (progress >= totalSeconds)
                {
                    GetSmoked();
                    return;
                }
            }

            kSelectable.ToggleStatusItem(BStatusItems.smoking, isInSmoke, this);
        }

        private void GetSmoked()
        {
            var spawnedItem = MiscUtil.Spawn(smokedItemTag, gameObject);
            if (spawnedItem.TryGetComponent(out PrimaryElement smokedPrimaryElement) &&
                TryGetComponent(out PrimaryElement thisPrimaryElement))
            {
                smokedPrimaryElement.SetMassTemperature(thisPrimaryElement.Mass, thisPrimaryElement.Temperature);
                smokedPrimaryElement.AddDisease(thisPrimaryElement.DiseaseIdx, thisPrimaryElement.diseaseCount, "copy from raw food");
            }

            // keep selection consistent
            if (SelectTool.Instance != null
                && kSelectable != null
                && SelectTool.Instance.selected == kSelectable)
            {
                SelectTool.Instance.Select(spawnedItem.GetComponent<KSelectable>());
            }

            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, "Smoked", transform, Vector3.zero);

            Util.KDestroyGameObject(gameObject);
        }

        private bool IsInCO2()
        {
            var cell = Grid.PosToCell(this);
            return Grid.Element[cell].id == SimHashes.CarbonDioxide && Grid.Temperature[cell] >= requiredTemperature;
        }

        public string GetStatusItemTooltip(string str)
        {
            var progressPercent = progress / totalSeconds;
            var formattedPercent = GameUtil.GetFormattedPercent(progressPercent);

            return string.Format(str, formattedPercent);
        }
    }
}
