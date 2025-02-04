using Beached.Content.BWorldGen;
using Beached.Content.ModDb;
using Klei.AI;

namespace Beached.Content.Scripts
{
	public class Beached_ScaredMonitor : GameStateMachine<Beached_ScaredMonitor, Beached_ScaredMonitor.Instance, IStateMachineTarget, Beached_ScaredMonitor.Def>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;

			root
				.Update(OnUpdate, UpdateRate.SIM_1000ms);
		}

		private void OnUpdate(Instance smi, float dt)
		{
			var cell = Grid.PosToCell(smi);

			if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == ZoneTypes.depths
				&& Grid.LightIntensity[cell] < smi.def.lightTreshold)
				smi.effects.Add(BEffects.SCARED, true);
		}

		public class Def : BaseDef
		{
			public int lightTreshold = 30;
		}

		public new class Instance : GameInstance
		{
			public Effects effects;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				effects = GetComponent<Effects>();
			}
		}
	}
}
