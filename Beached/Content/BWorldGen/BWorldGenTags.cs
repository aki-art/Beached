using ProcGen;

namespace Beached.Content.BWorldGen
{
    public class BWorldGenTags
    {
        public static Tag AtSideADepth = TagManager.Create("Beached_AtSideADepth");
        public static Tag AtSideBDepth = TagManager.Create("Beached_AtSideBDepth");
        public static Tag AtSideACorner = TagManager.Create("Beached_AtSideACorner");
        public static Tag AtSideBCorner = TagManager.Create("Beached_AtSideBCorner");
        public static Tag Reefify = TagManager.Create("Beached_Reefify");

        // used to replace the noise generation with something that closer resembles raw noise data
        // roughing up is done with post processors later
        public static Tag SmoothNoise = TagManager.Create("Beached_SmoothNoise");

        public static void Initialize()
        {
            WorldGenTags.DistanceTags.Add(AtSideADepth);
            WorldGenTags.DistanceTags.Add(AtSideBDepth);
            WorldGenTags.DistanceTags.Add(AtSideACorner);
            WorldGenTags.DistanceTags.Add(AtSideBCorner);
        }
    }
}
