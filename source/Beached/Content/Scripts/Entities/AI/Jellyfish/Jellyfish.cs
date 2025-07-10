
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Jellyfish : CritterGeneratorSpawner, ISim200ms, IImguiDebug, ISim1000ms
	{
		[Serialize] public float ElapsedSinceLastPulse { get; private set; }
		[Serialize] public bool isEnergized;

		[MySmiReq] public PulseMonitor.Instance pulseMonitor;
		[MyCmpReq] public ElectricEmitter emitter;

		[SerializeField] public float pulseDurationSeconds;

		public Jellyfish()
		{
			pulseDurationSeconds = 2.0f;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModHashes.medusaSignal, Pulse);
			Subscribe((int)GameHashes.Butcher, OnButchered);
		}

		// drops bottled water, empty it here
		private void OnButchered(object obj)
		{
			if (obj is GameObject[] drops)
			{
				for (var i = drops.Length - 1; i >= 0; i--)
				{
					var drop = drops[i];
					if (drop.TryGetComponent(out Dumpable dumpable))
						dumpable.Dump();
				}
			}
		}

		public void Pulse(object data)
		{
			ElapsedSinceLastPulse = 0;
			isEnergized = true;
		}

		public void Sim200ms(float dt)
		{
			if (isEnergized)
			{
				ElapsedSinceLastPulse += dt;

				if (ElapsedSinceLastPulse > pulseDurationSeconds)
				{
					Trigger(ModHashes.depleted);
					isEnergized = false;
					ElapsedSinceLastPulse = 0;
				}
			}
		}

		public void OnImguiDraw()
		{
#if DEVTOOLS
			if (ImGuiNET.ImGui.Button("Pulse"))
				Trigger(ModHashes.medusaSignal);
#endif
		}

		internal void EnableConnector()
		{
		}

		public void Sim1000ms(float dt)
		{
			emitter.Pulse(-1f, 4f, 1);
		}
	}
}
