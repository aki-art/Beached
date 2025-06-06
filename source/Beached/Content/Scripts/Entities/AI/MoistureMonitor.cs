using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class MoistureMonitor : GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>
	{
		private State wet;
		private DryStates dry;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = wet;

			root
				.EventHandler(GameHashes.Happy, smi => ToggleUnhappyModifier(smi, true))
				.EventHandler(GameHashes.Unhappy, smi => ToggleUnhappyModifier(smi, false))
				.EventHandler(GameHashes.TagsChanged, UpdateTags)
				.Enter(smi =>
				{
					if (smi.HasTag(GameTags.Creatures.Wild))
						smi.attributes.Add(smi.wildMucusModifier);
				});

			wet
				.Enter(Moisturize)
				.Enter(smi => SetMucusDelta(smi, 0))
				.ToggleAttributeModifier("producing mucus", smi => smi.wetMucusModifier)
				.UpdateTransition(dry.damp, (smi, dt) => !IsInLiquid(smi, dt));

			dry
				.DefaultState(dry.damp)
				.ToggleAttributeModifier("producing mucus", smi => smi.dryMucusModifier)
				.ToggleStateMachine(smi => new LubricatedMovementMonitor.Instance(smi.master))
				.ToggleAttributeModifier("DryingOut", smi => smi.baseMoistureModifier, null)
				.UpdateTransition(wet, IsInLiquid, UpdateRate.SIM_200ms)
				.UpdateTransition(dry.desiccating, IsCompletelyDry, UpdateRate.SIM_1000ms)
				.EventTransition(ModHashes.producedLubricant, dry.damp);

			dry.damp
				.Enter(smi => smi.hasBeenDryFor = 0)
				.Enter(smi => SetSpeedModifier(smi, 0.66f))
				.UpdateTransition(dry.secreting, UpdateDrying);

			dry.secreting
				.ToggleBehaviour(BTags.Creatures.secretingMucus, CanProduceLubricant);

			dry.desiccating
				.Enter(smi => SetSpeedModifier(smi, 0.33f))
				.ToggleBehaviour(BTags.Creatures.secretingMucus, CanProduceLubricant)
				.ToggleStatusItem(BStatusItems.desiccation, smi => smi)
				.Update(CheckDying);
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

		private void SetMucusDelta(Instance smi, float value)
		{
			smi.wetMucusModifier.SetValue(value);
		}

		private static void CheckDying(Instance smi, float dt)
		{
			smi.timeUntilDeath -= dt;

			if (smi.timeUntilDeath <= 0f)
			{
				Log.Debug("DEATH");
				var deathMonitor = smi.GetSMI<DeathMonitor.Instance>();
				if (deathMonitor != null)
					deathMonitor.Kill(BDeaths.desiccation);
				else
					Log.Warning("no death monitor on slickshell");

				smi.Trigger((int)ModHashes.desiccated);
			}
		}

		private static bool UpdateDrying(Instance smi, float dt)
		{
			smi.hasBeenDryFor += dt;
			smi.timeUntilDeath = smi.maxTimeUntilDeath;
			return smi.hasBeenDryFor >= 30f && smi.moisture.value < 80f;
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
			public State dry;
			public State resting;
			public State desiccating;
			public State secreting;
		}

		private static void SetSpeedModifier(Instance smi, float amount)
		{
			smi.navigator.defaultSpeed = smi.originalSpeed * amount;
		}

		private static void Moisturize(Instance smi)
		{
			Log.Debug("moisturizing");
			smi.moisture.SetValue(100f);
			smi.timeUntilDeath = smi.maxTimeUntilDeath;
			smi.navigator.defaultSpeed = smi.originalSpeed;
		}

		private static bool IsCompletelyDry(Instance smi, float _) => smi.moisture.value <= 0;

		private static bool IsInLiquid(Instance smi, float _) => Grid.IsSubstantialLiquid(Grid.PosToCell(smi), 0.05f);

		public class Def : BaseDef//, IGameObjectEffectDescriptor
		{
			public float defaultDryRate = -30f / CONSTS.CYCLE_LENGTH;
			public float defaultMucusRate = 30f / CONSTS.CYCLE_LENGTH;
			public SimHashes lubricant;
			public float lubricantTemperatureKelvin;

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
			public AttributeModifier baseMoistureModifier;
			public float originalSpeed;
			public Navigator navigator;
			[MyCmpReq] public Effects effects;
			public float hasBeenDryFor;
			public float timeUntilDeath;
			public float maxTimeUntilDeath = 60f;
			public AmountInstance mucusAmount;
			public AttributeModifier wetMucusModifier;
			public AttributeModifier dryMucusModifier;
			public AttributeModifier wildMucusModifier;
			public AttributeModifier unhappyMucusModifier;
			public Attributes attributes;
			public WildnessMonitor.Instance wildnessMonitor;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				attributes = master.gameObject.GetAttributes();

				moisture = BAmounts.Moisture.Lookup(gameObject);
				moisture.value = moisture.GetMax();

				baseMoistureModifier = new AttributeModifier(
					moisture.amount.deltaAttribute.Id,
					def.defaultDryRate,
					STRINGS.CREATURES.MODIFIERS.MOISTURE_LOSS_RATE.NAME);

				mucusAmount = BAmounts.Mucus.Lookup(gameObject);
				mucusAmount.value = 0;

				dryMucusModifier = new AttributeModifier(
					mucusAmount.amount.deltaAttribute.Id,
					def.defaultMucusRate,
					"Mucus Accumulation");

				wetMucusModifier = new AttributeModifier(
					mucusAmount.amount.deltaAttribute.Id,
					0,
					"Mucus Accumulation");

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
