using Beached.Content.Defs.Items;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content
{
    public class BTags
    {
        // relevant to other mods
        public static readonly Tag meat = TagManager.Create("Beached_Meat"); // any food from animals. Va'Hano eats these
        public static readonly Tag corrodable = TagManager.Create("Beached_Corrodable"); // get damaged in acid
        public static readonly Tag vista = TagManager.Create("Beached_Vista"); // allows Vista rooms

        public static readonly Tag lubricated = TagManager.Create("Beached_Lubricated");
        public static readonly Tag amphibious = TagManager.Create("Beached_Amphibious");
        public static readonly Tag bamboo = TagManager.Create("Beached_Bamboo");
        public static readonly Tag blueprintable = TagManager.Create("Beached_Blueprintable");
        public static readonly Tag coral = TagManager.Create("Beached_Coral");
        public static readonly Tag noPaint = TagManager.Create("NoPaint"); // MaterialColor mod uses this
        public static readonly Tag noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this
        public static readonly Tag aquatic = TagManager.Create("Beached_Aquatic"); 
        public static readonly Tag geneticallyModified = TagManager.Create("Beached_GeneticallyModified");

        public static TagSet eggs = new()
        {
            GameTags.IncubatableEgg
        };

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

        public static class BuildingMaterials
        {
            public static Tag chime = TagManager.Create("Beached_ChimeMaterial");
        }

        public static void OnModLoad()
        {
            GameTags.MaterialBuildingElements.Add(SeaShellConfig.ID);
            GameTags.MaterialCategories.Add(MaterialCategories.crystal);

            var index = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.FindIndex(tag => tag == GameTags.BuildableProcessed);
            if (index == -1) index = 0; // in case some other mod tweaked the filters and removed BuildableProcessed

            STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Insert(index, MaterialCategories.crystal);

            Filterable.filterableCategories.Add(GameTags.Egg);
            GameTags.AllCategories.Add(GameTags.Egg);
            GameTags.IgnoredMaterialCategories.Remove(GameTags.Egg);
        }
    }
}
