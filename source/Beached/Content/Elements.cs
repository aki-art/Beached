using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content
{
	public class Elements
	{
		public static readonly SimHashes
			amber = ToHash("Amber"),
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
			calcium = ToHash("Calcium"),
			moltenCalcium = ToHash("MoltenCalcium"),
			calciumGas = ToHash("CalciumGas"),
			carbonDioxideSnow = ToHash("CarbonDioxideSnow"),
			coquina = ToHash("Coquina"),
			crackedNeutronium = ToHash("CrackedNeutronium"),
			fuzz = ToHash("Fuzz"),
			galena = ToHash("Galena"),
			gravel = ToHash("Gravel"),
			iridium = ToHash("Iridium"),
			moltenIridium = ToHash("MoltenIridium"),
			iridiumGas = ToHash("IridiumGas"),
			latex = ToHash("Latex"),
			mucus = ToHash("Mucus"),
			mucusFrozen = ToHash("FrozenMucus"),
			murkyBrine = ToHash("MurkyBrine"),
			moss = ToHash("Moss"),
			nitrogen = ToHash("Nitrogen"),
			nitrogenFrozen = ToHash("FrozenNitrogen"),
			nitrogenLiquid = ToHash("LiquidNitrogen"),
			pearl = ToHash("Pearl"),
			rubber = ToHash("Rubber"),
			permaFrost = ToHash("PermaFrost"),
			rot = ToHash("Rot"),
			saltyOxygen = ToHash("SaltyOxygen"),
			silt = ToHash("Silt"),
			siltStone = ToHash("SiltStone"),
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

		// setting these when loaded
		public static SolidAmbienceType
			crystalAmbiance = 0;

		public static AmbienceType
			acidAmbience = 0;

		private static SimHashes ToHash(string name) => (SimHashes)Hash.SDBMLower($"Beached_{name}");

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
			{ mucusFrozen, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ pearl, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ permaFrost, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ rot, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
			{ SimHashes.SandStone, CONSTS.CORROSION_VULNERABILITY.VERY_REACTIVE },
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


		public static void AfterLoad()
		{
			SetAcidCorrosions();
			SetElectricalConductivities();

			foreach (var kvp in acidVulnerabilities)
			{
				var element = ElementLoader.FindElementByHash(kvp.Key);
				if (element != null)
				{
					element.oreTags = element.oreTags == null
						? ([BTags.corrodable])
						: element.oreTags.AddToArray(BTags.corrodable);
				}
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
				float conduction = 0;

				if (!electricConductiviy.ContainsKey(element.id))
				{
					if (element.hardness == byte.MaxValue)
						conduction = 0f;

					else if (element.HasTag(GameTags.RefinedMetal))
						conduction = 1f;

					else if (element.HasTag(GameTags.Metal))
						conduction = 0.8f;

					else if (element.IsLiquid)
						conduction = 0.5f;

					acidVulnerabilities[element.id] = conduction;
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

				// Chemical Processing
				"SolidPlatinum",
				"SolidSilver",

				// Rocketry Expanded
				"NeutroniumAlloy"
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

		// Eye irritation
		public static void SetExposureValues(Dictionary<SimHashes, float> customExposureRates)
		{
			customExposureRates[saltyOxygen] = CONSTS.EXPOSURE_EFFECT.COMFORTABLE;
			customExposureRates[nitrogen] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
			customExposureRates[mucus] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
			customExposureRates[murkyBrine] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
			customExposureRates[ammonia] = CONSTS.EXPOSURE_EFFECT.VERY_IRRITATING;
			customExposureRates[sulfurousWater] = CONSTS.EXPOSURE_EFFECT.OH_HECK_IT_BURNS;
		}

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
			woodLog.highTempTransition = ElementLoader.FindElementByHash(ash);

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
					Log.Debug($"- {def.name} {def.solidSounds.Length}");
					Elements.crystalAmbiance = (SolidAmbienceType)def.solidSounds.Length;
					var reference = def.solidSounds[(int)SolidAmbienceType.Ice];
					def.solidSounds = def.solidSounds.AddToArray(reference);
				}
			}

			var configs = new List<ElementsAudio.ElementAudioConfig>();

			var ice = elementsAudio.GetConfigForElement(SimHashes.Ice);
			var rawMetal = elementsAudio.GetConfigForElement(SimHashes.IronOre);
			var refinedMetal = elementsAudio.GetConfigForElement(SimHashes.Iron);
			var phosphate = elementsAudio.GetConfigForElement(SimHashes.PhosphateNodules);
			var clay = elementsAudio.GetConfigForElement(SimHashes.Clay);
			var rawRock = elementsAudio.GetConfigForElement(SimHashes.SandStone);

			configs.Add(ElementUtil.GetCrystalAudioConfig(amber));
			configs.Add(ElementUtil.CopyElementAudioConfig(ice, ammoniaFrozen));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Snow, permaFrost));
			configs.Add(ElementUtil.GetCrystalAudioConfig(aquamarine));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Sand, ash));
			configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, basalt));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, beryllium));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, bismuth));
			configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, bismuthOre));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, calcium));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.CrushedRock, gravel));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.OxyRock, zeolite));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, iridium));
			//configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Algae, moss));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.DirtyIce, mucusFrozen));
			configs.Add(ElementUtil.CopyElementAudioConfig(phosphate, pearl));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, zinc));
			configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, zincOre));
			configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, zirconium));
			configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, zirconiumOre));
			configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, siltStone));
			configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.DirtyWater, sulfurousWater));

			configs.Add(new ElementsAudio.ElementAudioConfig()
			{
				elementID = sulfurousWater,
				ambienceType = AmbienceType.None,
				solidAmbienceType = SolidAmbienceType.None,
				miningSound = null,
				miningBreakSound = null,
				oreBumpSound = null,
				floorEventAudioCategory = null,
				creatureChewSound = null
			});

			elementsAudio.elementAudioConfigs = elementsAudio.elementAudioConfigs.AddRangeToArray(configs.ToArray());
		}
	}
}
