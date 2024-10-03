namespace Beached.Content.Scripts.Entities.AI
{
	public class MinedStates : GameStateMachine<MinedStates, MinedStates.Instance, IStateMachineTarget, MinedStates.Def>
	{
		public State cowering;
		public State behaviourcomplete;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = cowering;

			cowering
				.ToggleStatusItem("MinedStates - cowering", "")
				.PlayAnim("hiding")
				.EventTransition(ModHashes.critterMined, behaviourcomplete)
				.TagTransition(BTags.Creatures.beingMined, behaviourcomplete, true);

			behaviourcomplete
				.ToggleStatusItem("MinedStates - behaviourcomplete", "")
				.BehaviourComplete(BTags.Creatures.beingMined);
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, BTags.Creatures.beingMined);
			}
		}
	}
}
