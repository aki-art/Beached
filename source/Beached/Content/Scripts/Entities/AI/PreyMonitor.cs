using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class PreyMonitor : GameStateMachine<PreyMonitor, PreyMonitor.Instance, IStateMachineTarget, PreyMonitor.Def>
	{
		public BoolParameter hasEggToGuard;
		public HuntingStates hunt;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = hunt.safe;

			root
				.EventHandler(GameHashes.ObjectDestroyed, (smi, d) => smi.Cleanup(d));

			hunt.safe
				.Enter(smi => smi.RefreshThreat(null))
				.Update((smi, dt) => smi.RefreshThreat(null), load_balance: true);

			hunt.hunting
				.ToggleBehaviour(BTags.Creatures.hunting, smi => smi.mainThreat != null, smi => smi.GoTo(hunt.cooldown))
				.Update(CritterUpdateThreats);

			hunt.cooldown
				.Update((smi, dt) => smi.RefreshThreat(null), load_balance: true)
				.ScheduleGoTo(smi => smi.huntCooldown, hunt.safe);
		}

		private static void CritterUpdateThreats(Instance smi, float dt)
		{
			if (smi.isMasterNull || smi.CheckForThreats())
				return;

			smi.GoTo(smi.sm.hunt.safe);
		}

		public class Def : BaseDef
		{
			public Tag[] allyTags;
			public string animPrefix;
		}

		public class HuntingStates :
		  State
		{
			public State safe;
			public State hunting;
			public State cooldown;
		}

		public new class Instance : GameInstance
		{
			public FactionAlignment alignment;
			private readonly Navigator navigator;
			public GameObject mainThreat;
			private List<FactionAlignment> threats = new();
			private int maxThreatDistance = 12;
			private CollarWearer collarWearer;
			public float huntCooldown = 20f;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				alignment = master.GetComponent<FactionAlignment>();
				navigator = master.GetComponent<Navigator>();
				collarWearer = master.GetComponent<CollarWearer>();
			}

			public bool CheckForThreats()
			{
				var threat = FindThreat();
				SetMainThreat(threat);

				return threat != null;
			}

			public void SetMainThreat(GameObject threat)
			{
				if (threat == mainThreat)
					return;

				if (mainThreat != null)
				{
					mainThreat.Unsubscribe((int)GameHashes.Died, RefreshThreat);
					mainThreat.Unsubscribe((int)GameHashes.ObjectDestroyed, RefreshThreat);

					if (threat == null)
						Trigger((int)GameHashes.TargetLost);
				}

				if (mainThreat != null)
				{
					mainThreat.Unsubscribe((int)GameHashes.Died, RefreshThreat);
					mainThreat.Unsubscribe((int)GameHashes.ObjectDestroyed, RefreshThreat);
				}

				mainThreat = threat;

				if (mainThreat != null)
				{
					mainThreat.Subscribe((int)GameHashes.Died, RefreshThreat);
					mainThreat.Subscribe((int)GameHashes.ObjectDestroyed, RefreshThreat);
				}
			}

			public GameObject FindThreat()
			{
				threats.Clear();

				var gathered_entries = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
				GameScenePartitioner.Instance.GatherEntries(new Extents(Grid.PosToCell(this), maxThreatDistance), GameScenePartitioner.Instance.attackableEntitiesLayer, gathered_entries);

				for (int index = 0; index < gathered_entries.Count; ++index)
				{
					var factionAlignment = gathered_entries[index].obj as FactionAlignment;

					if (!(factionAlignment.transform == null) && !(factionAlignment == alignment) && factionAlignment.IsAlignmentActive() && navigator.CanReach(factionAlignment.attackable))
					{
						var isAlly = false;

						if (!collarWearer.IsAllowedToKill(factionAlignment.GetComponent<KPrefabID>()))
							continue;

						foreach (var allyTag in def.allyTags)
						{
							if (factionAlignment.HasTag(allyTag))
								isAlly = true;
						}

						if (!isAlly)
							threats.Add(factionAlignment);
					}
				}

				gathered_entries.Recycle();

				return PickClosestTarget(threats);
			}

			public void Cleanup(object data)
			{
				if (mainThreat != null)
				{
					mainThreat.Unsubscribe((int)GameHashes.Died, RefreshThreat);
					mainThreat.Unsubscribe((int)GameHashes.ObjectDestroyed, RefreshThreat);
				}
			}

			public void GoToThreatened() => smi.GoTo(sm.hunt.hunting);

			public GameObject PickClosestTarget(List<FactionAlignment> threats)
			{
				var position = (Vector2)this.gameObject.transform.GetPosition();
				GameObject gameObject = null;

				var minDistance = float.PositiveInfinity;

				for (int index = threats.Count - 1; index >= 0; --index)
				{
					var threat = threats[index];
					var distance = Vector2.Distance(position, (Vector2)threat.transform.GetPosition());

					if (distance < minDistance)
					{
						minDistance = distance;
						gameObject = threat.gameObject;
					}
				}

				return gameObject;
			}

			public void RefreshThreat(object data)
			{
				if (!IsRunning())
					return;

				if (smi.CheckForThreats())
				{
					GoToThreatened();
					return;
				}

				if (smi.GetCurrentState() == sm.hunt.safe)
					return;

				Trigger((int)GameHashes.SafeFromThreats);

				smi.GoTo(sm.hunt.safe);
			}
		}
	}
}
