using Klei.AI;

namespace Beached.Content.ModDb
{
	public class BAmounts
	{
		public static Amount Moisture;
		public static Amount LimpetGrowth;
		public static Amount ShellGrowth; // todo: ExtendEntityToWildCreature
		public static Amount ShellIntegrity;
		public static Amount Mucus;
		public static Amount CrystalGrowth;

		[DbEntry]
		public static void Register(Database.Amounts __instance)
		{
			CrystalGrowth = __instance.CreateAmount(
				"Beached_CrystalGrowth",
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

			CrystalGrowth.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

			Moisture = __instance.CreateAmount(
				"Beached_Moisture",
				0f,
				100f,
				true,
				Units.Flat,
				0.35f,
				true,
				"STRINGS.CREATURES",
				"ui_icon_stamina",
				"attribute_stamina",
				"mod_stamina");

			Moisture.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

			Mucus = __instance.CreateAmount(
				"Beached_Mucus",
				0f,
				10f,
				false,
				Units.Flat,
				0.35f,
				true,
				"STRINGS.CREATURES",
				"ui_icon_stamina",
				"attribute_stamina",
				"mod_stamina");

			Mucus.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerCycle, null));

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
