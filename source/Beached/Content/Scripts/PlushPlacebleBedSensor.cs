﻿using Beached.Content.ModDb;
using Klei.AI;

namespace Beached.Content.Scripts
{
	public class PlushPlacebleBedSensor : Sensor
	{
		private Navigator navigator;
		public Beached_PlushiePlaceable placeable;
		private int cell;
		private bool eligible;

		public PlushPlacebleBedSensor(Sensors sensors) : base(sensors)
		{
			eligible = sensors.GetComponent<Traits>().HasTrait(BTraits.PLUSHIE_MAKER);
			navigator = GetComponent<Navigator>();
		}

		public override void Update()
		{
			if (!eligible || placeable != null)
				return;

			foreach (var bed in ModCmps.plushiePlaceables.items)
			{
				if (IsBedEligible(bed))
				{
					placeable = bed;
					break;
				}
			}
		}

		public int GetCell() => placeable == null ? -1 : Grid.PosToCell(placeable);

		private bool IsBedEligible(Beached_PlushiePlaceable bed)
		{
			return !bed.HasPlushie()
				&& bed.GetComponent<Operational>().IsOperational
				&& navigator.GetNavigationCost(bed.NaturalBuildingCell()) != -1;
		}

		public void Clear()
		{
			placeable = null;
		}
	}
}
