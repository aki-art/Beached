using ProcGen;

namespace Beached.Content.BWorldGen
{
	public class BWorldGenTags
	{
		public static Tag

			BeachedCluster = TagManager.Create("BeachedCluster"),

			AtSideADepth = TagManager.Create("Beached_AtSideADepth"),
			AtSideBDepth = TagManager.Create("Beached_AtSideBDepth"),
			AtSideACorner = TagManager.Create("Beached_AtSideACorner"),
			AtSideBCorner = TagManager.Create("Beached_AtSideBCorner"),

			// if a template is tagged with this, it turns into a Reef Biome
			// TODO: Moonlet will support biomed templates so this is unneccessary
			Reefify = TagManager.Create("Beached_Reefify"),
			// spawns a layer of sand above all solids
			SandBeds = TagManager.Create("Beached_SandBeds"),
			// turns some tiles into their crushed variant
			Shattered = TagManager.Create("Beached_Shattered"),
			// used to distinguish between the mimic frozen core and lush core traits
			BeachedTraits = TagManager.Create("Beached_Traits"),
			//
			WaveFunctionCollapse = TagManager.Create("Beached_WaveFunctionCollapse");

		public static void Initialize()
		{
			WorldGenTags.DistanceTags.Add(AtSideADepth);
			WorldGenTags.DistanceTags.Add(AtSideBDepth);
			WorldGenTags.DistanceTags.Add(AtSideACorner);
			WorldGenTags.DistanceTags.Add(AtSideBCorner);
		}
	}
}
