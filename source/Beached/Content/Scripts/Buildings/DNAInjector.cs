using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts.Items;
using HarmonyLib;
using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DNAInjector : StateMachineComponent<DNAInjector.StatesInstance>
	{
		private const string EGG_TARGET = "egg_target";

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
						if (rule.Value(creature))
						{
							eggListsByRule[rule.Key].Add(fertilityMonitor.eggPrefab);
						}
					}
				}
			}

			options = new Dictionary<Tag, Option>()
			{
				{
					GeneticSamplesConfig.PRODUCTIVE1,
					new Option(
						GeneticSamplesConfig.PRODUCTIVE1,
						BCritterTraits.PRODUCTIVE1,
						"beached_productive_i",
						eggListsByRule[Tuning.Genetics.RULE_ALL])
				},
				{
					GeneticSamplesConfig.PRODUCTIVE2,
					new Option(
						GeneticSamplesConfig.PRODUCTIVE2,
						BCritterTraits.PRODUCTIVE2,
						"beached_productive_ii",
						eggListsByRule[Tuning.Genetics.RULE_ALL])
				},
				{
					GeneticSamplesConfig.PRODUCTIVE3,
					new Option(
						GeneticSamplesConfig.PRODUCTIVE3,
						BCritterTraits.PRODUCTIVE3,
						"beached_productive_iii",
						eggListsByRule[Tuning.Genetics.RULE_ALL])
				},
				{
					GeneticSamplesConfig.FABULOUS,
					new Option(
						GeneticSamplesConfig.FABULOUS,
						BCritterTraits.FABULOUS,
						"beached_fabulous",
						eggListsByRule[Tuning.Genetics.RULE_ALL])
				},
				{
					GeneticSamplesConfig.MEATY,
					new Option(
						GeneticSamplesConfig.MEATY,
						BCritterTraits.MEATY,
						"beached_gmo_meaty",
						eggListsByRule[Tuning.Genetics.RULE_MEATDROPPERS])
				},
				{
					GeneticSamplesConfig.BLAND,
					new Option(
						GeneticSamplesConfig.BLAND,
						BCritterTraits.BLAND,
						"beached_bland",
						eggListsByRule[Tuning.Genetics.RULE_DECORPROVIDERS])
				},
				{
					GeneticSamplesConfig.LASTING,
					new Option(
						GeneticSamplesConfig.LASTING,
						BCritterTraits.LASTING,
						"beached_lasting",
						eggListsByRule[Tuning.Genetics.RULE_ALL])
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
					GeneticSamplesConfig.HYPOALLERGENIC,
					new Option(
						GeneticSamplesConfig.HYPOALLERGENIC,
						BCritterTraits.HYPOALLERGENIC,
						"beached_hypoallergenic",
						eggListsByRule[Tuning.Genetics.RULE_FURRY])
				},
			};
		}

		public override void OnSpawn()
		{
			smi.StartSM();

			var storages = GetComponents<Storage>();
			eggStorage = storages[0];
			sampleStorage = storages[1];

			eggStorageFilter = new FilteredStorage(GetComponent<KPrefabID>(), new[]
			{
				BTags.geneticallyModified,
				Tag.Invalid
			}, null, false, Db.Get().ChoreTypes.CreatureFetch);

			eggStorageFilter.SetHasMeter(false);

			SetSelectedSample(selectedSample);
		}

		public void SetSelectedSample(Tag sampleTag)
		{
			Log.Debug($"set selected sample: {sampleTag}");
			selectedSample = sampleTag;

			//operational.SetFlag(spiceSet, sampleTag != null);

			// TODO: set tint of tank

			if (tag == Tag.Invalid)
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
				eggStorageFilter.forbiddenTags[1] = TagManager.Create(sampleTag + "injected");
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

		public void ApplyTrait()
		{
			if (smi.currentEgg == null)
			{
				Log.Warning("DNAInjector: finished injeting DNA to egg, but there was no egg");
				return;
			}

			if (smi.currentEgg.TryGetComponent(out KPrefabID kPrefabID))
			{
				kPrefabID.AddTag(TagManager.Create(selectedSample + "injected"), true);
				kPrefabID.AddTag(BTags.geneticallyModified, true);
			}

			if (smi.currentEgg.TryGetComponent(out Beached_GeneticallyModifiableEgg modifiableEgg))
			{
				if (options.TryGetValue(selectedSample, out var option))
				{
					modifiableEgg.ApplyTrait(option.traitId);
				}
			}

			sampleStorage.ConsumeAndGetDisease(selectedSample, 1f / 20f, out _, out _, out _);
			eggStorage.Drop(smi.currentEgg);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, DNAInjector, object>.GameInstance
		{
			public GameObject currentEgg;
			KBatchedAnimController eggKbac;
			KBatchedAnimController kbac;
			KBatchedAnimTracker tracker;

			[MyCmpReq]
			public DNAInjectorWorkable workable;

			public Operational operational;

			public StatesInstance(DNAInjector master) : base(master)
			{
				kbac = master.GetComponent<KBatchedAnimController>();
				operational = master.GetComponent<Operational>();

				SetupEggSymbol();
				//UpdateEggSymbol();
				PositionActiveEgg();
			}

			public void UpdateEggSymbol()
			{
				if (eggKbac == null)
				{
					return;
				}

				var hasEgg = currentEgg != null;
				eggKbac.gameObject.SetActive(hasEgg);

				if (!hasEgg)
				{
					return;
				}

				eggKbac.SwapAnims(currentEgg.GetComponent<KBatchedAnimController>().AnimFiles);
				eggKbac.Play("idle", KAnim.PlayMode.Paused);
			}

			public void SetupEggSymbol()
			{
				var gameObject = new GameObject("eggSymbol");
				gameObject.SetActive(false);

				var column = (Vector3)kbac
					.GetSymbolTransform(EGG_TARGET, out bool _)
					.GetColumn(3) with
				{
					z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
				};

				gameObject.transform.SetPosition(column);
				eggKbac = gameObject.AddComponent<KBatchedAnimController>();
				eggKbac.AnimFiles = new[]
				{
					Assets.GetAnim( "egg_hatch_kanim")
				};

				tracker = gameObject.AddComponent<KBatchedAnimTracker>();
				tracker.symbol = EGG_TARGET;
				tracker.forceAlwaysVisible = true;

				tracker.SetAnimControllers(eggKbac, kbac);

				eggKbac.initialAnim = "idle";

				kbac.SetSymbolVisiblity((KAnimHashedString)EGG_TARGET, false);
			}

			private void PositionActiveEgg()
			{
				if (currentEgg == null)
					return;

				var component1 = this.currentEgg.GetComponent<KBatchedAnimController>();
				component1.enabled = true;
				component1.SetSceneLayer(Grid.SceneLayer.BuildingUse);

				KSelectable component2 = currentEgg.GetComponent<KSelectable>();
				if (component2 != null)
					component2.enabled = true;

				tracker = currentEgg.AddComponent<KBatchedAnimTracker>();
				tracker.symbol = EGG_TARGET;
			}

			public void UpdateEgg()
			{
				if (currentEgg != null)
				{
					if (master.eggStorage.items.Contains(currentEgg))
					{
						return;
					}
					else if (tracker != null)
					{
						Destroy(tracker);
					}
				}

				currentEgg = master.eggStorage.FindFirst(GameTags.Egg);
				Beached.Log.Debug(currentEgg?.PrefabID());
				//UpdateEggSymbol();
				PositionActiveEgg();
			}

			public void ApplyGenesToEgg()
			{
				if (currentEgg == null)
				{
					return;
				}

				Destroy(tracker);
			}
		}

		public class States : GameStateMachine<States, StatesInstance, DNAInjector>
		{
			public State unoperational;
			public State operational;
			public State ready;
			public BoolParameter isReady;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = unoperational;

				root
					.EventHandler(GameHashes.OnStorageChange, OnStorageChanged);

				unoperational
					.EventTransition(GameHashes.OperationalChanged, ready, IsOperational);

				operational
					.EventTransition(GameHashes.OperationalChanged, unoperational, Not(IsOperational))
					.ParamTransition(isReady, ready, IsReady)
					.PlayAnim("on");

				ready
					.EventTransition(GameHashes.OperationalChanged, unoperational, Not(IsOperational))
					.ParamTransition(isReady, operational, NoLongerReady)
					.ToggleRecurringChore(CreateChore);
			}

			private bool IsOperational(StatesInstance smi) => smi.operational.IsOperational;

			private bool IsReady(StatesInstance smi, bool ready) => isReady.Get(smi);

			private bool NoLongerReady(StatesInstance smi, bool ready) => !isReady.Get(smi);

			private Chore CreateChore(StatesInstance smi) => new WorkChore<DNAInjectorWorkable>(Db.Get().ChoreTypes.MachineFetch, smi.workable);

			private void OnStorageChanged(StatesInstance smi, object data)
			{
				smi.UpdateEgg();
				isReady.Set(smi.currentEgg != null && smi.master.sampleStorage.MassStored() >= 1f / 20f, smi);
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
