using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{

	public class SittableEggsMonitor : GameStateMachine<SittableEggsMonitor, SittableEggsMonitor.Instance, IStateMachineTarget, SittableEggsMonitor.Def>
	{
		public State idle;
		public State targeting;
		public State cooldown;
		public BoolParameter hasTarget;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			root
				.EventHandler(GameHashes.ObjectDestroyed, (smi, d) => smi.Cleanup(d));

			idle
				.Enter(smi => smi.RefreshTarget(null))
				.Update((smi, dt) => smi.RefreshTarget(null), load_balance: true);

			targeting
				.ToggleBehaviour(BTags.Creatures.wantsToSitOnEgg, smi => smi.target != null, smi => smi.GoTo(cooldown))
				.Update(CritterUpdateTargets);

			cooldown
				//.Update((smi, dt) => smi.RefreshTarget(null), load_balance: true)
				.ScheduleGoTo(smi => smi.cooldown, idle);
		}


		private static void CritterUpdateTargets(Instance smi, float dt)
		{
			if (smi.isMasterNull || smi.CheckForTarget())
				return;

			smi.GoTo(smi.sm.idle);
		}

		public class Def : BaseDef
		{
			public int maxSearchCost = 30;
		}

		public new class Instance : GameInstance
		{
			private readonly Navigator navigator;
			public GameObject target;
			public float cooldown = 30f;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				navigator = master.GetComponent<Navigator>();
			}

			public GameObject FindTarget()
			{
				var cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(gameObject));

				if (cavityForCell == null)
					return null;

				var closestDistance = def.maxSearchCost;
				GameObject result = null;

				foreach (var egg in cavityForCell.eggs)
				{
					if (!egg.HasTag(GameTags.Creatures.ReservedByCreature) && !egg.HasTag(GameTags.Stored))
					{
						var cell = Grid.PosToCell(egg);
						var navigationCost = navigator.GetNavigationCost(cell);
						if (navigationCost != -1 && navigationCost < closestDistance)
						{
							result = egg.gameObject;
							closestDistance = navigationCost;
						}
					}
				}

				return result;
			}

			public bool CheckForTarget()
			{
				// check if our existing target is still around
				if (this.target != null)
				{
					var cell = Grid.PosToCell(this.target);
					var navigationCost = navigator.GetNavigationCost(cell);

					if (navigationCost != -1 && navigationCost < def.maxSearchCost)
						return true;
				}

				var target = FindTarget();
				SetNewTarget(target);

				return target != null;
			}

			public void RefreshTarget(object data)
			{
				if (!IsRunning())
					return;

				if (smi.CheckForTarget())
				{
					GoToTargeted();
					return;
				}

				if (smi.GetCurrentState() != sm.idle)
					smi.GoTo(sm.idle);
			}

			public void Cleanup(object _)
			{
				if (target != null)
					target.Unsubscribe((int)GameHashes.ObjectDestroyed, RefreshTarget);
			}

			public void GoToTargeted() => smi.GoTo(sm.targeting);

			public void SetNewTarget(GameObject newTarget)
			{
				if (newTarget == target)
					return;

				if (target != null)
				{
					target.Unsubscribe((int)GameHashes.ObjectDestroyed, RefreshTarget);

					if (newTarget == null)
						Trigger((int)GameHashes.TargetLost);

					target.RemoveTag(GameTags.Creatures.ReservedByCreature);
				}

				if (newTarget != null)
				{
					newTarget.Subscribe((int)GameHashes.ObjectDestroyed, RefreshTarget);
					newTarget.AddTag(GameTags.Creatures.ReservedByCreature);
				}

				target = newTarget;
			}
		}
	}
}
