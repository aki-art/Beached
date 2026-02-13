using FMOD.Studio;
using ImGuiNET;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class Chime : StateMachineComponent<Chime.StatesInstance>, IImguiDebug
	{
		private const float STILL_THRESHOLD = 0.001f;
		private const float SLOW_THRESHOLD = 0.002f;
		private const float MEDIUM_THRESHOLD = 0.003f;
		private const float VOLUME_LERP_SPEED = 2f;

		public float flowTest;

		public static readonly HashedString pressureChangeParameter = "Beached_PressureChange";

		private EventInstance soundEvent;

		public override void OnSpawn() => smi.StartSM();

		private static float strength = 0f;
		public void OnImguiDraw()
		{
			ImGui.Text($"Chime Power: {Beached_Grid.GetFlow(smi.cell)}");

			if (ImGui.DragFloat("Chime", ref strength, 0.01f, 0f, 1f))
			{
				GetComponent<LoopingSounds>().UpdateFirstParameter("event:/beached/SFX/Chimes/Beached_Chime_Loop", "Beached_PressureChange", strength);
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, Chime, object>.GameInstance
		{
			public Operational operational;
			public LoopingSounds loopingSounds;
			public int cell;
			public float flowMin;
			public float flowMax;
			public float flow;
			public float targetVolume;
			public float currentVolume;
			public string chimeSound;
			public float volumeChangeSpeed = 0.1f;

			public StatesInstance(Chime master) : base(master)
			{
				chimeSound = GlobalAssets.GetSound("Beached_Chime_Loop");
				master.TryGetComponent(out operational);
				master.TryGetComponent(out loopingSounds);
				cell = Grid.CellBelow(Grid.PosToCell(master));
				flowMin = float.PositiveInfinity;
				flowMax = float.NegativeInfinity;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, Chime>
		{
			public State notInGas;
			public FlowState flowing;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = flowing;

				notInGas
					.EventHandlerTransition(GameHashes.OperationalChanged, flowing, IsOperational);

				flowing
					.DefaultState(flowing.still)
					//.Enter(telepadInstance => telepadInstance.audioPlayer.PlayLoop(ModAssets.Sounds.SHELL_CHIME_LOUD))
					//.Exit(telepadInstance => telepadInstance.audioPlayer.StopLooping())
					.Enter(StartSound)
					.Exit(StopSound)
					.Update(UpdateFlow, UpdateRate.SIM_200ms)
					.Update(UpdateVolume, UpdateRate.SIM_200ms)
					.EventHandlerTransition(GameHashes.OperationalChanged, flowing, (smi, _) => !IsOperational(smi, _));

				flowing.still
					.Enter(smi =>
					{
						smi.flowMin = float.NegativeInfinity;
						smi.flowMax = STILL_THRESHOLD;
					})
					.QueueAnim("off", true);

				flowing.mild
					.Enter(smi =>
					{
						smi.flowMin = STILL_THRESHOLD;
						smi.flowMax = SLOW_THRESHOLD;
					})
					.QueueAnim("mild", true);

				flowing.medium
					.Enter(smi =>
					{
						smi.flowMin = SLOW_THRESHOLD;
						smi.flowMax = MEDIUM_THRESHOLD;
					})
					.QueueAnim("medium", true);

				flowing.hard
					.Enter(smi =>
					{
						smi.flowMin = MEDIUM_THRESHOLD;
						smi.flowMax = float.PositiveInfinity;
					})
					.QueueAnim("hard", true);
			}

			private void StartSound(StatesInstance smi)
			{
				smi.loopingSounds.StartSound(smi.chimeSound);
				smi.currentVolume = 0;
			}

			private void StopSound(StatesInstance smi)
			{
				smi.loopingSounds.StopSound(smi.chimeSound);
			}

			private void UpdateVolume(StatesInstance smi, float dt)
			{
				smi.targetVolume = smi.flow;

				var strength = Mathf.Clamp01(Mathf.Lerp(smi.currentVolume, smi.targetVolume, dt * smi.volumeChangeSpeed) * 1f / 0.002f);


				smi.loopingSounds.UpdateFirstParameter("event:/beached/SFX/Chimes/Beached_Chime_Loop", pressureChangeParameter, strength * 0.01f);
			}

			private void UpdateFlow(StatesInstance smi, float _)
			{
				var flow = Beached_Grid.GetFlow(smi.cell);

				Log.Debug("updating gas flow: " + flow);
				switch (flow)
				{
					case float i when i > STILL_THRESHOLD && i < SLOW_THRESHOLD:
						smi.GoTo(smi.sm.flowing.mild);
						break;
					case float i when i > SLOW_THRESHOLD && i < MEDIUM_THRESHOLD:
						smi.GoTo(smi.sm.flowing.medium);
						break;
					case float i when i > MEDIUM_THRESHOLD:
						smi.GoTo(smi.sm.flowing.hard);
						break;
					default:
						smi.GoTo(smi.sm.flowing.still);
						break;
				}

				smi.flow = flow;
				smi.master.flowTest = flow;
			}

			private bool IsOperational(StatesInstance smi, object _) => smi.operational.IsOperational;

			public class FlowState : State
			{
				public State still;
				public State mild;
				public State medium;
				public State hard;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FlowTexVec2
		{
			public float X;
			public float Y;
		}
	}
}
