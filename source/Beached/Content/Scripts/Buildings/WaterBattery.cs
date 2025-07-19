using System;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class WaterBattery : Generator, IActivationRangeTarget
	{
		[SerializeField] public Storage lowerStorage;
		[SerializeField] public Storage upperStorage;
		[MyCmpReq] private Battery battery;

		public float MinValue => 0;
		public float MaxValue => 100;
		public bool UseWholeNumbers => true;
		public string ActivationRangeTitleText => "ActivationRangeTitleText";
		public string ActivateSliderLabelText => "ActivateSliderLabelText";
		public string DeactivateSliderLabelText => "DeactivateSliderLabelText";
		public string ActivateTooltip => "ActivateTooltip";
		public string DeactivateTooltip => "DeactivateTooltip";

		public float ActivateValue { get; set; }
		public float DeactivateValue { get; set; }

		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);

			operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

			if (!operational.IsOperational)
				return;

			var circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
				return;

			var batteries = circuitManager.GetBatteriesOnCircuit(CircuitID);
			if (batteries == null || batteries.Count() == 0)
				return;

			var storedEnergy = 0f;
			var maxStoredEnergy = 0f;

			foreach (var battery in batteries)
			{
				storedEnergy += battery.JoulesAvailable;
				maxStoredEnergy += battery.capacity;
			}

			var ratio = storedEnergy / maxStoredEnergy;
			if (ratio <= ActivateValue)

				AssignJoulesAvailable(operational.IsOperational ? Math.Min(battery.JoulesAvailable, WattageRating * dt) : 0.0f);
		}
	}
}
