using Beached.Content.Defs.Entities.Corals;
using Beached.Utils.GlobalEvents;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content
{
	public class Elements
	{
		public static readonly SimHashes
			amber = ToHash("Amber"),
			ambergris = ToHash("Ambergris"),
			ammonia = ToHash("Ammonia"),
			ammoniaFrozen = ToHash("FrozenAmmonia"),
			ammoniaLiquid = ToHash("AmmoniaLiquid"),
			aquamarine = ToHash("Aquamarine"),
			ash = ToHash("Ash"),
			bambooStem = ToHash("BambooStem"),
			basalt = ToHash("Basalt"),
			beryllium = ToHash("Beryllium"),
			berylliumGas = ToHash("BerylliumGas"),
			berylliumMolten = ToHash("BerylliumMolten"),
			bismuth = ToHash("Bismuth"),
			bismuthGas = ToHash("BismuthGas"),
			bismuthMolten = ToHash("BismuthMolten"),
			bismuthOre = ToHash("Bismuthinite"),
			bone = ToHash("Bone"),
			brass = ToHash("Brass"),
			brassMolten = ToHash("BrassMolten"),
			brassGas = ToHash("BrassGas"),
			cactusJuice = ToHash("CactusJuice"),
			calcium = ToHash("Calcium"),
			moltenCalcium = ToHash("MoltenCalcium"),
			calciumGas = ToHash("CalciumGas"),
			carbonDioxideSnow = ToHash("CarbonDioxideSnow"),
			corallium = ToHash("Corallium"),
			coquina = ToHash("Coquina"),
			crackedNeutronium = ToHash("CrackedNeutronium"),
			fireMoss = ToHash("FireMoss"),
			fuzz = ToHash("Fuzz"),
			galena = ToHash("Galena"),
			gnawBerryJuice = ToHash("GnawBerryJuice"),
			gristleBerryJuice = ToHash("GristleBerryJuice"),
			gravel = ToHash("Gravel"),
			iridium = ToHash("Iridium"),
			moltenIridium = ToHash("MoltenIridium"),
			iridiumGas = ToHash("IridiumGas"),
			mucus = ToHash("Mucus"),
			mucusSolid = ToHash("SolidMucus"),
			murkyBrine = ToHash("MurkyBrine"),
			moss = ToHash("Moss"),
			nitrogen = ToHash("Nitrogen"),
			nitrogenFrozen = ToHash("FrozenNitrogen"),
			nitrogenLiquid = ToHash("LiquidNitrogen"),
			pearl = ToHash("Pearl"),
			rubber = ToHash("Rubber"),
			pallasite = ToHash("Pallasite"),
			perplexium = ToHash("Perplexium"),
			permaFrost = ToHash("PermaFrost"),
			permaFrost_Transitional = ToHash("PermaFrost_Transitional"),
			rot = ToHash("Rot"),
			saltyOxygen = ToHash("SaltyOxygen"),
			silt = ToHash("Silt"),
			siltStone = ToHash("SiltStone"),
			slag = ToHash("Slag"),
			slagGlass = ToHash("SlagGlass"),
			moltenSlagGlass = ToHash("MoltenSlagGlass"),
			sulfurousIce = ToHash("SulfurousIce"),
			sulfurousWater = ToHash("SulfurousWater"),
			zeolite = ToHash("Zeolite"),
			zincOre = ToHash("ZincOre"),
			zinc = ToHash("Zinc"),
			zincMolten = ToHash("ZincMolten"),
			zincGas = ToHash("ZincGas"),
			zirconium = ToHash("Zirconium"),
			zirconiumGas = ToHash("ZirconiumGas"),
			zirconiumMolten = ToHash("ZirconiumMolten"),
			zirconiumOre = ToHash("Zircon"),
			sourBrine = ToHash("SourBrine"),
			sourBrineIce = ToHash("SourBrineIce");

		public class Moonlet
		{
			public static readonly SimHashes
				coquina = ToHashMod("CrossRoads_Coquina"),
				paleOre = ToHashMod("CrossRoads_PaleOre");
		}

		public class ChemicalProcessing
		{
			public static readonly SimHashes
				ammoniaGas = ToHashMod("AmmoniaGas"),
				ammoniumSalt = ToHashMod("AmmoniumSalt"),
				ammoniumWater = ToHashMod("AmmoniumWater"),
				argentiteOre = ToHashMod("ArgentiteOre"),
				aurichalciteOre = ToHashMod("AurichalciteOre"),
				baseGradeSand = ToHashMod("BaseGradeSand"),
				carbonDioxide = ToHashMod("CarbonDioxide"),
				carbonFiber = ToHashMod("CarbonFiber"),
				chloroschist = ToHashMod("Chloroschist"),
				concreteBlock = ToHashMod("ConcreteBlock"),
				crudeOil = ToHashMod("CrudeOil"),
				galena = ToHashMod("Galena"),
				highGradeSand = ToHashMod("HighGradeSand"),
				isopropaneGas = ToHashMod("IsopropaneGas"),
				liquidAmmonia = ToHashMod("LiquidAmmonia"),
				liquidNitric = ToHashMod("LiquidNitric"),
				liquidSulfuric = ToHashMod("LiquidSulfuric"),
				lowGradeSand = ToHashMod("LowGradeSand"),
				meteorOre = ToHashMod("MeteorOre"),
				moltenCopper = ToHashMod("MoltenCopper"),
				moltenLead = ToHashMod("MoltenLead"),
				moltenSilver = ToHashMod("MoltenSilver"),
				moltenSlag = ToHashMod("MoltenSlag"),
				moltenSteel = ToHashMod("MoltenSteel"),
				moltenZinc = ToHashMod("MoltenZinc"),
				phosphorBronze = ToHashMod("PhosphorBronze"),
				plasteel = ToHashMod("Plasteel"),
				rawNaturalGas = ToHashMod("RawNaturalGas"),
				silverGas = ToHashMod("SilverGas"),
				solidAmmonia = ToHashMod("SolidAmmonia"),
				solidBorax = ToHashMod("SolidBorax"),
				solidBrass = ToHashMod("SolidBrass"),
				solidFiberglass = ToHashMod("SolidFiberglass"),
				solidOilShale = ToHashMod("SolidOilShale"),
				solidSilver = ToHashMod("SolidSilver"),
				solidSlag = ToHashMod("SolidSlag"),
				solidZinc = ToHashMod("SolidZinc"),
				sourGas = ToHashMod("SourGas"),
				sourWater = ToHashMod("SourWater"),
				sulfuricGas = ToHashMod("SulfuricGas"),
				toxicClay = ToHashMod("ToxicClay"),
				toxicGas = ToHashMod("ToxicGas"),
				toxicSlurry = ToHashMod("ToxicSlurry"),
				zincGas = ToHashMod("ZincGas");
		}

		public class RocketryExpanded
		{
			public static readonly SimHashes
				spaceStationForceField = ToHashMod("SpaceStationForceField"),
				unobtaniumAlloy = ToHashMod("UnobtaniumAlloy"),
				unobtaniumDust = ToHashMod("UnobtaniumDust");
		}

		public class Biochemistry
		{
			public static readonly SimHashes
				bioplastic = ToHashMod("Bioplastic"),
				liquidBiodiesel = ToHashMod("LiquidBiodiesel"),
				liquidVegeOil = ToHashMod("LiquidVegeOil"),
				solidBiodiesel = ToHashMod("SolidBiodiesel"),
				solidBiomass = ToHashMod("SolidBiomass"),
				solidVegeOil = ToHashMod("SolidVegeOil"),
				vegeOilGas = ToHashMod("VegeOilGas");
		}

		public class AETE
		{
			public static readonly SimHashes
				fakeLumber = ToHashMod("AETE_FakeLumber"),
				frozenHoney = ToHashMod("AETE_FrozenHoney"),
				frozenJello = ToHashMod("AETE_FrozenJello"),
				frozenPinkSlime = ToHashMod("AETE_FrozenPinkSlime"),
				honey = ToHashMod("AETE_Honey"),
				jello = ToHashMod("AETE_Jello"),
				pinkSlime = ToHashMod("AETE_PinkSlime");
		}

		public class Blood
		{
			public static readonly SimHashes
				blood = ToHashMod("Blood"),
				frozenBlood = ToHashMod("FrozenBlood");
		}

		public class TwitchMod
		{
			public static readonly SimHashes
				oniTwitchIndestructibleElement = ToHashMod("OniTwitchIndestructibleElement"),
				oniTwitchSuperInsulator = ToHashMod("OniTwitchSuperInsulator");
		}

		public class ChaosEvents
		{
			public static readonly SimHashes
				inverseWater = ToHashMod("ITCE_Inverse_Water"),
				inverseIce = ToHashMod("ITCE_Inverse_Ice"),
				inverseWaterPlaceholder = ToHashMod("ITCE_Inverse_Water_Placeholder"),
				inverseSteam = ToHashMod("ITCE_Inverse_Steam"),
				creepyLiquidGas = ToHashMod("ITCE_CreepyLiquidGas"),
				creepyLiquid = ToHashMod("ITCE_CreepyLiquid"),
				liquidPoop = ToHashMod("ITCE_Liquid_Poop"),
				voidLiquid = ToHashMod("ITCE_VoidLiquid");
		}

		// setting these when loaded
		public static SolidAmbienceType
			crystalAmbiance = 0;

		public static AmbienceType
			acidAmbience = 0;

		private static SimHashes ToHash(string name) => (SimHashes)Hash.SDBMLower($"Beached_{name}");

		private static SimHashes ToHashMod(string name) => (SimHashes)Hash.SDBMLower(name);

		public static readonly Dictionary<SimHashes, float> lubricantStrengths = new()
		{
			{ mucus, 3f },
			{ SimHashes.PhytoOil, 5f }
		};

		// everything not in this list will get MEDIUM (0.5f) assigned, except metals which get an INSTANTLY_MELT (1f)
		/// <see cref="ModAPI.SetElementAcidVulnerability(SimHashes, float)"/>
		public static readonly Dictionary<SimHashes, float> acidVulnerabilities = new()
		{
			// resistant elements
			{ SimHashes.Ceramic, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Clay, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Copper, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Cuprite, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ crackedNeutronium, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Electrum, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Glass, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Gold, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.GoldAmalgam, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Granite, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Katairite, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.SolidMercury, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE }, // has no metal tag for some reason
			{ SimHashes.Unobtanium, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Polypropylene, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ rubber, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Steel, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Sulfur, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ sulfurousIce, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Tungsten, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ SimHashes.Wolframite, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },
			{ zirconium, CONSTS.CORROSION_VULNERABILITY.NONREACTIVE },

			// very reactive
			{ SimHashes.Algae, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.BrineIce, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ bone, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ coquina, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Cement, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.CrushedIce, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.DirtyIce, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Fossil, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Fullerene, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Graphite, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Ice, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Lime, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			//{ moss, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ mucusSolid, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ pearl, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ permaFrost, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ rot, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.SandStone, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Sand, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.Snow, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ sourBrineIce, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
		};

		public static readonly Dictionary<SimHashes, float> electricConductiviy = new()
		{
			// perfect insulators
			{ SimHashes.Polypropylene, 0 },
			{ SimHashes.Unobtanium, 0 },
			{ SimHashes.SuperInsulator, 0 },
			{ SimHashes.Salt, 0 },
			{ SimHashes.Isoresin, 0 },
			{ SimHashes.SaltWater, 0.8f },
			{ SimHashes.Brine, 0.8f },
			{ rubber, 0 },
			{ sourBrine, 0.8f },
			{ crackedNeutronium, 0 },
		};

		public static readonly Dictionary<ushort, float> electricConductivityLookup = [];

		public static List<SimHashes> GetMetals()
		{
			return
			[
				bismuth,
				bismuthOre,
				zincOre,
				zinc,
				iridium,
				beryllium,
				calcium,
				zirconium,
				zirconiumOre
			];
		}


		public static string Name(SimHashes element) => Strings.Get($"STRINGS.ELEMENTS.{element.ToString().ToUpperInvariant()}.NAME");

		public static string Description(SimHashes element) => Strings.Get($"STRINGS.ELEMENTS.{element.ToString().ToUpperInvariant()}.DESCRIPTION");

		public static void AfterLoad()
		{
			SetAcidCorrosions();
			SetElectricalConductivities();

			ElementLoader.FindElementByHash(rot).sublimateFX = ModAssets.Fx.ammoniaBubbles;

			ElementLoader.FindElementByHash(SimHashes.Sand).AddTag(BTags.BuildingMaterials.sand);
			ElementLoader.FindElementByHash(SimHashes.Isoresin).AddTag(BTags.BuildingMaterials.rubber);

			foreach (var kvp in acidVulnerabilities)
				ElementLoader.FindElementByHash(kvp.Key)?.AddTag(BTags.corrodable);

			if (ElementLoader.FindElementByHash(ChaosEvents.inverseWater) != null)
			{
				var waters = CoralTemplate.ALL_WATERS.ToHashSet();
				waters.Add(ChaosEvents.inverseWater);
				CoralTemplate.ALL_WATERS = [.. waters];
			}
		}

		private static void SetConduction(SimHashes element, float conduction)
		{
			electricConductivityLookup[ElementLoader.GetElementIndex(element)] = conduction;
		}

		private static void SetElectricalConductivities()
		{
			foreach (var element in ElementLoader.elements)
			{
				if (!electricConductiviy.TryGetValue(element.id, out var conduction))
				{
					if (element.hardness == byte.MaxValue)
						conduction = 0f;

					else if (element.HasTag(GameTags.RefinedMetal))
						conduction = 1f;

					else if (element.HasTag(GameTags.Metal))
						conduction = 0.8f;

					else if (element.IsLiquid)
						conduction = 0.5f;
				}

				SetConduction(element.id, conduction);
			}
		}

		private static void SetAcidCorrosions()
		{
			var modsAcidResistantElements = new HashSet<string>()
			{
				"Platinum",
				"Silver",
				ChemicalProcessing.solidSilver.ToString(),
				RocketryExpanded.unobtaniumAlloy.ToString(),
				RocketryExpanded.spaceStationForceField.ToString(),
			};

			foreach (var element in ElementLoader.elements)
			{
				if (acidVulnerabilities.ContainsKey(element.id))
					continue;

				if (modsAcidResistantElements.Contains(element.id.ToString()))
					acidVulnerabilities[element.id] = CONSTS.CORROSION_VULNERABILITY.NONREACTIVE;

				else if (element.HasTag(GameTags.Metal) || element.HasTag(GameTags.RefinedMetal))
					acidVulnerabilities[element.id] = CONSTS.CORROSION_VULNERABILITY.INSTANTLY_MELT;
			}
		}


		[Subscribe(GlobalEvent.WORLD_RELOADED)]
		public static void OnWorldReload(bool isBeachedWorld)
		{
			var lime = ElementLoader.GetElement(SimHashes.Lime.CreateTag());
			var diamond = ElementLoader.GetElement(SimHashes.Diamond.CreateTag());
			var abyssalite = ElementLoader.GetElement(SimHashes.Katairite.CreateTag());
			var woodLog = ElementLoader.GetElement(SimHashes.WoodLog.CreateTag());

			if (isBeachedWorld || Mod.settings.CrossWorld.Elements.CrystalCategory)
			{
				diamond.materialCategory = BTags.MaterialCategories.crystal;
				abyssalite.materialCategory = BTags.MaterialCategories.crystal;
			}
			else
			{
				diamond.materialCategory = Mod.settings.CrossWorld.Elements.originalDiamondCategory;
				abyssalite.materialCategory = Mod.settings.CrossWorld.Elements.originalAbyssaliteCategory;
			}

			if (isBeachedWorld || Mod.settings.CrossWorld.Elements.LimeToCalcium)
			{
				lime.highTemp = 1115f;
				lime.highTempTransition = ElementLoader.FindElementByHash(calcium);
			}
			else
			{
				lime.highTemp = Mod.settings.CrossWorld.Elements.originalLimeHighTemp;
				lime.highTempTransition = ElementLoader.GetElement(Mod.settings.CrossWorld.Elements.originalLimeHighTempTarget);
			}

			// todo: settings specific
			//woodLog.highTempTransition = ElementLoader.FindElementByHash(ash);

			var substanceTable = Assets.instance.substanceTable;
			substanceTable.GetSubstance(SimHashes.Water).colour = ModAssets.Colors.water;
			substanceTable.GetSubstance(SimHashes.SaltWater).colour = ModAssets.Colors.saltWater;

			var dirt = substanceTable.GetSubstance(SimHashes.Dirt);
			ModAssets.Textures.dirtOriginal ??= dirt.material.mainTexture;

			dirt.material.mainTexture = isBeachedWorld
				? ModAssets.Textures.dirtLigher
				: ModAssets.Textures.dirtOriginal;
		}

		public static void CreateAudioConfigs(ElementsAudio elementsAudio)
		{
			var ambianceManager = Object.FindObjectOfType<AmbienceManager>();
			if (ambianceManager != null)
			{
				foreach (var def in ambianceManager.quadrantDefs)
				{
					crystalAmbiance = (SolidAmbienceType)def.solidSounds.Length;
					var reference = def.solidSounds[(int)SolidAmbienceType.Ice];
					def.solidSounds = def.solidSounds.AddToArray(reference);
				}
			}
		}
	}
}
