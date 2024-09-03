using FMOD.Studio;
using ImGuiNET;
using System.Runtime.InteropServices;

namespace Beached.Content.Scripts.Buildings
{
	public class Chime : StateMachineComponent<Chime.StatesInstance>, IImguiDebug
	{
		private const float STILL_THRESHOLD = 0.001f;
		private const float SLOW_THRESHOLD = 0.002f;
		private const float MEDIUM_THRESHOLD = 0.003f;
		private const float VOLUME_LERP_SPEED = 2f;
		private EventInstance soundEvent;

		public override void OnSpawn() => smi.StartSM();

		private static float strength = 0f;
		public void OnImguiDraw()
		{
			ImGui.Text($"Chime Power: {smi.sm.ChimePower(smi.cell)}");

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

			public StatesInstance(Chime master) : base(master)
			{
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
				default_state = notInGas;

				notInGas
					.EventHandlerTransition(GameHashes.OperationalChanged, flowing, IsOperational);

				flowing
					.DefaultState(flowing.still)
					//.Enter(smi => smi.audioPlayer.PlayLoop(ModAssets.Sounds.SHELL_CHIME_LOUD))
					//.Exit(smi => smi.audioPlayer.StopLooping())
					.Update(UpdateFlow, UpdateRate.SIM_200ms)
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

			private void UpdateFlow(StatesInstance smi, float _)
			{
				var flow = ChimePower(smi.cell);
				if (flow < smi.flowMin || flow > smi.flowMax)
				{
					switch (flow)
					{
						case float i when i > STILL_THRESHOLD && i < SLOW_THRESHOLD:
							smi.GoTo(smi.sm.flowing.mild);
							//smi.audioPlayer.SetTargetVolume(0.2f, VOLUME_LERP_SPEED);
							break;
						case float i when i > SLOW_THRESHOLD && i < MEDIUM_THRESHOLD:
							smi.GoTo(smi.sm.flowing.medium);
							//smi.audioPlayer.SetTargetVolume(0.5f, VOLUME_LERP_SPEED);
							break;
						case float i when i > MEDIUM_THRESHOLD:
							smi.GoTo(smi.sm.flowing.hard);
							//smi.audioPlayer.SetTargetVolume(1f, VOLUME_LERP_SPEED);
							break;
						default:
							smi.GoTo(smi.sm.flowing.still);
							//smi.audioPlayer.SetTargetVolume(0f, VOLUME_LERP_SPEED);
							break;
					}
				}

			}

			// credit: Asquared
			unsafe public float ChimePower(int cell)
			{
				var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
				var flowTexVec = vecPtr[cell];
				var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

				return flowVec.sqrMagnitude;
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
