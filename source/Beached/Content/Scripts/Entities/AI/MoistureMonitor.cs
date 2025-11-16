using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class MoistureMonitor : GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>
	{
		private State wet;
		private DryStates dry;
		public State desiccating;
		public State dead;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = wet;

			root
				.EventHandler(GameHashes.Happy, smi => ToggleUnhappyModifier(smi, true))
				.EventHandler(GameHashes.Unhappy, smi => ToggleUnhappyModifier(smi, false))
				.ToggleStateMachine(smi => new LubricatedMovementMonitor.Instance(smi.master))
				.EventHandler(GameHashes.TagsChanged, UpdateTags)
				.EventTransition(GameHashes.Died, dead)
				.Enter(smi =>
				{
					if (smi.HasTag(GameTags.Creatures.Wild))
						smi.attributes.Add(smi.wildMucusModifier);
				});

			wet
				.Enter(smi => SetSpeedModifier(smi, 1.0f))
				.UpdateTransition(dry.damp, Dry);

			dry
				.DefaultState(dry.damp)
				.UpdateTransition(wet, NotDry, UpdateRate.SIM_1000ms)
				.UpdateTransition(desiccating, IsCompletelyDry, UpdateRate.SIM_1000ms)
				.EventTransition(ModHashes.producedLubricant, dry.damp);

			dry.damp
				.Enter(smi => SetSpeedModifier(smi, 0.66f))
				.UpdateTransition(dry.secreting, Dry);

			dry.secreting
				.ToggleBehaviour(BTags.Creatures.secretingMucus, CanProduceLubricant);

			//TODO add back the "dry for" tracker, because if the liquid is immediately removed now a loop forms here
			desiccating
				.Enter(smi => SetSpeedModifier(smi, 0.33f))
				.ToggleStatusItem(BStatusItems.desiccation, smi => smi)
				//.UpdateTransition(dry.secreting, (smi, dt) => CanProduceLubricant(smi))
				.Update(CheckDying, UpdateRate.SIM_4000ms);

			dead
				.DoNothing();
		}

		private static bool IsMoisturized(Instance smi, float moistureTreshold) => smi.moisture.value > moistureTreshold;

		private static bool NotDry(Instance smi, float _) => IsMoisturized(smi, 30.0f);

		private static bool Dry(Instance smi, float _) => !IsMoisturized(smi, 30.0f);

		private static bool IsCompletelyDry(Instance smi, float _) => smi.moisture.value <= 0;

		private void UpdateTags(Instance smi, object data)
		{
			if (data is TagChangedEventData tagData && tagData.tag == GameTags.Creatures.Wild)
			{
				if (tagData.added)
					smi.attributes.Add(smi.wildMucusModifier);
				else
					smi.attributes.Remove(smi.wildMucusModifier);
			}
		}

		private void ToggleUnhappyModifier(Instance smi, bool enabled)
		{
			if (enabled)
				smi.attributes.Add(smi.unhappyMucusModifier);
			else
				smi.attributes.Remove(smi.unhappyMucusModifier);
		}

		private static void CheckDying(Instance smi, float dt)
		{
			var health = smi.GetComponent<Health>();

			health.Damage(smi.def.desiccationDamagePerSecond * dt);

			if (health.IsDefeated())
			{
				smi.Trigger((int)ModHashes.desiccated);
			}
		}

		private static bool CanProduceLubricant(Instance smi)
		{
			if (smi.effects.HasEffect(BEffects.RECENTLY_PRODUCED_LUBRICANT))
				return false;

			if (smi.mucusAmount.value < 5f)
				return false;

			var cell = Grid.CellBelow(Grid.PosToCell(smi));

			return Grid.IsValidCell(cell) && Grid.IsSolidCell(cell);
		}

		public class DryStates : State
		{
			public State damp;
			public State secreting;
		}


		private static void SetSpeedModifier(Instance smi, float amount)
		{
			smi.navigator.defaultSpeed = smi.originalSpeed * amount;
		}

		public class Def : BaseDef, ICodexEntry//, IGameObjectEffectDescriptor
		{
			public float defaultMucusRate = 30f / CONSTS.CYCLE_LENGTH;
			public SimHashes lubricant;
			public float lubricantTemperatureKelvin;
			public float sufficientMoistureTreshold = 10f;
			public float desiccationDamagePerSecond = 0.1f;

			public void AddCodexEntries(CodexEntryGenerator_Elements.ElementEntryContext context, KPrefabID prefab)
			{
				var conversionEntry = CodexUtil.SimpleConversionBase(context, prefab.gameObject, $"Passively excreted by {prefab.GetProperName()}");

				var use = new ElementUsage(lubricant.CreateTag(), defaultMucusRate, true)
				{
					customFormating = (tag, amount, continous) => $"<size=70%>max.</size>{GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle)}" // TODO STRING
				};

				conversionEntry.outSet.Add(use);
				context.madeMap.Add(lubricant.CreateTag(), conversionEntry);
			}

			public int CodexEntrySortOrder() => 15;

			public override void Configure(GameObject prefab)
			{
				var initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
				initialAmounts.Add(BAmounts.Moisture.Id);
				initialAmounts.Add(BAmounts.Mucus.Id);
			}
		}

		public new class Instance : GameInstance
		{
			public AmountInstance moisture;
			public float originalSpeed;
			public Navigator navigator;
			[MyCmpReq] public Effects effects;
			public AmountInstance mucusAmount;
			public AttributeModifier wildMucusModifier;
			public AttributeModifier unhappyMucusModifier;
			public Attributes attributes;
			public WildnessMonitor.Instance wildnessMonitor;
			private Health health;

			public float GetEstimatedTimeUntilDeath()
			{
				return smi.IsInsideState(smi.sm.desiccating) ? health.hitPoints / def.desiccationDamagePerSecond : float.NaN;
			}

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				health = master.GetComponent<Health>();
				attributes = master.gameObject.GetAttributes();

				moisture = BAmounts.Moisture.Lookup(gameObject);

				mucusAmount = BAmounts.Mucus.Lookup(gameObject);
				mucusAmount.value = mucusAmount.GetMax();

				unhappyMucusModifier = new AttributeModifier(
					mucusAmount.amount.deltaAttribute.Id,
					-0.5f,
					"Mucus Accumulation",
					true);

				wildMucusModifier = new AttributeModifier(
					mucusAmount.amount.deltaAttribute.Id,
					-0.75f,
					"Wild Mucus Accumulation",
					true);

				navigator = smi.GetComponent<Navigator>();
				originalSpeed = navigator.defaultSpeed;
			}

			public void ProduceLubricant()
			{
				var mass = mucusAmount.value;

				if (mass > 0f)
				{
					BubbleManager.instance.SpawnBubble(
						transform.GetPosition(),
						Vector2.zero,
						def.lubricant,
						mass,
						def.lubricantTemperatureKelvin);

					Trigger(ModHashes.producedLubricant);
					effects.Add(BEffects.RECENTLY_PRODUCED_LUBRICANT, true);

					mucusAmount.value = 0;
				}
			}
		}
	}
}
