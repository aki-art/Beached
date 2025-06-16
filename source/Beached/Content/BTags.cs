using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace Beached.Content
{
	public class BTags
	{
		public static class Groups
		{
			public static Tag[] jellies =
			[
				JellyConfig.ID
			];

			public static Tag[] mosses =
			[
				Elements.moss.CreateTag(),
				Elements.fireMoss.CreateTag()
			];

			public static Tag[] grains =
			[
				ColdWheatConfig.SEED_ID,
				FernFoodConfig.ID
			];

			public static Tag[] sulfurs =
			[
				SulfurGlandConfig.ID,
				SimHashes.Sulfur.CreateTag()
			];
		}

		public static readonly Tag
			// ======= The ones potentially interesting for other mods ===========
			aquaticSeed = TagManager.Create("Beached_AquaticSeed"),
			// any buildig where foods are queued. meats will be scanned here, and Chef Makis can operate them
			cookingStation = TagManager.Create("Beached_CookingStation"),
			// triggers comfort seeker trait morale bonus when on a clothing item
			comfortableClothing = TagManager.Create("Beached_ComfortableCooking"),
			// add this to a creature to ignore electricity
			electricInvulnerable = TagManager.Create("Beached_ElectricInvulnerable"),
			// "eggs" Karacoos will consider an egg, but really aren't (infertile egg, egg artifact, etc.)
			karacooSittable = TagManager.Create("Beached_KaracooSittable"),
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
			// when submerged in a pot, this plant will take the liquid in for itself
			selfIrrigating = TagManager.Create("Beached_SelfIrrigating"),
			// triggers the Vista roomtype
			vista = TagManager.Create("Beached_Vista"),
			// should this critter induce small explosions when in contact with acid.
			// (Metallic scales or metals at positions above copper in the reactivity series.)
			sparksOnAcid = TagManager.Create("Beached_SparksOnAcid"),
			// plants and critters with this trait will ignore acid and treat it as a generic liquid
			acidImmune = TagManager.Create("Beached_AcidImmune"),
			preventLubrication = TagManager.Create("Beached_PreventLubrication"),
			mechanicalBuilding = TagManager.Create("Beached_MechanicalBuilding"),
			// used for showers or sinks to track if it has soap supplied
			soaped = TagManager.Create("Beached_Soaped"),
			underWater = TagManager.Create("Beached_UnderWater"),
			crystalCluster = TagManager.Create("Beached_CrystalCluster"),
			userNamedCritter = TagManager.Create("Beached_UserNamedCritter"),
			dnaAnalyzable = TagManager.Create("Beached_DNAAnalyzable"),
			markedForDNAAnalysis = TagManager.Create("Beached_MarkedForDNAAnalysis"),

			// ========= Other Mods ==============================================
			BackWalls_noBackwall = TagManager.Create("NoBackwall"),
			MaterialColor_noPaint = TagManager.Create("NoPaint"),
			FastTrack_registerRoom = TagManager.Create("RegisterRoom"),
			// Todo: remove when twitch mod is added
			OniTwitch_surpriseBoxForceDisabled = TagManager.Create("OniTwitchSurpriseBoxForceDisabled"),
			OniTwitch_surpriseBoxForceEnabled = TagManager.Create("OniTwitchSurpriseBoxForceEnabled "),

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
				jellyFishLure = TagManager.Create("Beached_JellyFishLure"),
				hunting = TagManager.Create("Beached_Behavior_Hunting"),
				wantsToSitOnEgg = TagManager.Create("Beached_Behavior_WantsToSitOnEgg"),
				// muffins will not hunt critters with this tag, regardless of collar settings
				doNotTargetMeByCarnivores = TagManager.Create("Beached_DoNotTargetMeByCarnivores"),
				muffinThreat = TagManager.Create("Beached_MuffinThreat"),

				beingMined = TagManager.Create("Beached_BeingMined"),
				grownShell = TagManager.Create("Beached_GrownShell");
		}

		public static class Species
		{
			public static readonly Tag
				snail = TagManager.Create("BeachedSnailSpecies"),
				muffin = TagManager.Create("BeachedMuffinSpecies"),
				mite = TagManager.Create("BeachedMiteSpecies"),
				jellyfish = TagManager.Create("BeachedJellyFishSpecies"),
				fuafua = TagManager.Create("BeachedFuaFuaSpecies"),
				rotMonger = TagManager.Create("BeachedRotMongerSpecies"),
				angularFish = TagManager.Create("BeachedAngularFishSpecies"),
				karacoo = TagManager.Create("BeachedKaracooSpecies");
		}

		public static class MaterialCategories
		{
			public static Tag crystal = TagManager.Create("Beached_Crystal");
		}

		public static class BuildingMaterials
		{
			public static Tag chime = TagManager.Create("Beached_ChimeMaterial");
			public static Tag sand = TagManager.Create("Beached_SandMaterial");
			public static Tag slag = TagManager.Create("Beached_SlagMaterial");
			public static Tag moss = TagManager.Create("Beached_MossMaterial");
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
			STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE = STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE.AddToArray(aquaticSeed);

			Filterable.filterableCategories.Add(GameTags.Egg);
			GameTags.AllCategories.Add(GameTags.Egg);
			GameTags.IgnoredMaterialCategories.Remove(GameTags.Egg);

			GameTags.AllCategories.Add(coralFrag);
			GameTags.AllCategories.Add(aquaticSeed);
			Filterable.filterableCategories.Add(coralFrag);
			Filterable.filterableCategories.Add(aquaticSeed);

			GameTags.Fabrics = GameTags.Fabrics.AddToArray(Elements.fuzz.CreateTag());

			//GameTags.AllCategories.Add(PalmLeafConfig.ID);
			//GameTags.AllCategories.Add(BioFuelConfig.ID);
		}
	}
}
