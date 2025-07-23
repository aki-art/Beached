namespace Beached.Content.ModDb
{
	public class BEffects
	{
		public const float PERSISTENT = 0f;

		public const string
			ARID_PLANTGROWTH = "Beached_Arid_PlantGrowthPenalty",
			CAPPED_RECOVERY = "Beached_Capped_Recovery",
			COMFORTABLE = "Beached_Comfortable",
			DAMP_PLANTGROWTH = "Beached_Damp_PlantGrowthBoost",
			DAZED = "Beached_Dazed",
			DIMWIT = "Beached_Dimwit",
			FLUMMOXED = "Beached_Flummoxed",
			GNAWBERRY_JUICE = "Beached_GnawberryJuice",
			GRISTLE_JUICE = "Beached_GristleJuice",
			ICEWRATH_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			KARACOO_HUG = "Beached_KaracooHug",
			LIMPETHOST = "Beached_LimpetHost", // for critters, used for growing limpets
			LIMPETHOST_RECOVERY = "Beached_LimpetHost_Recovery", // for critters, used for growing limpets
			LIMPETS_DUPLICANT_RECOVERY = "Beached_Limpets_Duplicant_Recovery",
			LUBRICATED_DOOR = "Beached_LubricatedDoor",
			LUBRICATED_TUNEUP = "Beached_LubricatedTuneUp",
			LUBRICATED_OPERATIONSPEED = "Beached_LubricatedOperationSpeed",
			NICE_SCENT = "Beached_NiceScent", // shower with soap
			OCEAN_BREEZE = "Beached_OceanBreeze", // -5% stress/cycle, -10g Oxygen
			PLUSHIE_MUFFIN = "Beached_PlushieMuffin",
			PLUSHIE_PACU = "Beached_PlushiePacu",
			PLUSHIE_PUFT = "Beached_PlushiePuft",
			PLUSHIE_VOLE = "Beached_PlushieVole",
			POFF_CLEANEDTASTEBUDS = "Beached_PoffCleanedTasteBuds",
			POFF_HELIUM = "Beached_PoffHelium",
			POFFMOUTH_RECOVERY = "Beached_PoffMouth_Recovery",
			RECENTLY_PRODUCED_LUBRICANT = "Beached_Recently_Produced_Lubricant",
			SANDBOX = "Beached_Sandbox",
			SANDBOX_RECENT = "Beached_Effect_RecentlySandbox",
			SCARED = "Beached_Scared", // +10% Stress/cycle, +5% Bladder delta
			SCARED_SIREN = "Beached_Scared_Siren", // +10% Stress/cycle, +5% Bladder delta
			SUBMERGED_IN_MUCUS = "Beached_SubmergedInMucus",
			THALASSOPHILE_BONUS = "Beached_ThalassophileBonus",
			UNSAVORY_MEAL = "Beached_Unsavory_Meal",
			SUPER_ALLERGY_MED = "Beached_SuperAllergeMed",
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

			//var histamineSuppression = set.effects.Get("HistamineSuppression");

			var cycle = CONSTS.CYCLE_LENGTH;

			new EffectBuilder(THALASSOPHILE_BONUS, 0, false)
				.Modifier(morale, 2, false)
				.Modifier(BAttributes.operatingSpeed.Id, 0.1f, true)
				.Add(set);

			new EffectBuilder(GNAWBERRY_JUICE, cycle, false)
				.Modifier(morale, 2, false)
				.Modifier(stressDelta, -0.1f, true)
				.Add(set);

			new EffectBuilder(GRISTLE_JUICE, cycle, false)
				.Modifier(morale, 2, false)
				.Modifier(stressDelta, -0.1f, true)
				.Add(set);

			new EffectBuilder(SUPER_ALLERGY_MED, CONSTS.CYCLE_LENGTH * 7f, false)
				.HideInUI()
				.Add(set);

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

			new EffectBuilder(RECENTLY_PRODUCED_LUBRICANT, 10f, false)
				.Modifier(BAmounts.Mucus.deltaAttribute.Id, 1, true)
				.HideInUI()
				.Add(set);

			new EffectBuilder(NICE_SCENT, CONSTS.CYCLE_LENGTH, false)
				.Modifier(morale, 2, false)
				.Add(set);

			new EffectBuilder(COMFORTABLE, 0, false)
				.Modifier(stressDelta, -5f / CONSTS.CYCLE_LENGTH)
				.Modifier(morale, 6, false)
				.Add(set);

			new EffectBuilder(LUBRICATED_DOOR, PERSISTENT, false)
				.Modifier(BAttributes.doorOpeningSpeed.Id, 2f, true)
				.Icon("status_item_plant_liquid")
				.Add(set);

			new EffectBuilder(LUBRICATED_OPERATIONSPEED, PERSISTENT, false)
				.Modifier(BAttributes.operatingSpeed.Id, 0.5f, true)
				.Icon("status_item_plant_liquid")
				.Add(set);

			new EffectBuilder(LUBRICATED_TUNEUP, PERSISTENT, false)
				.Modifier(Db.Get().Attributes.GeneratorOutput.Id, 25f, false)
				.Icon("status_item_plant_liquid")
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
