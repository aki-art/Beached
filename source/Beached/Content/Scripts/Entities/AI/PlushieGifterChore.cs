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
			return data is Chore<StatesInstance> chore && chore.smi.HasTargetCell();
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

		public bool GetWorkerPriority(Worker worker, out int priority)
		{
			priority = RELAXATION.PRIORITY.TIER1;
			return true;
		}

		public class States : GameStateMachine<States, StatesInstance, PlushieGifterChore>
		{
			public TargetParameter artist;
			public IntParameter plushiesMade = new IntParameter(0);
			public State goToBed;
			public State idle;
			public State creatingPlushie;
			public State success;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = goToBed;
				Target(artist);

				root
					.EventTransition(GameHashes.ScheduleBlocksChanged, idle, smi => !smi.IsRecTime());

				idle
					.DoNothing();

				goToBed
					 .Transition(null, smi => !smi.HasTargetCell())
					 .MoveTo(smi => smi.GetTargetCell(), creatingPlushie);

				creatingPlushie
					.PlayAnim("working_pre")
					.QueueAnim("working_loop")
					.QueueAnim("working_pst")
					.OnAnimQueueComplete(success);

				success
					.Enter(smi =>
					{
						smi.PlacePlushie();
						smi.StopSM("completed");
					})
					.ReturnSuccess();
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, PlushieGifterChore, object>.GameInstance
		{
			private PlushPlacebleBedSensor sensor;
			private GameObject plushieGifter;

			public StatesInstance(PlushieGifterChore master, GameObject plushieGifter) : base(master)
			{
				this.plushieGifter = plushieGifter;
				sm.artist.Set(plushieGifter, smi);
			}

			public bool HasTargetCell()
			{
				if (sensor == null)
					sensor = GetComponent<Sensors>().GetSensor<PlushPlacebleBedSensor>();

				return GetTargetCell() != Grid.InvalidCell;
			}

			public bool IsRecTime() => master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);

			public int GetTargetCell() => sensor.cell;

			public void PlacePlushie()
			{
				plushieGifter.GetSMI<PlushieGifter.Instance>().PlacePlushie(sensor.cell);
			}
		}
	}
}
