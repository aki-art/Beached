using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content
{
    public class Elements
    {
        public static ElementInfo
            amber = ElementInfo.Solid("Amber", ModAssets.Colors.amber),
            ammonia = ElementInfo.Gas("Ammonia", ModAssets.Colors.ammonia),
            ammoniaFrozen = ElementInfo.Solid("FrozenAmmonia", ModAssets.Colors.ammonia),
            ammoniaLiquid = ElementInfo.Liquid("AmmoniaLiquid", ModAssets.Colors.ammonia),
            ash = ElementInfo.Solid("Ash", ModAssets.Colors.ash),
            aquamarine = ElementInfo.Solid("Aquamarine", ModAssets.Colors.aquamarine),
            basalt = ElementInfo.Solid("Basalt", ModAssets.Colors.basalt),
            beryllium = ElementInfo.Solid("Beryllium", ModAssets.Colors.beryllium), // no ore, it comes from beryl,
            berylliumGas = ElementInfo.Gas("BerylliumGas", ModAssets.Colors.beryllium),
            berylliumMolten = ElementInfo.Liquid("BerylliumMolten", ModAssets.Colors.beryllium),
            bismuth = ElementInfo.Solid("Bismuth", ModAssets.Colors.bismuth),
            bismuthGas = ElementInfo.Gas("BismuthGas", ModAssets.Colors.bismuth),
            bismuthMolten = ElementInfo.Liquid("BismuthMolten", ModAssets.Colors.bismuth),
            bismuthOre = ElementInfo.Solid("Bismuthinite", ModAssets.Colors.bismuth),
            bone = ElementInfo.Solid("Bone", ModAssets.Colors.bone),
            calcium = ElementInfo.Solid("Calcium", ModAssets.Colors.calcium),
            calciumGas = ElementInfo.Gas("CalciumGas", ModAssets.Colors.calcium),
            calciumMolten = ElementInfo.Liquid("MoltenCalcium", ModAssets.Colors.calcium),
            coquina = ElementInfo.Solid("Coquina", ModAssets.Colors.coquina),
            crackedNeutronium = ElementInfo.Solid("CrackedNeutronium", Color.black),
            gravel = ElementInfo.Solid("Gravel", ModAssets.Colors.gravel),
            heulandite = ElementInfo.Solid("Heulandite", ModAssets.Colors.zeolite),
            iridium = ElementInfo.Solid("Iridium", ModAssets.Colors.iridium),
            iridiumGas = ElementInfo.Gas("IridiumGas", ModAssets.Colors.iridium),
            iridiumMolten = ElementInfo.Liquid("MoltenIridium", ModAssets.Colors.iridium),
            latex = ElementInfo.Liquid("Latex", ModAssets.Colors.latex),
            litter = ElementInfo.Solid("Litter", ModAssets.Colors.iridium),
            metamorphicRock = ElementInfo.Solid("MetamorphicRock", ModAssets.Colors.moss),
            moss = ElementInfo.Solid("Moss", ModAssets.Colors.moss),
            mucus = ElementInfo.Liquid("Mucus", ModAssets.Colors.mucus),
            mucusFrozen = ElementInfo.Solid("FrozenMucus", ModAssets.Colors.mucus),
            murkyBrine = ElementInfo.Liquid("MurkyBrine", ModAssets.Colors.murkyBrine),
            mycelium = ElementInfo.Solid("Mycelium", ModAssets.Colors.mycelium),
            nitrogen = ElementInfo.Gas("Nitrogen", ModAssets.Colors.nitrogen),
            nitrogenFrozen = ElementInfo.Solid("FrozenNitrogen", ModAssets.Colors.nitrogenOpaque),
            nitrogenLiquid = ElementInfo.Liquid("LiquidNitrogen", ModAssets.Colors.nitrogenOpaque),
            pearl = ElementInfo.Solid("Pearl", ModAssets.Colors.pearl),
            permaFrost = ElementInfo.Solid("PermaFrost", ModAssets.Colors.pearl),
            rot = ElementInfo.Solid("Rot", ModAssets.Colors.rot),
            root = ElementInfo.Solid("Root", ModAssets.Colors.root),
            rubber = ElementInfo.Solid("Rubber", ModAssets.Colors.root),
            saltyOxygen = ElementInfo.Gas("SaltyOxygen", ModAssets.Colors.saltyOxygen),
            selenite = ElementInfo.Gas("Selenite", ModAssets.Colors.selenite),
            siltStone = ElementInfo.Solid("SiltStone", ModAssets.Colors.ammonia),
            sourBrine = ElementInfo.Liquid("SourBrine", ModAssets.Colors.sourBrine),
            sourBrineIce = ElementInfo.Solid("SourBrineIce", ModAssets.Colors.sourBrine),
            sulfurousIce = ElementInfo.Solid("SulfurousIce", ModAssets.Colors.sulfurousWater),
            sulfurousWater = ElementInfo.Liquid("SulfurousWater", ModAssets.Colors.sulfurousWater),
            zirconium = ElementInfo.Solid("Zirconium", ModAssets.Colors.zirconium),
            zirconiumGas = ElementInfo.Gas("ZirconiumGas", ModAssets.Colors.zirconium),
            zirconiumMolten = ElementInfo.Liquid("ZirconiumMolten", ModAssets.Colors.zirconium),
            zirconiumOre = ElementInfo.Solid("Zircon", ModAssets.Colors.zirconium),
            zinc = ElementInfo.Solid("Zinc", ModAssets.Colors.zinc),
            zincGas = ElementInfo.Gas("ZincGas", ModAssets.Colors.zinc),
            zincOre = ElementInfo.Solid("ZincOre", ModAssets.Colors.zinc),
            zincMolten = ElementInfo.Liquid("ZincMolten", ModAssets.Colors.zinc);

#if ELEMENTS
        public static Dictionary<SimHashes, float> corrosionData;

        public static List<SimHashes> GetMetals()
        {
            return new List<SimHashes>() {
                bismuth,
                bismuthOre,
                zincOre,
                zinc,
                iridium,
                beryllium,
                calcium,
                zirconium,
                zirconiumOre };
        }

        public static void RegisterSubstances(List<Substance> list)
        {
            var ore = list.Find(e => e.elementID == SimHashes.Cuprite).material;
            var refined = list.Find(e => e.elementID == SimHashes.Copper).material;
            var gem = list.Find(e => e.elementID == SimHashes.Diamond).material;

            var iridiumSubstance = iridium.CreateSubstance(true, refined);
            iridiumSubstance.material.SetFloat("_WorldUVScale", 10f);

            // Add the ones with some special attribute
            var newElements = new HashSet<Substance>()
            {
                amber.CreateSubstance(true, gem, specularColor: Util.ColorFromHex("ffc000"), normal: "amber_normal"),
                aquamarine.CreateSubstance(true, gem),
                beryllium.CreateSubstance(true, refined),
                bismuth.CreateSubstance(true, refined),
                bismuthOre.CreateSubstance(true, ore),
                calcium.CreateSubstance(true, refined),
                iridiumSubstance,
                nitrogen.CreateSubstance(ModAssets.Colors.nitrogenOpaque, ModAssets.Colors.nitrogenOpaque),
                pearl.CreateSubstance(true, gem, normal: "pearl_normal"),
                saltyOxygen.CreateSubstance(ModAssets.Colors.mucusUi, ModAssets.Colors.mucusConduit),
                selenite.CreateSubstance(true, gem),
                zirconium.CreateSubstance(true, refined, specularColor: ModAssets.Colors.zirconSpecular),
                zirconiumOre.CreateSubstance(true, ore, specularColor: ModAssets.Colors.zirconSpecular),
                zinc.CreateSubstance(true, refined, specularColor: ModAssets.Colors.zincSpecular),
                zincOre.CreateSubstance(true, ore, specularColor: ModAssets.Colors.zincSpecular),
            };

            // Dump the rest in
            foreach (var element in ElementUtil.elements)
            {
                if (!element.isInitialized)
                {
                    newElements.Add(element.CreateSubstance());
                }
            }

            list.AddRange(newElements);

            SetAtmosphereModifiers();
            SetCorrosionData();
        }

        internal static void AfterLoad()
        {
            foreach (var kvp in corrosionData)
            {
                var element = ElementLoader.FindElementByHash(kvp.Key);
                if (element != null)
                {
                    if (element.oreTags == null)
                    {
                        element.oreTags = new Tag[] { BTags.corrodable };
                    }
                    else
                    {
                        element.oreTags = element.oreTags.AddToArray(BTags.corrodable);
                    }
                }
            }
        }

        private static void SetCorrosionData()
        {
            corrosionData = new Dictionary<SimHashes, float>()
            {
                { SimHashes.Algae, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.Aluminum, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.AluminumOre, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.SandStone, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.Sand, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.Salt, CONSTS.CORROSION_VULNERABILITY.WEAK },
                { SimHashes.Obsidian, CONSTS.CORROSION_VULNERABILITY.STRONG },
            };
        }

        // Food sterilization/rotting modifier
        private static void SetAtmosphereModifiers()
        {
            Rottable.AtmosphereModifier.Add((int)ammonia.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)ammoniaLiquid.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)mucus.SimHash, Rottable.RotAtmosphereQuality.Contaminating);
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

        // Decor and Overheat modifiers
        public static void AddAttributeModifiers()
        {
            ElementUtil.AddModifier(amber.Get(), 1f, -20f);
            ElementUtil.AddModifier(aquamarine.Get(), 1f, 0);
            ElementUtil.AddModifier(basalt.Get(), 0.1f, 50);
            ElementUtil.AddModifier(bismuth.Get(), 0.25f, -10);
            ElementUtil.AddModifier(bismuthOre.Get(), 0.25f, -10);
            ElementUtil.AddModifier(heulandite.Get(), 0.25f, 0);
            ElementUtil.AddModifier(pearl.Get(), 1.5f, 0);
            ElementUtil.AddModifier(selenite.Get(), 1f, 400);
            ElementUtil.AddModifier(zirconium.Get(), 0.4f, 150);
            ElementUtil.AddModifier(zirconiumOre.Get(), 0.4f, 150);
        }

        public static void OnWorldReload(bool isBeachedWorld)
        {
            var lime = ElementLoader.GetElement(SimHashes.Lime.CreateTag());
            var diamond = ElementLoader.GetElement(SimHashes.Diamond.CreateTag());
            var abyssalite = ElementLoader.GetElement(SimHashes.Katairite.CreateTag());

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
                lime.highTempTransition = calcium.Get();
            }
            else
            {
                lime.highTemp = Mod.settings.CrossWorld.Elements.originalLimeHighTemp;
                lime.highTempTransition = ElementLoader.GetElement(Mod.settings.CrossWorld.Elements.originalLimeHighTempTarget);
            }
        }

        public static ElementsAudio.ElementAudioConfig[] CreateAudioConfigs(ElementsAudio instance)
        {
            var configs = new List<ElementsAudio.ElementAudioConfig>();

            var ice = instance.GetConfigForElement(SimHashes.Ice);
            var rawMetal = instance.GetConfigForElement(SimHashes.IronOre);
            var refinedMetal = instance.GetConfigForElement(SimHashes.Iron);
            var phosphate = instance.GetConfigForElement(SimHashes.PhosphateNodules);
            var clay = instance.GetConfigForElement(SimHashes.Clay);
            var rawRock = instance.GetConfigForElement(SimHashes.SandStone);

            configs.Add(ElementUtil.GetCrystalAudioConfig(amber));
            configs.Add(ElementUtil.CopyElementAudioConfig(ice, ammoniaFrozen));
            configs.Add(ElementUtil.CopyElementAudioConfig(ice, permaFrost));
            configs.Add(ElementUtil.GetCrystalAudioConfig(aquamarine));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Sand, ash));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, basalt));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, beryllium));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, bismuth));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, bismuthOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, calcium));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.CrushedRock, gravel));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.OxyRock, heulandite));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, iridium));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Algae, moss));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.DirtyIce, mucusFrozen));
            configs.Add(ElementUtil.CopyElementAudioConfig(clay, mycelium));
            configs.Add(ElementUtil.CopyElementAudioConfig(phosphate, pearl));
            configs.Add(ElementUtil.GetCrystalAudioConfig(selenite));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, zinc));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, zincOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, zirconium));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, zirconiumOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, siltStone));

            return configs.ToArray();
        }
#endif
    }
}
