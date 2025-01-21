using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities.AI.Strobila;
using Beached.Content.Scripts.SegmentedEntities;

namespace Beached
{
	public class ModCmps
	{
		public static Components.Cmps<Beached_PlushiePlaceable> plushiePlaceables = [];
		public static Components.Cmps<ForceField> forceFields = [];
		public static Components.Cmps<SegmentedEntityRoot> segmentedEntityRoots = [];
		public static Components.Cmps<Strobila> jellyfishStrobilas = [];
	}
}
