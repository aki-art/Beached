using Beached.Utils;
using System;
using System.Collections.Generic;

namespace Beached.Content
{
    public class Elements
    {
        public static List<ElementInfo> elements = new List<ElementInfo>();

        public static ElementInfo Amber = ElementInfo.Solid("Amber", ModAssets.Colors.amber);
        public static ElementInfo Ammonia = ElementInfo.Gas("Ammonia", ModAssets.Colors.ammonia);
        public static ElementInfo AmmoniaFrozen = ElementInfo.Solid("AmmoniaFrozen", ModAssets.Colors.ammonia);
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
        public static ElementInfo BismuthOre = ElementInfo.Solid("BismuthOre", ModAssets.Colors.bismuth);
        public static ElementInfo Calcium = ElementInfo.Solid("Calcium", ModAssets.Colors.calcium);
        public static ElementInfo CalciumGas = ElementInfo.Gas("CalciumGas", ModAssets.Colors.calcium);
        public static ElementInfo CalciumMolten = ElementInfo.Liquid("CalciumMolten", ModAssets.Colors.calcium);
        public static ElementInfo CalciumOre = ElementInfo.Solid("CalciumOre", ModAssets.Colors.calcium);
        public static ElementInfo Gravel = ElementInfo.Solid("Gravel", ModAssets.Colors.gravel);
        public static ElementInfo Heulandite = ElementInfo.Solid("Heulandite", ModAssets.Colors.zeolite);
        public static ElementInfo Mucus = ElementInfo.Liquid("Mucus", ModAssets.Colors.mucus);
        public static ElementInfo MucusFrozen = ElementInfo.Solid("MucusFrozen", ModAssets.Colors.mucus);
        public static ElementInfo MurkyBrine = ElementInfo.Liquid("MurkyBrine", ModAssets.Colors.murkyBrine);
        public static ElementInfo Mycelium = ElementInfo.Solid("Mycelium", ModAssets.Colors.mycelium);
        public static ElementInfo Root = ElementInfo.Solid("Root", ModAssets.Colors.root);
        public static ElementInfo SaltyOxygen = ElementInfo.Gas("SaltyOxygen", ModAssets.Colors.saltyOxygen);
        public static ElementInfo Selenite = ElementInfo.Gas("Selenite", ModAssets.Colors.selenite);
        public static ElementInfo SiltStone = ElementInfo.Solid("SiltStone", ModAssets.Colors.ammonia);
        public static ElementInfo SulfurousIce = ElementInfo.Solid("SulfurousIce", ModAssets.Colors.sulfurousWater);
        public static ElementInfo SulfurousWater = ElementInfo.Liquid("SulfurousWater", ModAssets.Colors.sulfurousWater);
        public static ElementInfo Zirconium = ElementInfo.Solid("Zirconium", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumGas = ElementInfo.Gas("ZirconiumGas", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumMolten = ElementInfo.Liquid("ZirconiumMolten", ModAssets.Colors.zirconium);
        public static ElementInfo ZirconiumOre = ElementInfo.Solid("ZirconiumOre", ModAssets.Colors.zirconium);
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

            list.AddRange(new List<Substance>() 
            {
                Amber.CreateSubstance(true, gem, specularColor: Util.ColorFromHex("ffc000"), normal: "amber_normal"),
                Ammonia.CreateSubstance(),
                AmmoniaFrozen.CreateSubstance(),
                AmmoniaLiquid.CreateSubstance(),
                Ash.CreateSubstance(),
                Aquamarine.CreateSubstance(true, gem),
                Basalt.CreateSubstance(),
                Beryllium.CreateSubstance(true, refined),
                BerylliumGas.CreateSubstance(),
                BerylliumMolten.CreateSubstance(),
                Bismuth.CreateSubstance(true, refined),
                BismuthGas.CreateSubstance(),
                BismuthMolten.CreateSubstance(),
                BismuthOre.CreateSubstance(true, ore),
                Calcium.CreateSubstance(true, refined),
                CalciumGas.CreateSubstance(),
                CalciumMolten.CreateSubstance(),
                CalciumOre.CreateSubstance(true, ore),
                Gravel.CreateSubstance(),
                Heulandite.CreateSubstance(),
                Mucus.CreateSubstance(),
                MucusFrozen.CreateSubstance(),
                MurkyBrine.CreateSubstance(),
                Mycelium.CreateSubstance(),
                Root.CreateSubstance(),
                SaltyOxygen.CreateSubstance(ModAssets.Colors.mucusUi, ModAssets.Colors.mucusConduit),
                Selenite.CreateSubstance(true, gem),
                SiltStone.CreateSubstance(),
                SulfurousIce.CreateSubstance(),
                SulfurousWater.CreateSubstance(),
                Zirconium.CreateSubstance(true, refined),
                ZirconiumGas.CreateSubstance(),
                ZirconiumMolten.CreateSubstance(),
                ZirconiumOre.CreateSubstance(true, ore, specularColor: ModAssets.Colors.zirconSpecular),
                Zinc.CreateSubstance(true, refined),
                ZincGas.CreateSubstance(),
                ZincOre.CreateSubstance(true, ore),
                ZincMolten.CreateSubstance(),
            });

            Rottable.AtmosphereModifier.Add((int)Ammonia.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)AmmoniaLiquid.SimHash, Rottable.RotAtmosphereQuality.Sterilizing);
            Rottable.AtmosphereModifier.Add((int)Mucus.SimHash, Rottable.RotAtmosphereQuality.Contaminating);
        }

        // Eye irritation
        public static void SetExposureValues(Dictionary<SimHashes, float> customExposureRates)
        {
            customExposureRates[SaltyOxygen] = Consts.EXPOSURE_EFFECT.COMFORTABLE;
            customExposureRates[Mucus] = Consts.EXPOSURE_EFFECT.NEUTRAL;
            customExposureRates[MurkyBrine] = Consts.EXPOSURE_EFFECT.NEUTRAL;
            customExposureRates[Ammonia] = Consts.EXPOSURE_EFFECT.VERY_IRRITATING;
            customExposureRates[SulfurousWater] = Consts.EXPOSURE_EFFECT.OH_HECK_IT_BURNS;
        }
    }
}
