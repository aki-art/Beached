using Beached.Content.Defs.Items;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace Beached.Content
{
	public class BTags
	{
		public static readonly Tag
			amphibious = TagManager.Create("Beached_Amphibious"),
			amberInclusion = TagManager.Create("Beached_AmberInclusion"),
			aquatic = TagManager.Create("Beached_Aquatic"),
			bamboo = TagManager.Create("Beached_Bamboo"),
			blueprintable = TagManager.Create("Beached_Blueprintable"),
			coral = TagManager.Create("Beached_Coral"),
			coralFrag = TagManager.Create("Beached_CoralFrag"),
			smallAquariumSeed = TagManager.Create("Beached_SmallAquariumSeed"),
			corrodable = TagManager.Create("Beached_Corrodable"), // get damaged in acid
			dim = TagManager.Create("Beached_Dim"),
			furAllergic = TagManager.Create("Beached_FurAllergic"),
			geneticallyModified = TagManager.Create("Beached_GeneticallyModified"),
			geneticSample = TagManager.Create("Beached_GeneticSample"),
			heliumPoffed = TagManager.Create("Beached_HeliumPoffed"),
			lubricated = TagManager.Create("Beached_Lubricated"),
			meat = TagManager.Create("Beached_Meat"), // any food from animals. Va'Hano eats these
			noBackwall = TagManager.Create("NoBackwall"), // Background Tiles mod uses this
			noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
			palateCleansed = TagManager.Create("Beached_PalateCleansed"),
			palateCleanserFood = TagManager.Create("Beached_PalateCleanserFood"),
			seafoodAllergic = TagManager.Create("Beached_SeafoodAllergic"),
			setPiece = TagManager.Create("Beached_SetPiece"),
			glacier = TagManager.Create("Beached_Glacier"),
			vista = TagManager.Create("Beached_Vista"), // allows Vista rooms
			wishingStars = TagManager.Create("Beached_WishingStars"); // used on events that should trigger the wishing star effect on dupes

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
			public static readonly Tag
				secretingMucus = TagManager.Create("Beached_SecretingMucus"),
				hunting = TagManager.Create("Beached_Hunting"),
				doNotTargetMeByCarnivores = TagManager.Create("Beached_DoNotTargetMeByCarnivores");
		}

		public static class Species
		{
			public static readonly Tag snail = TagManager.Create("BeachedSnailSpecies");
			public static readonly Tag muffin = TagManager.Create("BeachedMuffinSpecies");
		}

		public static class MaterialCategories
		{
			public static Tag crystal = TagManager.Create("Beached_Crystal");
		}

		public static class BuildingMaterials
		{
			public static Tag chime = TagManager.Create("Beached_ChimeMaterial");
		}

		public static class TagCollections
		{
			public static HashSet<Tag> cullableCreatures;

			public static void Setup()
			{
				cullableCreatures = Assets.GetPrefabsWithComponent<CreatureBrain>()
				.Where(go => !go.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
				.Select(go => go.PrefabID())
				.ToHashSet();
			}
		}

		public static void OnDbInit()
		{
			TagCollections.Setup();
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
