namespace Beached.Content.ModDb
{
	public class BEffects
	{
		public const string
			DAMP_PLANTGROWTH = "Beached_Damp_PlantGrowthBoost",
			ARID_PLANTGROWTH = "Beached_Arid_PlantGrowthPenalty",
			OCEAN_BREEZE = "Beached_OceanBreeze", // -5% stress/cycle, -10g Oxygen
			SCARED = "Beached_Scared", // +10% Stress/cycle, +5% Bladder delta
			SCARED_SIREN = "Beached_Scared_Siren", // +10% Stress/cycle, +5% Bladder delta
			DIMWIT = "Beached_Dimwit",
			LIMPETS_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			ICEWRATH_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			CAPPED_RECOVERY = "Beached_Capped_Recovery",
			POFFMOUTH_RECOVERY = "Beached_PoffMouth_Recovery",
			LIMPETHOST = "Beached_LimpetHost", // for critters, used for growing limpets
			LIMPETHOST_RECOVERY = "Beached_LimpetHost_Recovery", // for critters, used for growing limpets
			STEPPED_IN_MUCUS = "Beached_SteppedInMucus",
			SUBMERGED_IN_MUCUS = "Beached_SubmergedInMucus",
			WISHING_STAR = "Beached_WishingStar", // applied when they see shooting stars
			POFF_CLEANEDTASTEBUDS = "Beached_PoffCleanedTasteBuds",
			POFF_HELIUM = "Beached_PoffHelium",
			PLUSHIE_MUFFIN = "Beached_Effect_PlushieMuffin",
			PLUSHIE_PACU = "Beached_Effect_PlushiePacu",
			PLUSHIE_PUFT = "Beached_Effect_PlushiePuft",
			PLUSHIE_VOLE = "Beached_Effect_PlushieVole",
			SANDBOX = "Beached_Effect_Sandbox",
			SANDBOX_RECENT = "Beached_Effect_RecentlySandbox",
			KARACOO_HUG = "Beached_KaracooHug",
			UNSAVORY_MEAL = "Beached_Unsavory_Meal";

		public class VANILLA
		{
			public const string
				SEAFOOD_RADIATION_RESISTANCE = "SeafoodRadiationResistance";
		}

		public static void Register(ModifierSet set)
		{
			var stressDelta = Db.Get().Amounts.Stress.deltaAttribute.Id;
			var peeDelta = Db.Get().Amounts.Bladder.deltaAttribute.Id;
			var carryCapacity = Db.Get().Attributes.CarryAmount.Id;
			var airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Id;

			new EffectBuilder(SANDBOX, 2370, false)
				.Modifier(Db.Get().Attributes.QualityOfLife.Id, 2, false)
				.HideInUI()
				.Add(set);

			new EffectBuilder(SANDBOX_RECENT, CONSTS.CYCLE_LENGTH / 2f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);

			new EffectBuilder(DAMP_PLANTGROWTH, 0f, false)
				.Modifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, true)
				.Add(set);

			new EffectBuilder(UNSAVORY_MEAL, 200f, true)
				.Modifier(stressDelta, 30f / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(ARID_PLANTGROWTH, 0f, true)
				.Modifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, -0.05f, true)
				.Add(set);

			new EffectBuilder(KARACOO_HUG, 0f, false)
				.Modifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 0.5f, true)
				.Add(set);

			new EffectBuilder(PLUSHIE_PACU, CONSTS.CYCLE_LENGTH, false)
				.Modifier(airConsumptionRate, -0.002f)
				.Add(set);

			new EffectBuilder(PLUSHIE_VOLE, CONSTS.CYCLE_LENGTH, false)
				.Modifier(Db.Get().Attributes.Digging.Id, 2)
				.Add(set);

			new EffectBuilder(PLUSHIE_MUFFIN, CONSTS.CYCLE_LENGTH, false)
				.Modifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, 1f / 250f)
				.Add(set);

			new EffectBuilder(PLUSHIE_PUFT, CONSTS.CYCLE_LENGTH, false)
				.Modifier(stressDelta, -5f / CONSTS.CYCLE_LENGTH) // TODO: something more interesting
				.Add(set);

			new EffectBuilder(OCEAN_BREEZE, 180f, false)
				.Modifier(airConsumptionRate, -0.01f)
				.Modifier(stressDelta, -5f / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(SCARED, 2f, true)
				.Modifier(stressDelta, 10f / CONSTS.CYCLE_LENGTH)
				.Modifier(peeDelta, 5f / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(SCARED_SIREN, 60f, true)
				.Modifier(stressDelta, 10f / CONSTS.CYCLE_LENGTH)
				.Modifier(peeDelta, 5f / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(STEPPED_IN_MUCUS, 1f, true)
				.Emote(BEmotes.mucusSlip, 60f)
				.Add(set);

			new EffectBuilder(SUBMERGED_IN_MUCUS, 120f, true)
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

			new EffectBuilder(ICEWRATH_DUPLICANT_RECOVERY, 160f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);

			new EffectBuilder(POFFMOUTH_RECOVERY, 160f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);

			new EffectBuilder(WISHING_STAR, 600f, false)
				.Modifier(Db.Get().Attributes.QualityOfLife.Id, 4)
				.Add(set);

			new EffectBuilder(POFF_CLEANEDTASTEBUDS, CONSTS.CYCLE_LENGTH * 6, false)
				.Modifier(Db.Get().Attributes.FoodExpectation.Id, 1)
				.Add(set);

			new EffectBuilder(POFF_HELIUM, CONSTS.CYCLE_LENGTH, false)
				.Modifier(Db.Get().Attributes.QualityOfLife.Id, 1)
				.Add(set);

			new EffectBuilder(LIMPETHOST, 0, true)
				.Modifier(BAmounts.LimpetGrowth.maxAttribute.Id, 100f)
				.Modifier(BAmounts.LimpetGrowth.deltaAttribute.Id, (5f / CONSTS.CYCLE_LENGTH) * 100f)
				.Modifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -250_000 / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(LIMPETHOST_RECOVERY, 160f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);
		}
	}
}
