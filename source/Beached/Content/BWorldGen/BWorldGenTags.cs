using ProcGen;

namespace Beached.Content.BWorldGen
{
    public class BWorldGenTags
    {
        public static Tag AtSideADepth = TagManager.Create("Beached_AtSideADepth");
        public static Tag AtSideBDepth = TagManager.Create("Beached_AtSideBDepth");
        public static Tag AtSideACorner = TagManager.Create("Beached_AtSideACorner");
        public static Tag AtSideBCorner = TagManager.Create("Beached_AtSideBCorner");

        // if a template is tagged with this, it turns into a Reef Biome
        public static Tag Reefify = TagManager.Create("Beached_Reefify");
        // spawns a layer of sand above all solids
        public static Tag SandBeds = TagManager.Create("Beached_SandBeds");
        // turns some tiles into their crushed variant
        public static Tag Shattered = TagManager.Create("Beached_Shattered");
        // used to distinguish between the mimic frozen core and lush core traits
        public static Tag BeachedTraits = TagManager.Create("Beached_Traits");

        public static void Initialize()
        {
            WorldGenTags.DistanceTags.Add(AtSideADepth);
            WorldGenTags.DistanceTags.Add(AtSideBDepth);
            WorldGenTags.DistanceTags.Add(AtSideACorner);
            WorldGenTags.DistanceTags.Add(AtSideBCorner);
        }
    }
}
