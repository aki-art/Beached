using UnityEngine;
using static Beached.Content.Scripts.Buildings.Chime;

namespace Beached.Content.Scripts.Buildings
{
	public class WaterGenerator : Generator
	{
		private const float FLOW_MULTIPLIER = 20_000f;
		private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

		[MyCmpReq] private KSelectable kSelectable;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.ActiveChanged, OnActiveChanged);
			accumulator = Game.Instance.accumulators.Add("Beached_WaterGenerator", this);
		}

		public override void OnCleanUp()
		{
			Game.Instance.accumulators.Remove(accumulator);
			base.OnCleanUp();
		}

		protected void OnActiveChanged(object data)
		{
			var status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
			kSelectable.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
		}

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);
			operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

			if (!operational.IsOperational)
				return;

			var cell = Grid.PosToCell(this);
			var isInLiquid = Grid.IsSubstantialLiquid(cell);

			var currentFlow = isInLiquid ? (Flow(cell) * FLOW_MULTIPLIER) : 0f;
			Game.Instance.accumulators.Accumulate(accumulator, currentFlow * dt);

			operational.SetActive(currentFlow > 0f);

			var averageFlow = Game.Instance.accumulators.GetAverageRate(this.accumulator);

			var powerGenerated = Mathf.Clamp(averageFlow, 0f, 300f);

			if (powerGenerated > 0f)
				GenerateJoules(Mathf.Max(averageFlow * dt, dt));

			//this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 380f);
			//this.UpdateStatusItem();
		}

		unsafe public float Flow(int cell)
		{
			var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
			var flowTexVec = vecPtr[cell];
			var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

			return flowVec.sqrMagnitude;
		}
	}
}
