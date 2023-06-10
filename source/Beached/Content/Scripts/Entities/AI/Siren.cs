using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class Siren : GameStateMachine<Siren, Siren.Instance, IStateMachineTarget>
	{
		private State idle;
		private State angry;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			root
				.TagTransition(GameTags.Dead, null);

			idle
				.EventTransition(GameHashes.StressedHadEnough, angry);

			angry
				.Enter(BeAngry)
				.Exit(StopBeingAngry)
				.EventTransition(GameHashes.NotStressed, idle)
				.ToggleStatusItem("Angry", "");
		}

		private void BeAngry(Instance smi)
		{
			TintPurple(smi);
			smi.CreatePasserbyReactable();
		}

		private void StopBeingAngry(Instance smi)
		{
			ResetTint(smi);
			smi.ClearPasserbyReactable();
		}

		private void TintPurple(Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().TintColour = new Color(1, 0, 1);
		}

		private void ResetTint(Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().TintColour = Color.white;
		}

		public new class Instance : GameInstance
		{
			private Reactable passerbyReactable;

			public Instance(IStateMachineTarget master) : base(master) { }

			public void CreatePasserbyReactable()
			{
				Beached.Log.Debug("Creating reactable");

				if (passerbyReactable != null)
					return;

				var emoteReactable = new EmoteReactable(
					gameObject,
					"Beached_SirenPasseryByReactable",
					Db.Get().ChoreTypes.EmoteHighPriority,
					5,
					5,
					localCooldown: Mod.debugMode ? 60f : CONSTS.CYCLE_LENGTH);

				emoteReactable
					.SetEmote(BEmotes.scared)
					.SetThought(BThoughts.scared)
					.AddPrecondition(ReactorIsOnFloor);

				emoteReactable
					.RegisterEmoteStepCallbacks("floor_floor_1_0_pst", AddReactionEffect, null);

				passerbyReactable = emoteReactable;

				Beached.Log.Debug("Created reactable");
			}

			private void AddReactionEffect(GameObject reactor) => reactor.GetComponent<Effects>().Add(BEffects.SCARED_SIREN, true);

			private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition) => transition.end == NavType.Floor;

			public void ClearPasserbyReactable()
			{
				if (passerbyReactable == null)
					return;

				passerbyReactable.Cleanup();
				passerbyReactable = null;
			}
		}
	}
}
