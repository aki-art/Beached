namespace Beached.Content.Scripts
{
	public class PlushPlacebleBedSensor : Sensor
	{
		private Navigator navigator;
		public Beached_PlushiePlaceable placeable;

		public PlushPlacebleBedSensor(Sensors sensors) : base(sensors)
		{
			navigator = GetComponent<Navigator>();
		}

		public override void Update()
		{
			if (placeable != null)
				return;

			foreach (var bed in Mod.plushiePlaceables.items)
			{
				if (IsBedEligible(bed))
				{
					placeable = bed;
					break;
				}
			}
		}

		private bool IsBedEligible(Beached_PlushiePlaceable bed)
		{
			return !bed.HasPlushie()
				&& bed.GetComponent<Operational>().IsOperational
				&& navigator.GetNavigationCost(bed.NaturalBuildingCell()) != -1;
		}
	}
}
