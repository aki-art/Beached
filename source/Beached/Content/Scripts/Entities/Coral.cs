using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	[DefaultExecutionOrder(0)]
	public class Coral : StateMachineComponent<Coral.StatesInstance>
	{
		[MyCmpReq] private ElementConsumer elementConsumer;
		[MyCmpReq] private ElementConverter elementConverter;
		[MyCmpReq] private WiltCondition wiltCondition;

		[SerializeField] public Tag emitTag;
		[SerializeField] public Tag filter;
		[SerializeField] public SpawnFXHashes spawnFX;
		[SerializeField] public float emitMass;
		[SerializeField] public Vector3 emitOffset = Vector3.zero;
		[SerializeField] public Vector2 initialVelocity;
		public float consumptionRate;

		[MyCmpReq] private ReceptacleMonitor receptacleMonitor;

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		protected void DestroySelf()
		{
			CreatureHelpers.DeselectCreature(gameObject);
			Util.KDestroyGameObject(gameObject);
		}

		public override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.PlanterStorage, OnReplanted);
		}

		private void OnReplanted(object _)
		{
			RefreshConsumptionRate();
		}

		public void RefreshConsumptionRate()
		{
			elementConsumer.consumptionRate = receptacleMonitor.Replanted
				? consumptionRate * 4f
				: consumptionRate;

			elementConverter.OutputMultiplier = receptacleMonitor.Replanted ? 4f : 1f;
		}

		public class States : GameStateMachine<States, StatesInstance, Coral>
		{
			public AliveStates alive;
			public State grow;
			public State dead;
			public State test;
			public State blocked;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = grow;

				dead
					.Enter(Die);

				blocked
					.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked)
					.EventTransition(GameHashes.EntombedChanged, alive, (smi => alive.ForceUpdateStatus(smi.master.gameObject)))
					.EventTransition(GameHashes.TooColdWarning, alive, (smi => alive.ForceUpdateStatus(smi.master.gameObject)))
					.EventTransition(GameHashes.TooHotWarning, alive, (smi => alive.ForceUpdateStatus(smi.master.gameObject)))
					.TagTransition(GameTags.Uprooted, dead);

				grow
					.Enter((smi =>
					{
						if (smi.master.receptacleMonitor.HasReceptacle() && !alive.ForceUpdateStatus(smi.master.gameObject))
							smi.GoTo(blocked);
					}))
					.PlayAnim("grow", KAnim.PlayMode.Once)
					.EventTransition(GameHashes.AnimQueueComplete, alive.mature);

				alive
					.InitializeStates(masterTarget, dead)
					.TagTransition(GameTags.Uprooted, dead)
					.DefaultState(alive.mature);

				alive.mature
					.EventTransition(GameHashes.Wilt, alive.wilting, (smi => smi.master.wiltCondition.IsWilting()))
					.PlayAnim("idle_grown", KAnim.PlayMode.Loop)
					.Update(UpdateConsumers)
					.Update(UpdateEmission, UpdateRate.SIM_1000ms)
					.EventHandler(GameHashes.OnStorageChange, OnStorageChanged)
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(CanConsume(smi)))
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false));

				alive.wilting
					.PlayAnim("wilt")
					.EventTransition(GameHashes.WiltRecover, alive.mature, (smi => !smi.master.wiltCondition.IsWilting()));
			}

			private static bool CanConsume(StatesInstance smi)
			{
				if (smi.master.filter == Tag.Invalid)
					return true;

				var cell = Grid.PosToCell(smi);
				return Grid.IsValidCell(cell) && Grid.Element[cell].HasTag(smi.master.filter);
			}

			private void UpdateConsumers(StatesInstance smi, float _)
			{
				if (smi.master.filter == Tag.Invalid)
					return;

				var consumerEnabled = smi.master.elementConsumer.consumptionEnabled;
				var shouldConsume = CanConsume(smi);

				if (consumerEnabled != shouldConsume)
					smi.master.elementConsumer.EnableConsumption(shouldConsume);
			}

			public class AliveStates : PlantAliveSubState
			{
				public State mature;
				public State wilting;
			}

			private void OnStorageChanged(StatesInstance smi)
			{
				smi.dirty = true;
			}

			private void Die(StatesInstance smi)
			{
				Log.Debug("Dying");
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
				smi.master.Trigger((int)GameHashes.Died);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, data => smi.master.DestroySelf());
			}

			private void UpdateEmission(StatesInstance smi, float dt)
			{
				if (smi.dirty)
				{
					var emitMass = smi.master.emitMass;
					var emitElement = smi.storage.FindFirst(smi.master.emitTag);

					if (emitElement == null)
					{
						return;
					}

					var primaryElement = emitElement.GetComponent<PrimaryElement>();
					if (primaryElement.Mass >= smi.master.emitMass)
					{
						primaryElement.Mass -= emitMass;
						BubbleManager.instance.SpawnBubble(smi.transform.position, smi.master.initialVelocity, primaryElement.ElementID, emitMass, primaryElement.Temperature);
						if (smi.master.spawnFX != SpawnFXHashes.None)
							Game.Instance.SpawnFX(smi.master.spawnFX, smi.transform.position, 0);
					}

					smi.dirty = false;
				}
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Coral, object>.GameInstance
		{
			public bool dirty;
			public Storage storage;

			public StatesInstance(Coral master) : base(master)
			{
				dirty = true;
				storage = master.GetComponent<Storage>();
			}
		}
	}
}
