using ImGuiNET;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class WaterGenerator : Generator, IImguiDebug, ISim33ms, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
	{
		private static float FLOW_MULTIPLIER = 40f;
		private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpGet] private ManualDeliveryKG delivery;

		[SerializeField] public float windDownSpeed = 0.01f;
		[SerializeField] public Storage storage;
		[SerializeField] public EnergyGenerator.Formula formula;

		private float batteryRefillPercent = 0.5f;
		private float _power01;
		private bool _animDirty;
		private Guid statusHandle;

		private AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -100f, "modifier");

		private float _forceOverrideFlow;

		public WaterGenerator()
		{
			_forceOverrideFlow = -1f;
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
		public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";

		public string SliderUnits => global::STRINGS.UI.UNITSUFFIXES.PERCENT;

		public int SliderDecimalPlaces(int _) => 0;

		public float GetSliderMin(int _) => 0.0f;

		public float GetSliderMax(int _) => 100f;

		public float GetSliderValue(int _) => this.batteryRefillPercent * 100f;

		public void SetSliderValue(float value, int _) => this.batteryRefillPercent = value / 100f;

		string ISliderControl.GetSliderTooltip(int _)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), delivery.RequestedItemTag.ProperName(), batteryRefillPercent * 100.0f);
		}

		public string GetSliderTooltipKey(int _) => "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";

		private bool IsConvertible(float dt)
		{
			foreach (var input in formula.inputs)
			{
				var massAvailable = storage.GetMassAvailable(input.tag);
				var wantsToConsume = input.consumptionRate * dt;

				if (massAvailable < wantsToConsume)
					return false;
			}

			return true;
		}

		protected void OnActiveChanged(object data)
		{
			var status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
			kSelectable.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
		}

		private void UpdateAnimation()
		{
			if (kbac.currentAnim == "on")
			{
				if (_power01 > 0.99f)
				{
					kbac.Play("on_fast", KAnim.PlayMode.Loop);
					return;
				}
			}
			else if (_power01 < 0.99f)
			{
				kbac.Play("on", KAnim.PlayMode.Loop);
				return;
			}

			kbac.Play(kbac.currentAnim, KAnim.PlayMode.Loop);
		}

		private void UpdateStatusItem()
		{
			/*

			var statusItem = BStatusItems.waterMillWattage;
			//	selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage);

			if (statusHandle == Guid.Empty)
				statusHandle = selectable.AddStatusItem(statusItem, this);
			else
				GetComponent<KSelectable>().ReplaceStatusItem(statusHandle, statusItem, this);
			*/
		}

		private float previousFlow = 0.0f;
		private bool wasWorkingPreviousUpdate;

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);
			operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

			if (!operational.IsOperational)
			{
				if (wasWorkingPreviousUpdate)
				{
					_power01 = 0;
					_animDirty = true;
					UpdateStatusItem();

					wasWorkingPreviousUpdate = false;
				}

				return;
			}


			var batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(CircuitID);
			var sufficientlyFull = false;

			foreach (var battery in batteriesOnCircuit)
			{
				if (batteryRefillPercent <= 0.0f && battery.PercentFull <= 0.0f)
				{
					sufficientlyFull = true;
					break;
				}

				if (battery.PercentFull < batteryRefillPercent)
				{
					sufficientlyFull = true;
					break;
				}
			}

			// todo
			//selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !sufficientlyFull);

			if (delivery != null)
				delivery.Pause(!sufficientlyFull, "Circuit has sufficient energy");

			var cell = Grid.PosToCell(this);
			var isInLiquid = Grid.IsSubstantialLiquid(cell);

			var currentFlow = isInLiquid ? (Flow(cell) * FLOW_MULTIPLIER) : 0f;

			Game.Instance.accumulators.Accumulate(accumulator, currentFlow * dt);

			foreach (var input in formula.inputs)
			{
				var amount = input.consumptionRate * dt;
				storage.ConsumeIgnoringDisease(input.tag, amount);
			}

			var averageFlow = Game.Instance.accumulators.GetAverageRate(accumulator);

			var flow = averageFlow;
			averageFlow = Mathf.Max(averageFlow, previousFlow);

			previousFlow = flow;

			//averageFlow = Mathf.Clamp(averageFlow, 0, 1f / 3f) * 3f;
			averageFlow = Mathf.Clamp(averageFlow, 0, 1f);

			if (_forceOverrideFlow != -1)
			{
				averageFlow = _forceOverrideFlow;
			}

			var modifierValue = (-1f + averageFlow) * 100f;
			modifier.SetValue(modifierValue);

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
				var power01 = Mathf.Clamp01(powerGenerated / building.Def.GeneratorWattageRating);
				power01 *= (1f / dt);

				_animDirty = power01 != 0;
				_power01 = power01;
			}

			UpdateStatusItem();
			wasWorkingPreviousUpdate = true;
		}

		public void Sim33ms(float dt)
		{
			if (_animDirty)
			{
				var position = kbac.GetPositionPercent();
				position %= 1.0f;

				kbac.PlaySpeedMultiplier = Mathf.Clamp01(_power01);
				UpdateAnimation();
				kbac.SetPositionPercent(position);

				_animDirty = false;
			}
		}

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			var descriptorList = new List<Descriptor>();

			if (formula.inputs == null || formula.inputs.Length == 0)
				return descriptorList;

			foreach (var input in formula.inputs)
			{
				var str = input.tag.ProperName();
				var descriptor = new Descriptor();
				descriptor.SetupDescriptor(string.Format((string)global::STRINGS.UI.BUILDINGEFFECTS.ELEMENTCONSUMED, str, GameUtil.GetFormattedMass(input.consumptionRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}")), string.Format((string)global::STRINGS.UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, str, GameUtil.GetFormattedMass(input.consumptionRate, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}")), Descriptor.DescriptorType.Requirement);
				descriptorList.Add(descriptor);
			}

			return descriptorList;
		}


		unsafe public float Flow(int cell)
		{
			return Beached_Grid.GetFlow(cell);
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
