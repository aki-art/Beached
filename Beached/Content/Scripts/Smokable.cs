using System.Collections.Generic;
using System.Linq;
using Beached.Content.Defs.Items.Foods;
using Beached.Content.ModDb;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
    internal class Smokable : KMonoBehaviour, ISim4000ms
    {
        public float requiredTemperature = MiscUtil.CelsiusToKelvin(70);

        [MyCmpReq] private KSelectable kSelectable;

        [SerializeField] public float cyclesToSmoke;
        [SerializeField] public Tag smokedItemTag;

        [Serialize] public float progress;
        private float totalSeconds;

        public static Dictionary<Tag, (float time, Tag smokedItem)> smokables = new()
        {
            { MeatConfig.ID, (0.4f, SmokedMeatConfig.ID)}
        };

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
                    var spawnedItem = MiscUtil.Spawn(smokedItemTag, gameObject);
                    if (spawnedItem.TryGetComponent(out PrimaryElement smokedPrimaryElement) &&
                        TryGetComponent(out PrimaryElement thisPrimaryElement))
                    {
                        smokedPrimaryElement.SetMassTemperature(thisPrimaryElement.Mass, thisPrimaryElement.Temperature);
                        smokedPrimaryElement.AddDisease(thisPrimaryElement.DiseaseIdx, thisPrimaryElement.diseaseCount, "copy from raw food");
                    }

                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, "Smoked", transform, Vector3.zero);

                    Util.KDestroyGameObject(gameObject);
                }
            }

            kSelectable.ToggleStatusItem(BStatusItems.smoking, isInSmoke, this);
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
