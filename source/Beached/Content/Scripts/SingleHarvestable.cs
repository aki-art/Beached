using UnityEngine;

namespace Beached.Content.Scripts
{
	public class SingleHarvestable : StateMachineComponent<SingleHarvestable.StatesInstance>
	{
		[MyCmpReq] public Harvestable harvestable;
		[MyCmpGet] public SeedProducer seedProducer;
		[MyCmpReq] public KBatchedAnimController animController;

		[SerializeField] public string deathFx;
		[SerializeField] public string soundFx;
		[SerializeField] public float volume;

		public SingleHarvestable()
		{
			volume = 1.0f;
		}

		public class StatesInstance(SingleHarvestable smi) : GameStateMachine<States, StatesInstance, SingleHarvestable, object>.GameInstance(smi)
		{
		}

		public class States : GameStateMachine<States, StatesInstance, SingleHarvestable>
		{
			public State seed_grow;
			public AliveStates alive;
			public State dead;

			public class AliveStates : PlantAliveSubState
			{
				public State idle;
				public State harvest;
			}

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = seed_grow;
				serializable = SerializeType.Both_DEPRECATED;

				seed_grow
					.PlayAnim("idle", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, alive.idle);

				alive
					.InitializeStates(masterTarget, dead);

				alive.idle
					.PlayAnim("idle")
					.EventTransition(GameHashes.Harvest, alive.harvest)
					.Enter(smi => smi.master.harvestable.SetCanBeHarvested(true));

				alive.harvest
					.PlayAnim("harvest")
					.EventHandler(GameHashes.AnimQueueComplete, smi => Log.Debug("anim queue completed"))
					.OnAnimQueueComplete(dead)
					.Exit(smi => smi.master.seedProducer.DropSeed());

				dead
					.Enter(Die);
			}

			private static void Die(StatesInstance smi)
			{
				smi.master.PlayDeathEffect();
				smi.master.Trigger((int)GameHashes.Died);
				smi.master.animController.StopAndClear();
				Destroy(smi.master.animController);
				smi.master.DestroySelf();
			}
		}

		private void PlayDeathEffect()
		{
			if (!deathFx.IsNullOrWhiteSpace())
				GameUtil.KInstantiate(
						Assets.GetPrefab(deathFx),
						smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront)
						.SetActive(true);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public void DestroySelf()
		{
			CreatureHelpers.DeselectCreature(gameObject);
			Util.KDestroyGameObject(gameObject);
		}
	}
}
