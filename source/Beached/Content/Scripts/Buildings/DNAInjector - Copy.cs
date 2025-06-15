/*using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using HarmonyLib;
using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FetchAreaChore.StatesInstance;

namespace Beached.Content.Scripts.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class DNAInjector : StateMachineComponent<DNAInjector.StatesInstance>
    {
        [Serialize] public Tag selectedSample;

        public static Operational.Flag spiceSet = new("Beached_GeneticSampleSelected", Operational.Flag.Type.Requirement);
        public static Dictionary<Tag, Option> options;

        [MyCmpReq] private ManualDeliveryKG manualDelivery;
        [MyCmpReq] private TreeFilterable treeFilterable;
        [MyCmpReq] private SimpleFlatFilterable flatFilteable;
        [MyCmpReq] private KSelectable kSelectable;
        [MyCmpReq] private Operational operational;
        private Storage sampleStorage;
        private Storage eggStorage;
        private FilteredStorage eggStorageFilter = null;

        public static void InitializeOptions()
        {
            var creatures = Assets.GetPrefabsWithComponent<CreatureBrain>();

            var eggListsByRule = new Dictionary<string, HashSet<Tag>>();
            Tuning.Genetics.rules.Do(rule => eggListsByRule.Add(rule.Key, new HashSet<Tag>()));

            foreach (var creature in creatures)
            {
                var fertilityMonitor = creature.GetDef<FertilityMonitor.Def>();
                if (fertilityMonitor != null && fertilityMonitor.eggPrefab != null)
                {
                    foreach (var rule in Tuning.Genetics.rules)
                    {
                        if(rule.Value(creature))
                        {
                            eggListsByRule[rule.Key].Add(fertilityMonitor.eggPrefab);
                        }
                    }
                }
            }

            options = new Dictionary<Tag, Option>()
            {
                {
                    GeneticSamplesConfig.MEATY,
                    new Option(
                        GeneticSamplesConfig.MEATY,
                        BCritterTraits.MEATY,
                        "beached_gmo_meaty",
                        eggListsByRule[Tuning.Genetics.RULE_MEATDROPPERS])
                },
                {
                    GeneticSamplesConfig.EVERLASTING,
                    new Option(
                        GeneticSamplesConfig.EVERLASTING,
                        BCritterTraits.EVERLASTING,
                        "beached_gmo_everlasting",
                        eggListsByRule[Tuning.Genetics.RULE_ALL])
                },
                {
                    GeneticSamplesConfig.BLAND,
                    new Option(
                        GeneticSamplesConfig.BLAND,
                        BCritterTraits.BLAND,
                        "beached_gmo_everlasting",
                        eggListsByRule[Tuning.Genetics.RULE_DECORPROVIDERS])
                },
            };
        }

        public override void OnSpawn()
        {
            telepadInstance.StartSM();
            Log.Debug("treefilterable target: " + treeFilterable.storage.storageFilters.Join(t => t.ToString(), ", "));
            var storages = GetComponents<Storage>();
            eggStorage = storages[0];
            sampleStorage = storages[1];

            eggStorageFilter = new FilteredStorage(GetComponent<KPrefabID>(), null, null, false, Db.Get().ChoreTypes.CreatureFetch);
            eggStorageFilter.SetHasMeter(false);
        }

        public void SetSelectedSample(Tag sampleTag)
        {
            Log.Debug($"set selected sample: {sampleTag}");
            selectedSample = sampleTag;

            //operational.SetFlag(spiceSet, sampleTag != null);
            
            // TODO: set tint of tank

            if(tag == Tag.Invalid)
            {
                CancelDelivery();

            }
            else if (manualDelivery.RequestedItemTag != sampleTag)
            {
                sampleStorage.DropAll();
                manualDelivery.RequestedItemTag = sampleTag;
                manualDelivery.Pause(false, "New Genetic Trait Selected");
                manualDelivery.RequestDelivery();
            }

            if (options.TryGetValue(sampleTag, out var sample))
            {
                var tags = sample.eligibleEggs ?? BTags.eggs.tags.ToHashSet();
                flatFilteable.tagOptions = tags;
                treeFilterable.UpdateFilters(tags);
                eggStorageFilter.FilterChanged();
            }

            RefreshSideScreen();
        }

        public void CancelDelivery()
        {
            manualDelivery.RequestedItemTag = Tag.Invalid;
            manualDelivery.Pause(true, "Cancelled");

            RefreshSideScreen();
        }

        public void RefreshSideScreen()
        {
            if (kSelectable == null || !kSelectable.IsSelected)
            {
                return;
            }

            DetailsScreen.Instance.Refresh(gameObject);
        }

        public Tag GetSelectedOption()
        {
            return selectedSample;
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, DNAInjector, object>.GameInstance
        {
            public StatesInstance(DNAInjector master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, DNAInjector>
        {

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = root;
            }
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

    }
}
*/