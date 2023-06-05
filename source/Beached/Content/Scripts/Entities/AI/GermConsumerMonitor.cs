using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	// very similar to GasAndLiquidMonitor
	public class GermConsumerMonitor : GameStateMachine<GermConsumerMonitor, GermConsumerMonitor.Instance, IStateMachineTarget, GermConsumerMonitor.Def>
	{
		private State cooldown;
		private State satisfied;
		private State looking;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = cooldown;

			cooldown
				.Enter("ClearTargetCell", smi => smi.ClearTargetCell())
				.ScheduleGoTo(smi => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), satisfied);

			satisfied
				.Enter("ClearTargetCell", smi => smi.ClearTargetCell())
				.TagTransition(smi => smi.def.transitionTag, looking);

			looking
				.ToggleBehaviour(smi => smi.def.behaviourTag, smi => smi.targetCell != -1, smi => smi.GoTo(cooldown))
				.TagTransition(smi => smi.def.transitionTag, satisfied, true)
				.Update("FindGermyTile", (smi, dt) => smi.FindElement(), UpdateRate.SIM_1000ms);
		}

		public class Def : BaseDef
		{
			public Tag[] transitionTag = { GameTags.Creatures.Hungry };
			public Tag behaviourTag = GameTags.Creatures.WantsToEat;
			public float minCooldown = 5f;
			public float maxCooldown = 5f;
			public float consumptionRate = 100_000f;
			public Diet diet;
			public byte consumableGermIdx;
		}

		public new class Instance : GameInstance
		{
			public int targetCell = -1;
			public string germTag;
			private Navigator navigator;
			private int massUnavailableFrameCount;
			private List<Klei.AI.Disease> diseases;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				navigator = smi.GetComponent<Navigator>();

				if (def.consumableGermIdx == byte.MaxValue)
				{
					Beached.Log.Warning($"{def.consumableGermIdx} is not a valid germ id");
					return;
				}

				diseases = Db.Get().Diseases.resources;
				germTag = diseases[def.consumableGermIdx].Id;

				if (smi.def.diet == null)
				{
					Beached.Log.Warning($"{nameof(GermConsumerMonitor)} of {master.name} needs a diet.");
				}
			}

			public void ClearTargetCell()
			{
				targetCell = -1;
				massUnavailableFrameCount = 0;
			}

			public void FindElement()
			{
				targetCell = -1;
				FindTargetCell();
			}

			public bool IsConsumableCell(int cell)
			{
				var diseaseIdx = Grid.DiseaseIdx[cell];

				if (diseaseIdx == byte.MaxValue || diseaseIdx > diseases.Count)
				{
					return false;
				}

				if (smi.def.diet != null)
				{
					foreach (var info in smi.def.diet.infos)
					{
						if (info.IsMatch(diseases[diseaseIdx].Id))
						{
							return true;
						}
					}
				}

				return false;
			}

			public void FindTargetCell()
			{
				var query = new ConsumableCellQuery(smi, 25);
				navigator.RunQuery(query);
				if (!query.success)
				{
					return;
				}

				targetCell = query.GetResultCell();
			}

			public void Consume(float dt)
			{
				/*                var index = Game.Instance.diseaseConsumptionCallbackManager.Add(
									OnDiseaseConsumed,
									this,
									nameof(GermConsumerMonitor)).index;*/

				/*                SimMessages.ConsumeDisease(
                                    Grid.PosToCell(this),
                                    100f,
                                    (int)(def.consumptionRate * dt),
                                    index);*/

				var cell = Grid.PosToCell(this);
				var diseaseCount = Grid.DiseaseCount[cell];
				var diseaseDelta = Mathf.Min(diseaseCount, (int)(def.consumptionRate * dt));

				if (diseaseDelta > 0)
				{
					SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), def.consumableGermIdx, -diseaseDelta);
					OnDiseaseConsumed(diseaseDelta);
					Beached.Log.Debug($"consumed {diseaseDelta} germs");
				}
			}

			private void OnDiseaseConsumed(int diseaseCount)
			{
				if (!IsRunning())
				{
					return;
				}

				if (diseaseCount > 0)
				{
					if (def.diet != null)
					{
						massUnavailableFrameCount = 0;
						var dietInfo = def.diet.GetDietInfo(germTag);

						if (dietInfo == null)
						{
							Beached.Log.Debug("no dietInfo");
							return;
						}

						var calories = dietInfo.ConvertConsumptionMassToCalories(diseaseCount);
						Beached.Log.Debug($"finished consuming, result: {calories} calories");
						Trigger((int)GameHashes.CaloriesConsumed, new CreatureCalorieMonitor.CaloriesConsumedEvent()
						{
							tag = germTag,
							calories = calories
						});
					}
				}
				else
				{
					++massUnavailableFrameCount;

					if (massUnavailableFrameCount < 2)
					{
						return;
					}

					Trigger((int)GameHashes.ElementNoLongerAvailable);
				}
			}

			public class ConsumableCellQuery : PathFinderQuery
			{
				public bool success;
				private Instance smi;
				private int maxIterations;

				public ConsumableCellQuery(Instance smi, int maxIterations)
				{
					this.smi = smi;
					this.maxIterations = maxIterations;
				}

				public override bool IsMatch(int cell, int parent_cell, int cost)
				{
					success = smi.IsConsumableCell(cell);
					return success || --maxIterations <= 0;
				}
			}
		}
	}
}
