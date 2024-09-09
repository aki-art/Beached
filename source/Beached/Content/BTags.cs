using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace Beached.Content
{
	public class BTags
	{
		public static readonly Tag
			// ======= The ones potentially interesting for other mods ===========
			// any buildig where foods are queued. meats will be scanned here, and Chef Makis can operate them
			cookingStation = TagManager.Create("Beached_CookingStation"),
			// do not enable traits on this geyser prefab
			geyserNoTraits = TagManager.Create("Beached_GeyserNoTraits"),
			// generate a blueprint for this building prefab, single time buildable
			blueprintable = TagManager.Create("Beached_Blueprintable"),
			// allow planting this in a small aquarium. will expect an "aquarium_loop" animation in the kanim
			smallAquariumSeed = TagManager.Create("Beached_SmallAquariumSeed"),
			// any food from animals.Va'Hano diet, Muffin diet, Vegetarians stress out by it
			meat = TagManager.Create("Beached_Meat"),
			// foods that do not count as vegetarian, but also not enough meat to be "carnivore" diet either.
			// if the food already has Meat tag this is not neccessary
			nonVegetarian = TagManager.Create("Beached_NonVegetarian"),
			// triggers the Vista roomtype
			vista = TagManager.Create("Beached_Vista"),
			// should this critter induce small explosions when in contact with acid.
			// (Metallic scales or metals at positions above copper in the reactivity series.)
			sparksOnAcid = TagManager.Create("Beached_SparksOnAcid"),

			// ========= Other Mods ==============================================
			BackWalls_noBackwall = TagManager.Create("NoBackwall"),
			MaterialColor_noPaint = TagManager.Create("NoPaint"),
			FastTrack_registerRoom = TagManager.Create("RegisterRoom"),

			// ========= Misc ====================================================
			amphibious = TagManager.Create("Beached_Amphibious"),
			amberInclusion = TagManager.Create("Beached_AmberInclusion"),
			aquaticPlant = TagManager.Create("Beached_AquaticPlant"),
			aquaticFarm = TagManager.Create("Beached_AquaticFarm"),
			bamboo = TagManager.Create("Beached_Bamboo"),
			carnivorous = TagManager.Create("Beached_Carnivorous"),
			coral = TagManager.Create("Beached_Coral"),
			coralFrag = TagManager.Create("Beached_CoralFrag"),
			corrodable = TagManager.Create("Beached_Corrodable"), // get damaged in acid
			dim = TagManager.Create("Beached_Dim"),
			furAllergic = TagManager.Create("Beached_FurAllergic"),
			geneticallyModified = TagManager.Create("Beached_GeneticallyModified"),
			geneticSample = TagManager.Create("Beached_GeneticSample"),
			heliumPoffed = TagManager.Create("Beached_HeliumPoffed"),
			lubricated = TagManager.Create("Beached_Lubricated"),
			palateCleansed = TagManager.Create("Beached_PalateCleansed"),
			palateCleanserFood = TagManager.Create("Beached_PalateCleanserFood"),
			seafoodAllergic = TagManager.Create("Beached_SeafoodAllergic"),
			setPiece = TagManager.Create("Beached_SetPiece"),
			glacier = TagManager.Create("Beached_Glacier"),
			vegetarian = TagManager.Create("Beached_Vegetarian"),
			buildingAttachmentSmoker = TagManager.Create("Beached_BuildingAttachmentSmoker"),
			// used on events that should trigger the wishing star effect on dupes
			wishingStars = TagManager.Create("Beached_WishingStars");

		public static TagSet eggs =
		[
			GameTags.IncubatableEgg
		];

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
			GameTags.MaterialBuildingElements.Add(BuildingMaterials.chime);

			GameTags.MaterialCategories.Add(MaterialCategories.crystal);

			var index = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.FindIndex(tag => tag == GameTags.BuildableProcessed);
			if (index == -1) index = 0; // in case some other mod tweaked the filters and removed BuildableProcessed

			STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Insert(index, MaterialCategories.crystal);
			STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.AddToArray(MaterialCategories.crystal);
			STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Add(MaterialCategories.crystal);

			STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.AddToArray(coralFrag);

			Filterable.filterableCategories.Add(GameTags.Egg);
			GameTags.AllCategories.Add(GameTags.Egg);
			GameTags.IgnoredMaterialCategories.Remove(GameTags.Egg);

			GameTags.AllCategories.Add(coralFrag);
			Filterable.filterableCategories.Add(coralFrag);
		}
	}
}
