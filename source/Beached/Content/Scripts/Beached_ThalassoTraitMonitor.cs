using Klei.AI;
using ProcGen;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	public class Beached_ThalassoTraitMonitor : GameStateMachine<Beached_ThalassoTraitMonitor, Beached_ThalassoTraitMonitor.Instance, IStateMachineTarget, Beached_ThalassoTraitMonitor.Def>
	{
		public State idle;
		public State inSea;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;

			idle
				.EnterTransition(inSea, IsInSea)
				.EventHandlerTransition(ModHashes.enteredZoneType, inSea, HasEnteredSeaZoneType);

			inSea
				.ToggleEffect(smi => smi.def.effectId)
				.EventHandlerTransition(ModHashes.enteredZoneType, idle, (smi, data) => !HasEnteredSeaZoneType(smi, data));
		}

		private bool HasEnteredSeaZoneType(Instance smi, object data)
		{
			return (data is SubWorld.ZoneType zoneType) && IsInZoneType(smi, zoneType);
		}

		private bool IsInZoneType(Instance smi, SubWorld.ZoneType zoneType)
		{
			return smi.def.invert != smi.def.seaBiomes.Contains(zoneType);
		}

		private bool IsInSea(Instance smi)
		{
			var cell = Grid.PosToCell(smi);
			var zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(cell);

			return IsInZoneType(smi, zoneType);
		}

		public class Def : BaseDef
		{
			public HashSet<SubWorld.ZoneType> seaBiomes;
			public string effectId;
			public bool invert;
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
