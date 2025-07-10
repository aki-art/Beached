namespace Beached.Settings
{
	public class Graphics
	{
		public bool MirrorsRender { get; set; } = true;

		public int MaxEntitiesVisibleInMirrors { get; set; } = 8;

		public Quantity ParticlesCount { get; set; } = Quantity.Normal;

		public enum Quantity
		{
			Essential,
			Less,
			Normal
		}
	}
}
