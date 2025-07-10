using Beached.Content.Scripts.Entities;

namespace Beached.Content.Scripts.Buildings
{
	public class DevElectricEmitter : KMonoBehaviour, IMultiSliderControl, ISim33ms
	{
		[MyCmpReq] public ElectricEmitter emitter;

		public int strokeCount;
		public float interval;
		public float power;

		private float _elapsed;

		public string SidescreenTitleKey => "STRINGS.BUILDINGS.PREFABS.BEACHED_DEVELECTRICEMITTER.NAME";

		public ISliderControl[] sliderControls => [
			new IntervalControl(this),
			new PowerControl(this),
			new StrokeControl(this)
			];

		public bool SidescreenEnabled() => true;

		public void Sim33ms(float dt)
		{
			_elapsed += (dt * 1000f);

			if (_elapsed > interval)
			{
				emitter.Pulse(0, power, strokeCount);
				_elapsed = 0;
			}
		}

		public class StrokeControl(DevElectricEmitter target) : ISingleSliderControl, ISliderControl
		{
			private readonly DevElectricEmitter target = target;

			public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.BEACHED_DEVELECTRICEMITTER.STROKE_LABEL";

			public string SliderUnits => " strokes";

			public float GetSliderMax(int index) => 10f;

			public float GetSliderMin(int index) => 1f;

			public string GetSliderTooltip(int index) => "";

			public string GetSliderTooltipKey(int index) => "";

			public float GetSliderValue(int index) => target.strokeCount;

			public void SetSliderValue(float value, int index) => target.strokeCount = (int)value;

			public int SliderDecimalPlaces(int index) => 0;
		}

		public class IntervalControl(DevElectricEmitter target) : ISingleSliderControl, ISliderControl
		{
			private readonly DevElectricEmitter target = target;

			public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.BEACHED_DEVELECTRICEMITTER.INTERVAL_LABEL";

			public string SliderUnits => " ms";

			public float GetSliderMax(int index) => 3000f;

			public float GetSliderMin(int index) => 33f;

			public string GetSliderTooltip(int index) => "";

			public string GetSliderTooltipKey(int index) => "";

			public float GetSliderValue(int index) => target.interval;

			public void SetSliderValue(float value, int index) => target.interval = value;

			public int SliderDecimalPlaces(int index) => 4;
		}

		public class PowerControl(DevElectricEmitter target) : ISingleSliderControl, ISliderControl
		{
			private readonly DevElectricEmitter target = target;

			public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.BEACHED_DEVELECTRICEMITTER.POWER_LABEL";

			public string SliderUnits => global::STRINGS.UI.UNITSUFFIXES.ELECTRICAL.JOULE;

			public float GetSliderMax(int index) => 100f;

			public float GetSliderMin(int index) => 0f;

			public string GetSliderTooltip(int index) => "";

			public string GetSliderTooltipKey(int index) => "";

			public float GetSliderValue(int index) => target.power;

			public void SetSliderValue(float value, int index) => target.power = value;

			public int SliderDecimalPlaces(int index) => 2;
		}
	}
}
