using Beached.Utils;
using ProcGen;
using System;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
    public class TreasureChances
    {
        public static Dictionary<SimHashes, TreasureChances> treasureChances = new();

        public List<TreasureConfig> treasures;
        public float extraLootChance;
        public float minimumMassKg = 300f;

        public static void AddTreasure(SimHashes element, float extraLootChance, List<TreasureConfig> treasures, float minimumMassKg = 300f)
        {
            treasureChances[element] = new TreasureChances()
            {
                treasures = treasures,
                extraLootChance = extraLootChance,
                minimumMassKg = minimumMassKg
            };
        }

        public static TreasureChances AddSingle(SimHashes element, float chance, string tag, float amount, bool notifyPlayer = false, float weight = 1f, float minimumMass = 300f)
        {
            treasureChances[element] = new TreasureChances()
            {
                treasures = new List<TreasureConfig>() { new TreasureConfig(tag, amount, weight, notifyPlayer) },
                extraLootChance = chance,
                minimumMassKg = minimumMass
            };

            return treasureChances[element];
        }

        public TreasureChances AddSingle(string tag, float amount, float weight = 1f)
        {
            treasures = new List<TreasureConfig>()
            {
                new TreasureConfig(tag, amount, weight, true)
            };

            return this;
        }

        public void OnExcavation(Diggable diggable, int cell, Element element, MinionResume minion)
        {
            var mass = Grid.Mass[cell];

            Log.Debug(mass);

            if (mass < minimumMassKg)
            {
                Log.Debug("not enough mass " + minimumMassKg);
                //return;
            }

            if (extraLootChance < UnityEngine.Random.value)
            {
                Log.Debug("roll fail");
                return;
            }

            var item = MiscUtil.GetWeightedRandom(treasures);

            if (item != null)
            {
                Log.Debug("rolled item " + item.tag);
                var loot = MiscUtil.Spawn(item.tag, diggable.gameObject);

                var primaryElement = loot.GetComponent<PrimaryElement>();
                primaryElement.Mass = item.amount;
                primaryElement.Temperature = Grid.Temperature[cell];
                // TODO: disease

                if (item.notifyPlayer)
                {
                    //notifier.AutoClickFocus = true;

                    //notifier.Add(notification, "");
                }

                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, loot.GetProperName(), loot.transform);
            }
        }

        [Serializable]
        public class TreasureConfig : IWeighted
        {
            public string tag;

            public float amount { get; set; }

            public float weight { get; set; }

            public bool notifyPlayer { get; set; }

            public TreasureConfig(string tag, float amount, float weight, bool notifyPlayer)
            {
                this.tag = tag;
                this.weight = weight;
                this.amount = amount;
                this.notifyPlayer = notifyPlayer;
            }
        }
    }
}
