using Beached.Content.Defs.Items;
using TUNING;

namespace Beached.Content
{
	public class BTags
	{
		public static readonly Tag
			meat = TagManager.Create("Beached_Meat"), // any food from animals. Va'Hano eats these
			corrodable = TagManager.Create("Beached_Corrodable"), // get damaged in acid
			vista = TagManager.Create("Beached_Vista"), // allows Vista rooms
			lubricated = TagManager.Create("Beached_Lubricated"),
			amphibious = TagManager.Create("Beached_Amphibious"),
			bamboo = TagManager.Create("Beached_Bamboo"),
			blueprintable = TagManager.Create("Beached_Blueprintable"),
			coral = TagManager.Create("Beached_Coral"),
			noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
			noBackwall = TagManager.Create("NoBackwall"), // Background Tiles mod uses this
			aquatic = TagManager.Create("Beached_Aquatic"),
			geneticallyModified = TagManager.Create("Beached_GeneticallyModified"),
			wishingStars = TagManager.Create("Beached_WishingStars"), // used on events that should trigger the wishing star effect on dupes
			coralFrag = TagManager.Create("Beached_CoralFrag"),
			dim = TagManager.Create("Beached_Dim"),
			palateCleanserFood = TagManager.Create("Beached_PalateCleanserFood"),
			palateCleansed = TagManager.Create("Beached_PalateCleansed"),
			furAllergic = TagManager.Create("Beached_FurAllergic");

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
