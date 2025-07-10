using Beached.Content.Defs.Flora.Gnawica;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	public class GnawicaGrowing : StateMachineComponent<GnawicaGrowing.StatesInstance>
	{
		[SerializeField] public int minLengthForMaw;

		[Serialize] private Ref<GnawicaMaw> _maw;

		public override void OnSpawn()
		{
			smi.StartSM();

			if (_maw != null)
			{
				var maw = _maw.Get();
				if (maw != null)
					smi.sm.mawTarget.Set(maw, smi);
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, GnawicaGrowing, object>.GameInstance
		{
			public GnawicaCore core;

			public StatesInstance(GnawicaGrowing master) : base(master)
			{
				core = master.GetComponent<GnawicaCore>();
			}
		}

		private void TryGrowMaw(GnawicaStalk stalk)
		{
			/*			var isConnectedTop = stalk.Connections.HasFlag(GnawicaStalk.Connection.Top);
						if (isConnectedTop)
							return;

						var isConnectedLeft = stalk.Connections.HasFlag(GnawicaStalk.Connection.Left);
						var isConnectedRight = stalk.Connections.HasFlag(GnawicaStalk.Connection.Right);

						if (isConnectedLeft != isConnectedRight)
						{*/

			var position = (stalk.transform.position + Vector3.left) with { z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack) - 0.05f };
			var go = FUtility.Utils.Spawn(GnawicaMawConfig.ID, stalk.transform.position + Vector3.left);
			if (go.TryGetComponent(out GnawicaMaw maw))
			{
				_maw = new Ref<GnawicaMaw>(maw);
				smi.sm.mawTarget.Set(maw, smi);
			}
			//}
		}

		public class States : GameStateMachine<States, StatesInstance, GnawicaGrowing>
		{
			public TargetParameter mawTarget;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = root;

				root
					.EventHandler(ModHashes.multiPartPlant_Joined, OnStalkGrown);
			}

			private void OnStalkGrown(StatesInstance smi, object data)
			{
				Log.Debug($"on stalk grown trigger {data is GnawicaStalk}");
				if (mawTarget.Get(smi) == null
					&& smi.core.Size() >= smi.master.minLengthForMaw
					&& data is GnawicaStalk stalk)
					smi.master.TryGrowMaw(stalk);
			}
		}
	}
}
