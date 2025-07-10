namespace Beached.Content.Scripts
{

	public class Beached_ElectricShockable : GameStateMachine<Beached_ElectricShockable, Beached_ElectricShockable.Instance, IStateMachineTarget, Beached_ElectricShockable.Def>
	{
		private State idle;
		private State shocked;
		private State shockedPst;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			idle
				.UpdateTransition(shocked, IsInShockedTile);

			shocked
				.Enter(Damage)
				.GoTo(shockedPst);

			shockedPst
				.ScheduleGoTo(0.5f, idle);
		}

		private void Damage(Instance smi)
		{
			smi.health.Damage(10f);
			Game.Instance.SpawnFX(SpawnFXHashes.ElectrobankDamage, Grid.PosToCell(smi), 0);
		}

		private bool IsInShockedTile(Instance smi, float _)
		{
			var treshold = 0.05f;
			var cell = Grid.PosToCell(smi);

			if (Grid.IsValidCell(cell) && Beached_Grid.Instance.electricity[cell] > treshold)
				return true;

			var cellBelow = Grid.CellBelow(cell);
			return Grid.IsValidCell(cellBelow) && Beached_Grid.Instance.electricity[cellBelow] > treshold;
		}

		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Health health;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				health = master.GetComponent<Health>();
			}
		}
	}
}
