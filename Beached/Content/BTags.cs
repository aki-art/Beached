using Beached.Content.Defs.Items;
using TUNING;
using UnityEngine;

namespace Beached.Content
{
    public class BTags
    {
        public static readonly Tag amphibious = TagManager.Create("Beached_Amphibious");
        public static readonly Tag bamboo = TagManager.Create("Beached_Bamboo");
        public static readonly Tag blueprintable = TagManager.Create("Beached_Blueprintable");
        public static readonly Tag lubricated = TagManager.Create("Beached_Lubricated");
        public static readonly Tag coral = TagManager.Create("Beached_Coral");
        public static readonly Tag corrodable = TagManager.Create("Beached_Corrodable");
        public static readonly Tag vista = TagManager.Create("Beached_Vista");

        public static class FastTrack
        {
            public static readonly Tag registerRoom = TagManager.Create("RegisterRoom");
        }

        public class Creatures
        {
            public static readonly Tag secretingMucus = TagManager.Create("BeachedSecretingMucus");
        }

        public static class Species
        {
            public static readonly Tag snail = TagManager.Create("BeachedSnailSpecies");
        }

        public static class MaterialCategories
        {
            public static Tag crystal = TagManager.Create("Beached_Crystal");
            public static Tag dim = TagManager.Create("Beached_Dim");
            public static Tag dark = TagManager.Create("Beached_Dark");
        }

        public static void OnModLoad()
        {
            GameTags.MaterialBuildingElements.Add(SeaShellConfig.ID);
            GameTags.MaterialCategories.Add(MaterialCategories.crystal);

            var index = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.FindIndex(tag => tag == GameTags.BuildableProcessed);
            if (index == -1) index = 0; // in case some other mod tweaked the filters and removed BuildableProcessed

            STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Insert(index, MaterialCategories.crystal);
        }
    }
}
