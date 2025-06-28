using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	public class Oxybloon : StateMachineComponent<Oxybloon.StatesInstance>
	{
		[MyCmpReq] public Harvestable harvestable;
		[MyCmpReq] public Storage storage;
		[MyCmpReq] public KBatchedAnimController animController;

		[SerializeField] public string deathFx;
		[SerializeField] public string soundFx;
		[SerializeField] public float volume;
		[SerializeField] public SimHashes elementId;

		[Serialize] public bool hasFilledUp;

		public Oxybloon()
		{
			volume = 1.0f;
			elementId = SimHashes.Oxygen;
		}

		public class StatesInstance(Oxybloon smi) : GameStateMachine<States, StatesInstance, Oxybloon, object>.GameInstance(smi)
		{
		}

		public class States : GameStateMachine<States, StatesInstance, Oxybloon>
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
					.OnAnimQueueComplete(dead)
					.Exit(smi => smi.master.ReleaseOxygen());

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

		private void ReleaseOxygen()
		{
			var oxygenMass = storage.GetMassAvailable(elementId.CreateTag());
			storage.DropSome(elementId.CreateTag(), oxygenMass, true, showInWorldNotification: true);
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
			if (!hasFilledUp && smi.master.storage.IsEmpty())
			{
				var element = ElementLoader.FindElementByHash(smi.master.elementId);

				var material = element.substance.SpawnResource(
					smi.transform.position,
					100f,
					element.defaultValues.temperature,
					byte.MaxValue,
					0,
					true);

				smi.master.storage.Store(material, true, true);
				hasFilledUp = true;
			}
		}

		public void DestroySelf()
		{
			CreatureHelpers.DeselectCreature(gameObject);
			Util.KDestroyGameObject(gameObject);
		}
	}
}
