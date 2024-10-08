using Klei.AI;

namespace Beached.Content.ModDb
{
	public class BAmounts
	{
		public static Amount Moisture;
		public static Amount LimpetGrowth;
		public static Amount Wet;
		public static Amount ShellGrowth; // todo: ExtendEntityToWildCreature
		public static Amount ShellIntegrity;

		[DbEntry]
		public static void Register(Database.Amounts __instance)
		{
			Moisture = __instance.CreateAmount(
				"Beached_Moisture",
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

			LimpetGrowth = __instance.CreateAmount(
				"Beached_LimpetGrowth",
				0f,
				100f,
				true,
				Units.Flat,
				0,
				true,
				"STRINGS.CREATURES",
				"ui_icon_scale_growth");

			LimpetGrowth.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));

			Wet = __instance.CreateAmount(
				"Beached_Wet",
				0f,
				100f,
				true,
				Units.Flat,
				0.1675f,
				true,
				"STRINGS.DUPLICANTS",
				"beached_amount_wet");

			Wet.SetDisplayer(new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));

			ShellGrowth = __instance.CreateAmount(
					"Beached_ShellGrowth",
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

			ShellGrowth.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

			ShellIntegrity = __instance.CreateAmount(
						"Beached_ShellIntegrity",
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

			ShellIntegrity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));
		}
	}
}
