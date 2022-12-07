using Beached.Content.Defs.Items;
using Beached.Content.Scripts;
using Beached.Utils;
using System.Collections.Generic;

namespace Beached.Content
{
    public class Elements
    {
        public static ElementInfo Amber = ElementInfo.Solid("Amber", ModAssets.Colors.amber);
        public static ElementInfo Ammonia = ElementInfo.Gas("Ammonia", ModAssets.Colors.ammonia);
        public static ElementInfo AmmoniaFrozen = ElementInfo.Solid("FrozenAmmonia", ModAssets.Colors.ammonia);
        public static ElementInfo AmmoniaLiquid = ElementInfo.Liquid("AmmoniaLiquid", ModAssets.Colors.ammonia);
        public static ElementInfo Ash = ElementInfo.Solid("Ash", ModAssets.Colors.ash);
        public static ElementInfo Aquamarine = ElementInfo.Solid("Aquamarine", ModAssets.Colors.aquamarine);
        public static ElementInfo Basalt = ElementInfo.Solid("Basalt", ModAssets.Colors.basalt);
        public static ElementInfo Beryllium = ElementInfo.Solid("Beryllium", ModAssets.Colors.beryllium); // no ore, it comes from beryls
        public static ElementInfo BerylliumGas = ElementInfo.Gas("BerylliumGas", ModAssets.Colors.beryllium);
        public static ElementInfo BerylliumMolten = ElementInfo.Liquid("BerylliumMolten", ModAssets.Colors.beryllium);
        public static ElementInfo Bismuth = ElementInfo.Solid("Bismuth", ModAssets.Colors.bismuth);
        public static ElementInfo BismuthGas = ElementInfo.Gas("BismuthGas", ModAssets.Colors.bismuth);
        public static ElementInfo BismuthMolten = ElementInfo.Liquid("BismuthMolten", ModAssets.Colors.bismuth);
        public static ElementInfo BismuthOre = ElementInfo.Solid("Bismuthinite", ModAssets.Colors.bismuth);
        public static ElementInfo Bone = ElementInfo.Solid("Bone", ModAssets.Colors.bone);
        public static ElementInfo Calcium = ElementInfo.Solid("Calcium", ModAssets.Colors.calcium);
        public static ElementInfo CalciumGas = ElementInfo.Gas("CalciumGas", ModAssets.Colors.calcium);
        public static ElementInfo CalciumMolten = ElementInfo.Liquid("MoltenCalcium", ModAssets.Colors.calcium);
        public static ElementInfo Gravel = ElementInfo.Solid("Gravel", ModAssets.Colors.gravel);
        public static ElementInfo Heulandite = ElementInfo.Solid("Heulandite", ModAssets.Colors.zeolite);
        public static ElementInfo Iridium = ElementInfo.Solid("Iridium", ModAssets.Colors.iridium);
        public static ElementInfo IridiumGas = ElementInfo.Gas("IridiumGas", ModAssets.Colors.iridium);
        public static ElementInfo IridiumMolten = ElementInfo.Liquid("MoltenIridium", ModAssets.Colors.iridium);
        public static ElementInfo Moss = ElementInfo.Solid("Moss", ModAssets.Colors.moss);
        public static ElementInfo Mucus = ElementInfo.Liquid("Mucus", ModAssets.Colors.mucus);
        public static ElementInfo MucusFrozen = ElementInfo.Solid("FrozenMucus", ModAssets.Colors.mucus);
        public static ElementInfo MurkyBrine = ElementInfo.Liquid("MurkyBrine", ModAssets.Colors.murkyBrine);
        public static ElementInfo Mycelium = ElementInfo.Solid("Mycelium", ModAssets.Colors.mycelium);
        public static ElementInfo Nitrogen = ElementInfo.Gas("Nitrogen", ModAssets.Colors.nitrogen);
        public static ElementInfo NitrogenFrozen = ElementInfo.Solid("FrozenNitrogen", ModAssets.Colors.nitrogenOpaque);
        public static ElementInfo NitrogenLiquid = ElementInfo.Liquid("LiquidNitrogen", ModAssets.Colors.nitrogenOpaque);
        public static ElementInfo Pearl = ElementInfo.Solid("Pearl", ModAssets.Colors.pearl);
        public static ElementInfo Rot = ElementInfo.Solid("Rot", ModAssets.Colors.rot);
        public static ElementInfo Root = ElementInfo.Solid("Root", ModAssets.Colors.root);
        public static ElementInfo SaltyOxygen = ElementInfo.Gas("SaltyOxygen", ModAssets.Colors.saltyOxygen);
        public static ElementInfo Selenite = ElementInfo.Gas("Selenite", ModAssets.Colors.selenite);
        public static ElementInfo SiltStone = ElementInfo.Solid("SiltStone", ModAssets.Colors.ammonia);
        public static ElementInfo SulfurousIce = ElementInfo.Solid("SulfurousIce", ModAssets.Colors.sulfurousWater);
        public static ElementInfo SulfurousWater = ElementInfo.Liquid("SulfurousWater", ModAssets.Colors.sulfurousWater);
        public static ElementInfo Zirconium = ElementInfo.Solid("Zirconium", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumGas = ElementInfo.Gas("ZirconiumGas", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumMolten = ElementInfo.Liquid("ZirconiumMolten", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumOre = ElementInfo.Solid("Zircon", ModAssets.Colors.zirconium);
        public static ElementInfo Zinc = ElementInfo.Solid("Zinc", ModAssets.Colors.zinc);
        public static ElementInfo ZincGas = ElementInfo.Gas("ZincGas", ModAssets.Colors.zinc);
        public static ElementInfo ZincOre = ElementInfo.Solid("ZincOre", ModAssets.Colors.zinc);
        public static ElementInfo ZincMolten = ElementInfo.Liquid("ZincMolten", ModAssets.Colors.zinc);
        // maybe Emerald
        // maybe Coquina

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

            /*            var soundTable = Traverse.Create(GlobalAssets.Instance).Field<Dictionary<string, string>>("SoundTable").Value;
                        Log.Debug("&&&&&&&&&&&&&");
                        foreach(var asset in soundTable)
                        {
                            Log.Debug(asset.Key + " - " + asset.Value);
                        }
                        Log.Debug("&&&&&&&&&&&&&");*/

            return configs.ToArray();
        }

        public static void SetTreasureChances()
        {
            TreasureChances.AddTreasure(SimHashes.Diamond, 1f, new()
            {
                new TreasureChances.TreasureConfig(RareGemsConfig.FLAWLESS_DIAMOND, 1f, 1f, true)
            });

            TreasureChances.AddTreasure(Aquamarine, 1f, new()
            {
                new TreasureChances.TreasureConfig(RareGemsConfig.MAXIXE, 1f, 1f, true)
            });

            TreasureChances.AddTreasure(ZirconiumOre, 1f, new()
            {
                new TreasureChances.TreasureConfig(RareGemsConfig.HADEAN_ZIRCON, 1f, 1f, true)
            });
        }
    }
}
