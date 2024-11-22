using Beached.Content.Defs.Entities.Critters.Jellies;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI.Strobila
{
	public class Strobila : StateMachineComponent<Strobila.StatesInstance>
	{
		[Serialize] public int ephyraStock;
		[SerializeField] public int maxEphyra;

		public override void OnSpawn()
		{
			smi.StartSM();
			ModCmps.jellyfishStrobilas.Add(this);
			smi.sm.ephyraRemaining.Set(ephyraStock, smi);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			ModCmps.jellyfishStrobilas.Remove(this);
		}

		public void SpawnEphyra()
		{
			FUtility.Utils.Spawn(BabyJellyfishConfig.ID, gameObject);
			ephyraStock--;

			if (ephyraStock <= 0)
				smi.sm.ephyraRemaining.Set(ephyraStock, smi);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Strobila, object>.GameInstance
		{
			public StatesInstance(Strobila master) : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, Strobila>
		{
			public State growing;
			public State emitting;
			public State dying;

			public IntParameter ephyraRemaining;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = growing;

				root
					.ParamTransition(ephyraRemaining, dying, (smi, num) => num <= 0);

				dying
					.PlayAnim("death")
					.Enter(smi => smi.GetComponent<KBatchedAnimController>().onAnimComplete += anim =>
					{
						Util.KDestroyGameObject(smi.gameObject);
					});
			}
		}
	}
}
