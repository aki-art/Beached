namespace Beached.Content.Scripts
{
    public class PlushPlacebleBedSensor : Sensor
    {
        private Navigator navigator;
        public int cell;

        public PlushPlacebleBedSensor(Sensors sensors) : base(sensors)
        {
            navigator = GetComponent<Navigator>();
        }

        public override void Update()
        {
            cell = Grid.InvalidCell;

            foreach (var bed in Mod.plushiePlaceables)
            {
                var buildingCell = bed.NaturalBuildingCell();

                if (IsBedEligible(bed, buildingCell))
                {
                    cell = bed.NaturalBuildingCell();
                    break;
                }
            }
        }

        private bool IsBedEligible(Beached_PlushiePlaceable bed, int cell)
        {
            return !bed.HasPlushie()
                && bed.GetComponent<Operational>().IsOperational
                && navigator.GetNavigationCost(cell) != -1;
        }
    }
}
