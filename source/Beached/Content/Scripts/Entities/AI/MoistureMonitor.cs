using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class MoistureMonitor : GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>
	{
		private State onDryLand;
		private State inLiquid;
		private State secreting;
		public State dead;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = onDryLand;

			root
				.ToggleStateMachine(smi => new LubricatedMovementMonitor.Instance(smi.master))
				.EventHandler(GameHashes.Happy, smi => ToggleUnhappyModifier(smi, true))
				.EventHandler(GameHashes.Unhappy, smi => ToggleUnhappyModifier(smi, false))
				.EventHandler(GameHashes.TagsChanged, UpdateTags)
				.EventTransition(GameHashes.Died, dead)
				.Enter(smi =>
				{
					if (smi.HasTag(GameTags.Creatures.Wild))
						smi.attributes.Add(smi.wildMucusModifier);
				});

			// not in liquid currently. produce mucus if gland is full or frying out
			onDryLand
				.UpdateTransition(inLiquid, IsInLiquid)
				.ToggleAttributeModifier("dry", smi => smi.baseMoistureModifier)
				.UpdateTransition(secreting, IsMucusEnough);

			// in liquid, happy and satisfied. do not make mucus
			inLiquid
				.UpdateTransition(onDryLand, (smi, dt) => !IsInLiquid(smi, dt))
				.ToggleAttributeModifier("wet", smi => smi.wetMoistureModifier);

			secreting
				.ToggleBehaviour(BTags.Creatures.secretingMucus, CanProduceLubricant, smi => smi.GoTo(onDryLand));

			dead
				.DoNothing();
		}

		private static bool IsInLiquid(Instance smi, float _) => Grid.IsSubstantialLiquid(Grid.PosToCell(smi), 0.05f);

		private bool IsMucusEnough(Instance smi, float _)
		{
			var treshold = smi.HasTag(BTags.Creatures.dry) ? 1f : 10.0f;
			return smi.mucusAmount.value >= treshold;
		}

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

		private static bool CanProduceLubricant(Instance smi)
		{
			if (smi.effects.HasEffect(BEffects.RECENTLY_PRODUCED_LUBRICANT))
				return false;

			var cell = Grid.CellBelow(Grid.PosToCell(smi));

			return Grid.IsValidCell(cell) && Grid.IsSolidCell(cell);
		}

		public class Def : BaseDef, ICodexEntry//, IGameObjectEffectDescriptor
		{
			public float defaultMucusRate = 30f / CONSTS.CYCLE_LENGTH;
			public SimHashes lubricant;
			public float lubricantTemperatureKelvin;
			public float sufficientMoistureTreshold = 10f;
			public float defaultDryRate = -30f / CONSTS.CYCLE_LENGTH;
			public float defaultSoakRate = 6000f / CONSTS.CYCLE_LENGTH;

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
			[MyCmpReq] public Effects effects;
			public AmountInstance mucusAmount;
			public AttributeModifier wildMucusModifier;
			public AttributeModifier unhappyMucusModifier;
			public Attributes attributes;
			public WildnessMonitor.Instance wildnessMonitor;

			public AmountInstance moisture;
			public AttributeModifier baseMoistureModifier;
			public AttributeModifier wetMoistureModifier;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				moisture = BAmounts.Moisture.Lookup(gameObject);
				moisture.value = moisture.GetMax();

				baseMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					def.defaultDryRate,
					STRINGS.CREATURES.MODIFIERS.MOISTURE_LOSS_RATE.NAME);

				wetMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					def.defaultSoakRate,
					STRINGS.CREATURES.MODIFIERS.MOISTURE_GAIN_RATE.NAME);

				attributes = master.gameObject.GetAttributes();

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
