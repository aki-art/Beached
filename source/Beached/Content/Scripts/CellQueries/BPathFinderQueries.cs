namespace Beached.Content.Scripts.CellQueries
{
	public class BPathFinderQueries
	{
		public static DryLandQuery dryLandQuery = new();

		public static void Reset()
		{
			dryLandQuery = new();
		}
	}
}
