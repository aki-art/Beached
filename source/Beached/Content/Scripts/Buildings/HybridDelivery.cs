using Beached.Content.ModDb;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class HybridDelivery : StateMachineComponent<HybridDelivery.StatesInstance>
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private ConduitConsumer consumer;
		[MyCmpReq] private ManualDeliveryKG manualDelivery;

		[SerializeField] public float consumedPerSecond;
		[SerializeField] public Tag consumedTag;
		[SerializeField] public Storage storage;
		[Serialize] public bool forcePaused;

		private static readonly Operational.Flag requiresFuelFlag = new("Beached_HasMaterial", Operational.Flag.Type.Requirement);

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();

			Subscribe((int)GameHashes.ConduitConnectionChanged, OnConduitConnectionChanged);

			Pause(forcePaused);
		}

		public void Pause(bool paused)
		{
			if (forcePaused == paused)
				return;

			forcePaused = paused;

			if (paused)
			{
				smi.GoTo(smi.sm.paused);
				return;
			}

			if (operational.IsFunctional && HasEnoughMassToStartConverting())
			{
				smi.GoTo(smi.sm.on);
				return;
			}

			smi.GoTo(smi.sm.off);
		}

		private bool IsManuallyPaused() => manualDelivery.userPaused;

		private void OnConduitConnectionChanged(object _)
		{
			if (IsManuallyPaused() || forcePaused)
			{
				return;
			}

			manualDelivery.Pause(consumer.IsConnected, "Conduit Connection");
		}

		private bool HasEnoughMassToStartConverting()
		{
			var availableMass = storage.GetMassAvailable(consumedTag);
			return availableMass >= consumedPerSecond;
		}

		public class States : GameStateMachine<States, StatesInstance, HybridDelivery>
		{
			public State disabled;
			public State off;
			public State on;
			public State paused;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = disabled;

				disabled
					.Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
					.EventTransition(GameHashes.FunctionalChanged, off, smi => smi.master.operational.IsFunctional && !smi.master.forcePaused);

				off
					.EventTransition(GameHashes.FunctionalChanged, disabled, smi => !smi.master.operational.IsFunctional)
					.Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, false))
					.ToggleStatusItem(BStatusItems.awaitingMaterial, smi => smi.master)
					.EventTransition(GameHashes.OnStorageChange, on, smi => smi.master.HasEnoughMassToStartConverting());
				on
					.EventTransition(GameHashes.FunctionalChanged, disabled, smi => !smi.master.operational.IsFunctional)
					.Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, true))
					.EventTransition(GameHashes.OnStorageChange, off, smi => !smi.master.HasEnoughMassToStartConverting());

				paused
					.Enter(smi => smi.master.operational.SetFlag(requiresFuelFlag, false));
			}
		}

		public class StatesInstance(HybridDelivery master) : GameStateMachine<States, StatesInstance, HybridDelivery, object>.GameInstance(master)
		{
		}
	}
}
