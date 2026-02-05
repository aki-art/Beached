using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class DesiccationMonitor : GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>
	{
		private State wet;
		private State dry;
		private State desiccating;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = wet;

			// in liquid and satisfied
			wet
				.Enter(smi => SetSpeedModifier(smi, 1.0f))
				.UpdateTransition(dry, Dry);

			// not in liquid but fine
			dry
				.UpdateTransition(wet, NotDry, UpdateRate.SIM_1000ms)
				.UpdateTransition(desiccating, IsCompletelyDry, UpdateRate.SIM_1000ms)
				.ToggleTag(BTags.Creatures.dry)
				.Enter(smi => SetSpeedModifier(smi, 0.66f));

			// not in liquid and dying
			desiccating
				.Enter(smi =>
				{
					SetSpeedModifier(smi, 0.33f);
					smi.ApplySadLook();
				})
				.Exit(smi => smi.RemoveSadLook())
				.UpdateTransition(wet, (smi, dt) => !IsCompletelyDry(smi, dt), UpdateRate.SIM_200ms)
				.ToggleStatusItem(BStatusItems.desiccation, smi => smi)
				.ToggleTag(BTags.Creatures.dry)
				.Update(CheckDying, UpdateRate.SIM_4000ms);
		}

		private static void CheckDying(Instance smi, float dt)
		{
			smi.health.Damage(smi.def.desiccationDamagePerSecond * dt);

			if (smi.health.IsDefeated())
				smi.Trigger((int)ModHashes.desiccated);
		}

		private static bool IsMoisturized(Instance smi, float moistureTreshold) => smi.moisture.value > moistureTreshold;

		private static bool NotDry(Instance smi, float _) => IsMoisturized(smi, 30.0f);

		private static bool Dry(Instance smi, float _) => !IsMoisturized(smi, 30.0f);

		private static bool IsCompletelyDry(Instance smi, float _) => smi.moisture.value <= 0;

		private static void SetSpeedModifier(Instance smi, float amount)
		{
			smi.navigator.defaultSpeed = smi.originalSpeed * amount;
		}

		public class Def : BaseDef
		{
			public float desiccationDamagePerSecond = 0.1f;
		}

		public new class Instance : GameInstance
		{
			public float originalSpeed;
			public Navigator navigator;
			public AmountInstance moisture;
			public Health health;
			private static readonly Color32 dryColorDiff = new(45, 45, 45, 0);
			private KBatchedAnimController kbac;

			public void ApplySadLook()
			{
				kbac.TintColour = new Color32()
				{
					r = (byte)Mathf.Clamp((kbac.TintColour.r - dryColorDiff.r), 0, byte.MaxValue),
					g = (byte)Mathf.Clamp((kbac.TintColour.g - dryColorDiff.g), 0, byte.MaxValue),
					b = (byte)Mathf.Clamp((kbac.TintColour.b - dryColorDiff.b), 0, byte.MaxValue),
					a = kbac.TintColour.a
				};
			}

			public void RemoveSadLook()
			{
				kbac.TintColour = new Color32()
				{
					r = (byte)Mathf.Clamp((kbac.TintColour.r + dryColorDiff.r), 0, byte.MaxValue),
					g = (byte)Mathf.Clamp((kbac.TintColour.g + dryColorDiff.g), 0, byte.MaxValue),
					b = (byte)Mathf.Clamp((kbac.TintColour.b + dryColorDiff.b), 0, byte.MaxValue),
					a = kbac.TintColour.a
				};
			}

			public float GetEstimatedTimeUntilDeath()
			{
				return smi.IsInsideState(smi.sm.desiccating) ? health.hitPoints / def.desiccationDamagePerSecond : float.NaN;
			}

			public bool IsDesiccating() => smi.IsInsideState(smi.sm.desiccating);

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{

				moisture = BAmounts.Moisture.Lookup(gameObject);
				health = master.GetComponent<Health>();

				navigator = smi.GetComponent<Navigator>();
				kbac = master.GetComponent<KBatchedAnimController>();

				originalSpeed = navigator.defaultSpeed;
			}
		}
	}
}
