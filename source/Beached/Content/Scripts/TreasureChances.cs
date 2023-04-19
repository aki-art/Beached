using ProcGen;
using System;
using System.Collections.Generic;
using static Beached.Content.Scripts.TreasureChances;

namespace Beached.Content.Scripts
{
    public class TreasureChances : Dictionary<SimHashes, TreasureSource>
    {
        public TreasureSource CreateTreasureEntry(SimHashes element, float extraLootChance, float minimumMassKg = 300f)
        {
            if (ContainsKey(element))
            {
                Log.Warning("Trying to add duplicate entry to TreasureChances: " + element);
            }
            else
            {
                this[element] = new()
                {
                    extraLootChance = extraLootChance,
                    minimumMassKg = minimumMassKg,
                    treasures = new()
                };
            }

            return this[element];
        }

        [Serializable]
        public class TreasureSource
        {
            public List<TreasureConfig> treasures;
            public float extraLootChance;
            public float minimumMassKg = 300f;

            public TreasureSource Add(string tag, float amount = 1f, float weight = 1f, bool notifyPlayer = false)
            {
                treasures.Add(new()
                {
                    tag = tag,
                    amount = amount,
                    weight = weight,
                    notifyPlayer = notifyPlayer
                });

                return this;
            }

            public TreasureConfig GetRandomTreasure() => MiscUtil.GetWeightedRandom(treasures);

            public bool Roll() => UnityEngine.Random.value < extraLootChance;
        }

        [Serializable]
        public class TreasureConfig : IWeighted
        {
            public string tag;

            public float amount { get; set; }

            public float weight { get; set; }

            public bool notifyPlayer { get; set; }
        }
    }
}
