using TUNING;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class PlushieGifterChore : Chore<PlushieGifterChore.StatesInstance>, IWorkerPrioritizable
	{
		private Precondition HasBedCondition = new()
		{
			id = "Beached_Precondition_Hasbed",
			description = "Found a bed to place a plushie on.",
			fn = HasBed
		};

		private static bool HasBed(ref Precondition.Context context, object data)
		{
			Log.Debug("checking bed condition");

			var result = data is Chore<StatesInstance> chore && chore.smi.HasTargetCell();
			Log.Debug(result);
			return result;
		}

		public PlushieGifterChore(IStateMachineTarget target) : base(
			Db.Get().ChoreTypes.JoyReaction,
			target,
			target.GetComponent<ChoreProvider>(),
			false,
			master_priority_class: PriorityScreen.PriorityClass.high,
			report_type: ReportManager.ReportType.PersonalTime)
		{
			showAvailabilityInHoverText = false;
			smi = new StatesInstance(this, target.gameObject);

			AddPrecondition(HasBedCondition, this);
			AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
			AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
			AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
		}

		public bool GetWorkerPriority(WorkerBase worker, out int priority)
		{
			priority = RELAXATION.PRIORITY.TIER1;
			return true;
		}

		public class States : GameStateMachine<States, StatesInstance, PlushieGifterChore>
		{
			public TargetParameter artist;
			public IntParameter plushiesMade = new(0);
			public State goToBed;
			public State idle;
			public State creatingPlushie;
			public State success;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;
				Target(artist);

				root
					.Enter(smi => Log.Debug("PlusheGifterChore root"))
					.EventTransition(GameHashes.ScheduleBlocksChanged, idle, smi => !smi.IsRecTime());

				idle
					.Enter(smi => Log.Debug("PlusheGifterChore idle"))
					.UpdateTransition(goToBed, FindBed, UpdateRate.SIM_1000ms);
				//.EventTransition(GameHashes.NewBuilding, telepadInstance => GameplayEventManager.Instance, goToBed, FindBed);

				goToBed
					.UpdateTransition(idle, (smi, dt) => smi.targetBed == null)
					.EventHandlerTransition(GameHashes.QueueDestroyObject, smi => smi.targetBed, idle, (smi, data) => true)
					.MoveTo(smi => smi.GetTargetCell(), creatingPlushie);

				creatingPlushie
					.Enter(smi => Log.Debug("PlusheGifterChore creatingPlushie"))
					.PlayAnim("working_pre")
					.QueueAnim("working_loop")
					.QueueAnim("working_pst")
					.OnAnimQueueComplete(success);

				success
					.Enter(smi => Log.Debug("PlusheGifterChore success"))
					.Enter(smi =>
					{
						smi.PlacePlushie();
						smi.StopSM("completed");
					})
					.ReturnSuccess();
			}

			private bool FindBed(StatesInstance smi, float dt)
			{
				Log.Debug("looking for bed");
				foreach (var plushiePlaceable in ModCmps.plushiePlaceables.items)
				{
					Log.Debug($"\tbed: {plushiePlaceable.name}");
					if (IsBedEligible(plushiePlaceable, smi))
					{
						Log.Debug($"\tyes");
						smi.targetBed = plushiePlaceable;
						return true;
					}
					else
						Log.Debug($"\no");
				}

				return false;
			}

			private bool IsBedEligible(Beached_PlushiePlaceable bed, StatesInstance smi)
			{
				Log.Debug($"\t\thasPlushie: {bed.HasPlushie()}, isOp: {bed.GetComponent<Operational>().IsOperational} nav cost: {smi.navigator.GetNavigationCost(bed.NaturalBuildingCell())}");
				return !bed.HasPlushie()
					&& bed.GetComponent<Operational>().IsOperational
					&& smi.navigator.GetNavigationCost(bed.NaturalBuildingCell()) != -1;
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, PlushieGifterChore, object>.GameInstance
		{
			public Beached_PlushiePlaceable targetBed;
			public PlushPlacebleBedSensor bedSensor;
			public Navigator navigator;
			private GameObject plushieGifter;

			public StatesInstance(PlushieGifterChore master, GameObject plushieGifter) : base(master)
			{
				this.plushieGifter = plushieGifter;
				navigator = master.GetComponent<Navigator>();
				sm.artist.Set(plushieGifter, smi);
				bedSensor = GetComponent<Sensors>().GetSensor<PlushPlacebleBedSensor>();
			}

			public bool HasTargetCell() => bedSensor.placeable != null;

			public bool IsRecTime() => master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);

			public int GetTargetCell() => bedSensor.GetCell();

			public void PlacePlushie()
			{
				if (bedSensor.placeable != null)
					plushieGifter.GetSMI<PlushieGifter.Instance>().PlacePlushie(bedSensor.GetCell());

				bedSensor.Clear();
			}
		}
	}
}
