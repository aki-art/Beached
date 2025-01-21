namespace Beached.Content.ModDb
{
	public class BEffects
	{
		public const float PERSISTENT = 0f;

		public const string
			ARID_PLANTGROWTH = "Beached_Arid_PlantGrowthPenalty",
			CAPPED_RECOVERY = "Beached_Capped_Recovery",
			DAMP_PLANTGROWTH = "Beached_Damp_PlantGrowthBoost",
			DAZED = "Beached_Effect_Dazed",
			DIMWIT = "Beached_Dimwit",
			FLUMMOXED = "Beached_Flummoxed",
			ICEWRATH_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			KARACOO_HUG = "Beached_KaracooHug",
			LIMPETHOST = "Beached_LimpetHost", // for critters, used for growing limpets
			LIMPETHOST_RECOVERY = "Beached_LimpetHost_Recovery", // for critters, used for growing limpets
			LIMPETS_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			LUBRICATED = "Beached_Effect_Lubricated",
			NICE_SCENT = "Beached_Effect_NiceScent", // shower with soap
			OCEAN_BREEZE = "Beached_OceanBreeze", // -5% stress/cycle, -10g Oxygen
			PLUSHIE_MUFFIN = "Beached_Effect_PlushieMuffin",
			PLUSHIE_PACU = "Beached_Effect_PlushiePacu",
			PLUSHIE_PUFT = "Beached_Effect_PlushiePuft",
			PLUSHIE_VOLE = "Beached_Effect_PlushieVole",
			POFF_CLEANEDTASTEBUDS = "Beached_PoffCleanedTasteBuds",
			POFF_HELIUM = "Beached_PoffHelium",
			POFFMOUTH_RECOVERY = "Beached_PoffMouth_Recovery",
			SANDBOX = "Beached_Effect_Sandbox",
			SANDBOX_RECENT = "Beached_Effect_RecentlySandbox",
			SCARED = "Beached_Scared", // +10% Stress/cycle, +5% Bladder delta
			SCARED_SIREN = "Beached_Scared_Siren", // +10% Stress/cycle, +5% Bladder delta
			SUBMERGED_IN_MUCUS = "Beached_SubmergedInMucus",
			UNSAVORY_MEAL = "Beached_Unsavory_Meal",
			WISHING_STAR = "Beached_WishingStar"; // applied when they see shooting stars

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
			var morale = Db.Get().Attributes.QualityOfLife.Id;

			new EffectBuilder(SANDBOX, 2370, false)
				.Modifier(Db.Get().Attributes.QualityOfLife.Id, 2, false)
				.HideInUI()
				.Add(set);

			new EffectBuilder(SANDBOX_RECENT, CONSTS.CYCLE_LENGTH / 2f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);

			new EffectBuilder(FLUMMOXED, 200, false)
				.Add(set);

			new EffectBuilder(NICE_SCENT, CONSTS.CYCLE_LENGTH, false)
				.Modifier(morale, 2, false)
				.Add(set);

			new EffectBuilder(LUBRICATED, PERSISTENT, false)
				.Modifier(Db.Get().Attributes.GeneratorOutput.Id, 0.25f, true)
				.Modifier(BAttributes.operatingSpeed.Id, 0.5f, true)
				.Modifier(BAttributes.doorOpeningSpeed.Id, 2f, true)
				.HideInUI()
				.Add(set);

			new EffectBuilder(DAMP_PLANTGROWTH, PERSISTENT, false)
				.Modifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, true)
				.Add(set);

			new EffectBuilder(UNSAVORY_MEAL, 200f, true)
				.Modifier(stressDelta, 30f / CONSTS.CYCLE_LENGTH)
				.Add(set);

			new EffectBuilder(ARID_PLANTGROWTH, PERSISTENT, true)
				.Modifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, -0.05f, true)
				.Add(set);

			new EffectBuilder(KARACOO_HUG, PERSISTENT, false)
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

			new EffectBuilder(LIMPETHOST, PERSISTENT, true)
				.Modifier(Db.Get().CritterAttributes.Metabolism.Id, 1.2f, true)
				.Modifier(BAmounts.LimpetGrowth.deltaAttribute.Id, 100f / (3f * CONSTS.CYCLE_LENGTH))
				.Add(set);

			new EffectBuilder(DAZED, 3f * CONSTS.CYCLE_LENGTH, false)
				.Modifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, true)
				.Add(set);

			new EffectBuilder(LIMPETHOST_RECOVERY, 160f, false)
				.HideInUI()
				.HideFloatingText()
				.Add(set);
		}
	}
}
