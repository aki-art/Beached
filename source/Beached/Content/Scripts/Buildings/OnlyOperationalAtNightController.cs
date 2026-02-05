namespace Beached.Content.Scripts.Buildings
{
	public class OnlyOperationalAtNightController : GameStateMachine<OnlyOperationalAtNightController, OnlyOperationalAtNightController.Instance>
	{
		public State night;
		public State day;

		public static Operational.Flag isNightFlag = new("Beached_IsNightFlag", Operational.Flag.Type.Requirement);

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = day;
			root
				.Enter(smi => smi.GetComponent<Operational>().SetFlag(isNightFlag, false));

			day
				.EventTransition(GameHashes.Nighttime, smi => GameClock.Instance, night)
				.EnterTransition(night, smi => GameClock.Instance.IsNighttime());

			night
				.ToggleOperationalFlag(isNightFlag)
				.PlayAnim("on")
				.EventTransition(GameHashes.NewDay, smi => GameClock.Instance, day)
				.ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight);
		}

		public class Def : BaseDef
		{
		}

		public new class Instance(IStateMachineTarget master, Def def) : GameInstance(master, def)
		{
		}
	}
}
