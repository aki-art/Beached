using Beached.Utils;

namespace Beached.Content.ModDb
{
    public class BEffects
    {
        // -5% stress/cycle, -10g Oxygen
        public const string OCEAN_BREEZE = "Beached_OceanBreeze";

        // +10% Stress/cycle, +5% Bladder delta
        public const string SCARED = "Beached_Scared";

        // idk yet
        public const string DIMWIT = "Beached_Dimwit";

        // no effect
        public const string LIMPETS_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery";

        // no effect
        public const string CAPPED_RECOVERY = "Beached_Capped_Recovery";

        // for critters, used for growing limpets
        public const string LIMPETHOST = "Beached_LimpetHost";

        // for critters, used for growing limpets
        public const string MUCUS_SOAKED = "Beached_MucusSoaked";

        public static void Register(ModifierSet set)
        {
            var stressDelta = Db.Get().Amounts.Stress.deltaAttribute.Id;
            var peeDelta = Db.Get().Amounts.Bladder.deltaAttribute.Id;
            var carryCapacity = Db.Get().Attributes.CarryAmount.Id;
            var airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Id;

            new EffectBuilder(OCEAN_BREEZE, 180f, false)
                .Modifier(airConsumptionRate, -0.01f)
                .Modifier(stressDelta, -5f / CONSTS.CYCLE_LENGTH)
                .Add(set);

            new EffectBuilder(SCARED, 2f, true)
                .Modifier(stressDelta, 10f / CONSTS.CYCLE_LENGTH)
                .Modifier(peeDelta, 5f / CONSTS.CYCLE_LENGTH)
                .Add(set);

            new EffectBuilder(MUCUS_SOAKED, 120f, true)
                .Modifier(carryCapacity, -0.5f, true)
                .Add(set);

            new EffectBuilder(DIMWIT, 2f, true)
                // TODO
                .Add(set);

            new EffectBuilder(LIMPETS_DUPLICANT_RECOVERY, 160f, false)
                .HideInUI()
                .HideFloatingText()
                .Add(set);

            new EffectBuilder(CAPPED_RECOVERY, 160f, false)
                .HideInUI()
                .HideFloatingText()
                .Add(set);

            /*            new EffectBuilder(LIMPETHOST, 0, false)
                            .Modifier(BAmounts.LimpetGrowth.maxAttribute.Id, 100f)
                            .Modifier(BAmounts.LimpetGrowth.deltaAttribute.Id, 5f / Consts.CYCLE_LENGTH * 100f)
                            .Modifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -250000 / Consts.CYCLE_LENGTH)
                            .Add(set);*/
        }
    }
}
