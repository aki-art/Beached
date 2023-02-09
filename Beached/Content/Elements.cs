using Beached.Content.Defs.Items;
using Beached.Content.Scripts;
using Beached.Utils;
using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Content
{
    public class Elements
    {
        public static ElementInfo
            Amber = ElementInfo.Solid("Amber", ModAssets.Colors.amber),
            Ammonia = ElementInfo.Gas("Ammonia", ModAssets.Colors.ammonia),
            AmmoniaFrozen = ElementInfo.Solid("FrozenAmmonia", ModAssets.Colors.ammonia),
            AmmoniaLiquid = ElementInfo.Liquid("AmmoniaLiquid", ModAssets.Colors.ammonia),
            Ash = ElementInfo.Solid("Ash", ModAssets.Colors.ash),
            Aquamarine = ElementInfo.Solid("Aquamarine", ModAssets.Colors.aquamarine),
            Basalt = ElementInfo.Solid("Basalt", ModAssets.Colors.basalt),
            Beryllium = ElementInfo.Solid("Beryllium", ModAssets.Colors.beryllium), // no ore, it comes from beryl,
            BerylliumGas = ElementInfo.Gas("BerylliumGas", ModAssets.Colors.beryllium),
            BerylliumMolten = ElementInfo.Liquid("BerylliumMolten", ModAssets.Colors.beryllium),
            Bismuth = ElementInfo.Solid("Bismuth", ModAssets.Colors.bismuth),
            BismuthGas = ElementInfo.Gas("BismuthGas", ModAssets.Colors.bismuth),
            BismuthMolten = ElementInfo.Liquid("BismuthMolten", ModAssets.Colors.bismuth),
            BismuthOre = ElementInfo.Solid("Bismuthinite", ModAssets.Colors.bismuth),
            Bone = ElementInfo.Solid("Bone", ModAssets.Colors.bone),
            Calcium = ElementInfo.Solid("Calcium", ModAssets.Colors.calcium),
            CalciumGas = ElementInfo.Gas("CalciumGas", ModAssets.Colors.calcium),
            CalciumMolten = ElementInfo.Liquid("MoltenCalcium", ModAssets.Colors.calcium),
            Gravel = ElementInfo.Solid("Gravel", ModAssets.Colors.gravel),
            Heulandite = ElementInfo.Solid("Heulandite", ModAssets.Colors.zeolite),
            Iridium = ElementInfo.Solid("Iridium", ModAssets.Colors.iridium),
            IridiumGas = ElementInfo.Gas("IridiumGas", ModAssets.Colors.iridium),
            IridiumMolten = ElementInfo.Liquid("MoltenIridium", ModAssets.Colors.iridium),
            Latex = ElementInfo.Liquid("Latex", ModAssets.Colors.latex),
            Litter = ElementInfo.Solid("Litter", ModAssets.Colors.iridium),
            Moss = ElementInfo.Solid("Moss", ModAssets.Colors.moss),
            Mucus = ElementInfo.Liquid("Mucus", ModAssets.Colors.mucus),
            MucusFrozen = ElementInfo.Solid("FrozenMucus", ModAssets.Colors.mucus),
            MurkyBrine = ElementInfo.Liquid("MurkyBrine", ModAssets.Colors.murkyBrine),
            Mycelium = ElementInfo.Solid("Mycelium", ModAssets.Colors.mycelium),
            Nitrogen = ElementInfo.Gas("Nitrogen", ModAssets.Colors.nitrogen),
            NitrogenFrozen = ElementInfo.Solid("FrozenNitrogen", ModAssets.Colors.nitrogenOpaque),
            NitrogenLiquid = ElementInfo.Liquid("LiquidNitrogen", ModAssets.Colors.nitrogenOpaque),
            Pearl = ElementInfo.Solid("Pearl", ModAssets.Colors.pearl),
            PermaFrost = ElementInfo.Solid("PermaFrost", ModAssets.Colors.pearl),
            Rot = ElementInfo.Solid("Rot", ModAssets.Colors.rot),
            Root = ElementInfo.Solid("Root", ModAssets.Colors.root),
            Rubber = ElementInfo.Solid("Rubber", ModAssets.Colors.root),
            SaltyOxygen = ElementInfo.Gas("SaltyOxygen", ModAssets.Colors.saltyOxygen),
            Selenite = ElementInfo.Gas("Selenite", ModAssets.Colors.selenite),
            SiltStone = ElementInfo.Solid("SiltStone", ModAssets.Colors.ammonia),
            SulfurousIce = ElementInfo.Solid("SulfurousIce", ModAssets.Colors.sulfurousWater),
            SulfurousWater = ElementInfo.Liquid("SulfurousWater", ModAssets.Colors.sulfurousWater),
            Zirconium = ElementInfo.Solid("Zirconium", ModAssets.Colors.zirconium),
            ZirconiumGas = ElementInfo.Gas("ZirconiumGas", ModAssets.Colors.zirconium),
            ZirconiumMolten = ElementInfo.Liquid("ZirconiumMolten", ModAssets.Colors.zirconium),
            ZirconiumOre = ElementInfo.Solid("Zircon", ModAssets.Colors.zirconium),
            Zinc = ElementInfo.Solid("Zinc", ModAssets.Colors.zinc),
            ZincGas = ElementInfo.Gas("ZincGas", ModAssets.Colors.zinc),
            ZincOre = ElementInfo.Solid("ZincOre", ModAssets.Colors.zinc),
            ZincMolten = ElementInfo.Liquid("ZincMolten", ModAssets.Colors.zinc);
        // maybe Emerald
        // maybe Coquina

        public static Dictionary<SimHashes, float> corrosionData;

        public static void RegisterSubstances(List<Substance> list)
        {
            var ore = list.Find(e => e.elementID == SimHashes.Cuprite).material;
            var refined = list.Find(e => e.elementID == SimHashes.Copper).material;
            var gem = list.Find(e => e.elementID == SimHashes.Diamond).material;

            // Add the ones with some special attribute
            var newElements = new HashSet<Substance>()
            {
                Amber.CreateSubstance(true, gem, specularColor: Util.ColorFromHex("ffc000"), normal: "amber_normal"),
                Aquamarine.CreateSubstance(true, gem),
                Beryllium.CreateSubstance(true, refined),
                Bismuth.CreateSubstance(true, refined),
                BismuthOre.CreateSubstance(true, ore),
                Calcium.CreateSubstance(true, refined),
                Nitrogen.CreateSubstance(ModAssets.Colors.nitrogenOpaque, ModAssets.Colors.nitrogenOpaque),
                Pearl.CreateSubstance(true, gem, normal: "pearl_normal"),
                SaltyOxygen.CreateSubstance(ModAssets.Colors.mucusUi, ModAssets.Colors.mucusConduit),
                Selenite.CreateSubstance(true, gem),
                Zirconium.CreateSubstance(true, refined, specularColor: ModAssets.Colors.zirconSpecular),
                ZirconiumOre.CreateSubstance(true, ore, specularColor: ModAssets.Colors.zirconSpecular),
                Zinc.CreateSubstance(true, refined, specularColor: ModAssets.Colors.zincSpecular),
                ZincOre.CreateSubstance(true, ore, specularColor: ModAssets.Colors.zincSpecular),
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
            SetTreasureChances();
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
                        element.oreTags = new Tag[] { BTags.Corrodable };
                    }
                    else
                    {
                        element.oreTags = element.oreTags.AddToArray(BTags.Corrodable);
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
            Rottable.AtmosphereModifier.Add((int)Ammonia.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)AmmoniaLiquid.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)Mucus.SimHash, Rottable.RotAtmosphereQuality.Contaminating);
        }

        // Eye irritation
        public static void SetExposureValues(Dictionary<SimHashes, float> customExposureRates)
        {
            customExposureRates[SaltyOxygen] = CONSTS.EXPOSURE_EFFECT.COMFORTABLE;
            customExposureRates[Nitrogen] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
            customExposureRates[Mucus] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
            customExposureRates[MurkyBrine] = CONSTS.EXPOSURE_EFFECT.NEUTRAL;
            customExposureRates[Ammonia] = CONSTS.EXPOSURE_EFFECT.VERY_IRRITATING;
            customExposureRates[SulfurousWater] = CONSTS.EXPOSURE_EFFECT.OH_HECK_IT_BURNS;
        }

        // Decor and Overheat modifiers
        public static void AddAttributeModifiers()
        {
            ElementUtil.AddModifier(Amber.Get(), 1f, -20f);
            ElementUtil.AddModifier(Aquamarine.Get(), 1f, 0);
            ElementUtil.AddModifier(Basalt.Get(), 0.1f, 50);
            ElementUtil.AddModifier(Bismuth.Get(), 0.25f, -10);
            ElementUtil.AddModifier(BismuthOre.Get(), 0.25f, -10);
            ElementUtil.AddModifier(Heulandite.Get(), 0.25f, 0);
            ElementUtil.AddModifier(Pearl.Get(), 1.5f, 0);
            ElementUtil.AddModifier(Selenite.Get(), 1f, 400);
            ElementUtil.AddModifier(Zirconium.Get(), 0.4f, 150);
            ElementUtil.AddModifier(ZirconiumOre.Get(), 0.4f, 150);
        }

        public static void OnWorldReload(bool isBeachedWorld)
        {
            var lime = ElementLoader.GetElement(SimHashes.Lime.CreateTag());
            var diamond = ElementLoader.GetElement(SimHashes.Diamond.CreateTag());
            var abyssalite = ElementLoader.GetElement(SimHashes.Katairite.CreateTag());

            if (isBeachedWorld || Mod.Settings.CrossWorld.Elements.CrystalCategory)
            {
                diamond.materialCategory = BTags.MaterialCategories.Crystal;
                abyssalite.materialCategory = BTags.MaterialCategories.Crystal;
            }
            else
            {
                diamond.materialCategory = Mod.Settings.CrossWorld.Elements.originalDiamondCategory;
                abyssalite.materialCategory = Mod.Settings.CrossWorld.Elements.originalAbyssaliteCategory;
            }

            if (isBeachedWorld || Mod.Settings.CrossWorld.Elements.LimeToCalcium)
            {
                lime.highTemp = 1115f;
                lime.highTempTransition = Calcium.Get();
            }
            else
            {
                lime.highTemp = Mod.Settings.CrossWorld.Elements.originalLimeHighTemp;
                lime.highTempTransition = ElementLoader.GetElement(Mod.Settings.CrossWorld.Elements.originalLimeHighTempTarget);
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

            configs.Add(ElementUtil.GetCrystalAudioConfig(Amber));
            configs.Add(ElementUtil.CopyElementAudioConfig(ice, AmmoniaFrozen));
            configs.Add(ElementUtil.CopyElementAudioConfig(ice, PermaFrost));
            configs.Add(ElementUtil.GetCrystalAudioConfig(Aquamarine));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Sand, Ash));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, Basalt));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Beryllium));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Bismuth));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, BismuthOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Calcium));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.CrushedRock, Gravel));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.OxyRock, Heulandite));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Iridium));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.Algae, Moss));
            configs.Add(ElementUtil.CopyElementAudioConfig(SimHashes.DirtyIce, MucusFrozen));
            configs.Add(ElementUtil.CopyElementAudioConfig(clay, Mycelium));
            configs.Add(ElementUtil.CopyElementAudioConfig(phosphate, Pearl));
            configs.Add(ElementUtil.GetCrystalAudioConfig(Selenite));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Zinc));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, ZincOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(refinedMetal, Zirconium));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawMetal, ZirconiumOre));
            configs.Add(ElementUtil.CopyElementAudioConfig(rawRock, SiltStone));

            return configs.ToArray();
        }

        public static void SetTreasureChances()
        {
            TreasureChances.AddSingle(ZirconiumOre, 0.3f, RareGemsConfig.HADEAN_ZIRCON, 1f, true);

            TreasureChances.AddTreasure(SimHashes.Diamond, 1f, new()
            {
                new TreasureChances.TreasureConfig(RareGemsConfig.FLAWLESS_DIAMOND, 1f, 1f, true)
            });

            TreasureChances.AddTreasure(Aquamarine, 1f, new()
            {
                new TreasureChances.TreasureConfig(RareGemsConfig.MAXIXE, 1f, 1f, true)
            });
        }
    }
}
