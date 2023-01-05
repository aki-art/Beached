using Klei.AI;

namespace Beached.Content
{
    public class BAmounts
    {
        public static Amount Moisture;
        public static Amount LimpetGrowth;

        public static void Register(Database.Amounts amounts)
        {
            Moisture = amounts.CreateAmount(
                "Moisture",
                0f,
                100f,
                false,
                Units.Flat,
                0.35f,
                true,
                "STRINGS.CREATURES",
                "ui_icon_stamina",
                "attribute_stamina",
                "mod_stamina");

            Moisture.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

            LimpetGrowth = amounts.CreateAmount(
                "LimpetGrowth",
                0f,
                100f,
                true,
                Units.Flat,
                0.1675f,
                true,
                "STRINGS.CREATURES",
                "ui_icon_scale_growth");

            LimpetGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
        }
    }
}
