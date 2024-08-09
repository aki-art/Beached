using Beached.Content.ModDb;
using KSerialization;

namespace Beached.Content.Scripts.Entities.AI
{
	public class PlushieGifter : GameStateMachine<PlushieGifter, PlushieGifter.Instance>
	{
		public IntParameter plushiesCreated;
		public State neutral;
		public OverjoyedStates overjoyed;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = neutral;

			root
				.TagTransition(GameTags.Dead, null);

			neutral
				.TagTransition(GameTags.Overjoyed, overjoyed);

			overjoyed
				.TagTransition(GameTags.Overjoyed, neutral, true)
				.DefaultState(overjoyed.idle)
				.ParamTransition(plushiesCreated, overjoyed.exitEarly, HasGivenOutEnoughPlushies)
				.Exit(ResetNumPlushies);

			overjoyed.idle
				.EnterTransition(overjoyed.placingPlushie, IsRecTime)
				.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning)
				.EventTransition(GameHashes.ScheduleBlocksChanged, overjoyed.placingPlushie, IsRecTime);

			overjoyed.placingPlushie
				.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut)
				.EventTransition(GameHashes.ScheduleBlocksChanged, overjoyed.idle, Not(IsRecTime))
				.ToggleChore(smi => new PlushieGifterChore(smi.master), overjoyed.idle);

			overjoyed.exitEarly
				.Enter(ExitJoyReactionEarly);
		}

		private bool HasGivenOutEnoughPlushies(Instance smi, int num) => num >= 4;

		private void ResetNumPlushies(Instance smi)
		{
			smi.numPlushiesCreated = 0;
			plushiesCreated.Set(0, smi);
		}

		public void ExitJoyReactionEarly(Instance smi)
		{
			var joyBehaviourMonitor = smi.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			joyBehaviourMonitor.sm.exitEarly.Trigger(joyBehaviourMonitor);
		}

		public bool IsRecTime(Instance smi) => smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);

		public class OverjoyedStates : State
		{
			public State idle;
			public State placingPlushie;
			public State placePlushie;
			public State exitEarly;
		}

		public new class Instance : GameInstance
		{
			[Serialize] public int numPlushiesCreated;

			public Instance(IStateMachineTarget master) : base(master)
			{
				Beached.Log.Debug("new plushie gifter instance");
			}

			public void PlacePlushie(int cell)
			{
				if (Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(cell, out var gameObject))
				{
					if (gameObject.TryGetComponent(out Beached_PlushiePlaceable plushiePlaceable))
					{
						var plushie = BDb.plushies.resources.GetRandom().Id;
						plushiePlaceable.StorePlushie(plushie);
					}
				}
			}
		}
	}
}
