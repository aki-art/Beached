using static ProcGen.WorldPlacement;

namespace Beached.Content.BWorldGen
{
	public class BLocationTypes
	{
		public static readonly LocationType
			BeforeMeteorSwarm = (LocationType)Hash.SDBMLower("Beached_BeforeMeteorSwarm"),
			InsideMeteorSwarm = (LocationType)Hash.SDBMLower("Beached_InsideMeteorSwarm"),
			AfterMeteorSwarm = (LocationType)Hash.SDBMLower("Beached_AfterMeteorSwarm");
	}
}
