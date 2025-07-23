using Beached.Content.ModDb;
using ImGuiNET;
using Klei.AI;
using System;
using UnityEngine;
using static Beached.Content.Scripts.Buildings.Chime;

namespace Beached.Content.Scripts.Buildings
{
	public class WaterGenerator : Generator, IImguiDebug, ISim33ms
	{
		private static float FLOW_MULTIPLIER = 400_000f;
		private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private KBatchedAnimController kbac;

		private float _power01;
		private bool _animDirty;
		private int _lastFanState;
		private Guid statusHandle;

		private AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -100f, "modifier");

		private float _forceOverrideFlow;

		public WaterGenerator()
		{
			_forceOverrideFlow = -1f;
			_lastFanState = -1;
			_animDirty = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.ActiveChanged, OnActiveChanged);
			gameObject.GetAttributes().Add(modifier);
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

		private void UpdateStatusItem()
		{
			var statusItem = BStatusItems.waterMillWattage;
			//	selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage);

			if (statusHandle == Guid.Empty)
				statusHandle = selectable.AddStatusItem(statusItem, this);
			else
				GetComponent<KSelectable>().ReplaceStatusItem(statusHandle, statusItem, this);
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


			var averageFlow = Game.Instance.accumulators.GetAverageRate(accumulator);

			averageFlow = Mathf.Clamp(averageFlow, 0, 1f / 3f) * 3f;

			if (_forceOverrideFlow != -1)
			{
				averageFlow = _forceOverrideFlow;
			}

			modifier.SetValue((-1f + averageFlow) * 100f);

			if (averageFlow <= 0)
			{
				operational.SetActive(false);
				_animDirty = _power01 != 0;
				return;
			}

			var powerGenerated = WattageRating * dt;

			operational.SetActive(true);
			_animDirty = _power01 != powerGenerated;

			if (powerGenerated > 0f)
			{
				// Mathf.Max(averageFlow * dt, dt);
				var power = powerGenerated * dt;
				GenerateJoules(power);
				var power01 = Mathf.Clamp01(power / building.Def.GeneratorWattageRating);
				_animDirty = power01 != 0;
				_power01 = power01;
			}

			this.UpdateStatusItem();
		}

		public void Sim33ms(float dt)
		{
			if (_animDirty)
			{
				kbac.PlaySpeedMultiplier = Mathf.Clamp01(_power01);
				kbac.SetDirty();
				kbac.UpdateAnim(0);
				_animDirty = false;
			}
		}

		unsafe public float Flow(int cell)
		{
			return Beached_Grid.GetFlow(cell);

			var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
			var flowTexVec = vecPtr[cell];
			var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

			return flowVec.magnitude;
		}

		public void OnImguiDraw()
		{
			ImGui.DragFloat("flow mult###WaterGeneratorFlowMult", ref FLOW_MULTIPLIER, 1, 1000, 100000);
			ImGui.DragFloat("force override flow###WaterGeneratorOverrideDlow", ref _forceOverrideFlow, 0.01f, -1f, 1f);
			ImGui.Text("flow is : " + Flow(Grid.PosToCell(this)));
			ImGui.Text("average flow is : " + Game.Instance.accumulators.GetAverageRate(accumulator));
			ImGui.Text("anim speed is : " + _power01);
		}

		public float GetPower() => _power01;
	}
}
