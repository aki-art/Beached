using Klei.AI;

namespace Beached.Content
{
	public class BAmounts
	{
		public static Amount Moisture;
		public static Amount LimpetGrowth;

		[DbEntry]
		public static void Register(Database.Amounts __instance)
		{
			Moisture = __instance.CreateAmount(
				"Moisture",
				0f,
				100f,
				false,
				Units.Flat,
				0.35f,
				true,
				"STRINGS_OLD.CREATURES",
				"ui_icon_stamina",
				"attribute_stamina",
				"mod_stamina");

			Moisture.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

			LimpetGrowth = __instance.CreateAmount(
				"LimpetGrowth",
				0f,
				100f,
				true,
				Units.Flat,
				0.1675f,
				true,
				"STRINGS_OLD.CREATURES",
				"ui_icon_scale_growth");

			LimpetGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
		}
	}
}
