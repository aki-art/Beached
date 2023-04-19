/*using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
    public class DNAInjector_old : GameStateMachine<DNAInjector_old, DNAInjector_old.Instance, IStateMachineTarget, DNAInjector_old.Def>
    {
        private State empty;
        private State ready;

        public static Dictionary<Tag, Option> options;

        public static void InitializeOptions()
        {
            var creatures = Assets.GetPrefabsWithComponent<Butcherable>();

            HashSet<Tag> meatDroppers;

            meatDroppers = new HashSet<Tag>();
            foreach (var creature in creatures)
            {
                if (creature.TryGetComponent(out Butcherable butcherable)
                    && butcherable.drops.Contains(MeatConfig.ID))
                {
                    var fertilityMonitor = creature.GetDef<FertilityMonitor.Def>();
                    if (fertilityMonitor != null && fertilityMonitor.eggPrefab != null)
                    {
                        meatDroppers.Add(fertilityMonitor.eggPrefab);
                    }
                }
            }

            options = new Dictionary<Tag, Option>()
            {
                { GeneticSamplesConfig.MEATY, new Option(GeneticSamplesConfig.MEATY, BCritterTraits.MEATY, "beached_gmo_meaty", meatDroppers)},
                { GeneticSamplesConfig.EVERLASTING, new Option(GeneticSamplesConfig.EVERLASTING, BCritterTraits.EVERLASTING, "beached_gmo_everlasting") },
            };
        }

        public class Option
        {
            public readonly Tag sampleTag;
            public readonly string traitId;
            public readonly Trait trait;
            public readonly HashSet<Tag> eligibleEggs;
            public readonly string name;
            public readonly string description;
            public readonly string detailedDescription;
            public readonly string sprite;
            public readonly Ingredient[] ingredients;

            public Option(Tag sampleTag, string traitId, string sprite, HashSet<Tag> eligibleEggs = null)
            {
                this.sampleTag = sampleTag;
                this.traitId = traitId;
                this.sprite = sprite;
                this.eligibleEggs = eligibleEggs;
                trait = Db.Get().traits.TryGet(traitId);
                description = trait.description;
                name = trait.Name;
                var eligibles = eligibleEggs == null ? "Everyone" : $"<size=80%>{eligibleEggs.Join(tag => tag.ProperName(), ", ")}</size>";
                detailedDescription = $"{trait.description} \n\n <b>Eligible:</b> {eligibles}";
            }

            public bool IsValidForEgg(string eggId) => trait != null && (eligibleEggs == null || eligibleEggs.Contains(eggId));

            public Sprite GetIcon() => Assets.GetSprite(sprite);
        }

        public class Ingredient : IConfigurableConsumerIngredient
        {
            public Tag[] IngredientSet = null;
            public float AmountKG = 0.0f;

            public float GetAmount() => AmountKG;

            public Tag[] GetIDSets() => IngredientSet;
        }

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = empty;
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            private Storage eggStorage;
            private FilteredStorage eggStorageFilter;
            private Tag[] forbiddenEggs;

            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
                var storages = gameObject.GetComponents<Storage>();
                eggStorage = storages[0];

                eggStorageFilter = new FilteredStorage(GetComponent<KPrefabID>(), forbiddenEggs, null, false, Db.Get().ChoreTypes.CreatureFetch);
                eggStorageFilter.SetHasMeter(false);
            }
        }
    }
}
*/