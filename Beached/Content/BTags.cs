using TUNING;
using UnityEngine;

namespace Beached.Content
{
    public class BTags
    {
        public static class MaterialCategories
        {
            public static Tag Crystal = TagManager.Create("Beached_Crystal");
            public static Tag Dim = TagManager.Create("Beached_Dim");
            public static Tag Dark = TagManager.Create("Beached_Dark");
        }

        public static void OnModLoad()
        {
            GameTags.MaterialCategories.Add(MaterialCategories.Crystal);

            var index = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.FindIndex(tag => tag == GameTags.BuildableProcessed);
            index = Mathf.Max(index, 0); // in case some other mod tweaked the filters and removed BuildableProcessed

            STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Insert(index, MaterialCategories.Crystal);
        }
    }
}
