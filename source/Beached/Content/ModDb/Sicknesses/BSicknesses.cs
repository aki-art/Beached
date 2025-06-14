﻿using Beached.Content.ModDb.Germs;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class BSicknesses
	{
		public static Sickness limpets;
		public static Sickness capped;
		public static Sickness poffMouth;
		public static Sickness iceWrath;

		[DbEntry]
		public static void Register(Database.Sicknesses __instance)
		{
			capped = __instance.Add(new CappedSickness());
			poffMouth = __instance.Add(new PoffMouthSickness());
			limpets = __instance.Add(new LimpetsSickness());
			iceWrath = __instance.Add(new IceWrathSickness());

			var exposures = new List<ExposureType>(TUNING.GERM_EXPOSURE.TYPES)
			{
				new() {
					germ_id = LimpetEggGerms.ID,
					sickness_id = LimpetsSickness.ID,
					exposure_threshold = 100,
					base_resistance = 0,
					infect_immediately = true,
					excluded_effects =
					[
						BEffects.LIMPETS_DUPLICANT_RECOVERY
					]
				},
				new() {
					germ_id = CapSporeGerms.ID,
					sickness_id = CappedSickness.ID,
					exposure_threshold = 100,
					base_resistance = 0,
					infect_immediately = true,
					excluded_effects =
					[
						BEffects.CAPPED_RECOVERY
					]
				},
				new() {
					germ_id = PoffSporeGerms.ID,
					sickness_id = PoffMouthSickness.ID,
					exposure_threshold = 100,
					base_resistance = 0,
					infect_immediately = true,
					excluded_effects =
					[
						BEffects.POFFMOUTH_RECOVERY
					]
				},
				new() {
					germ_id = IceWrathGerms.ID,
					sickness_id = IceWrathSickness.ID,
					exposure_threshold = 100,
					base_resistance = 0,
					infect_immediately = true,
					excluded_effects =
					[
						BEffects.ICEWRATH_DUPLICANT_RECOVERY
					]
				}
			};

			TUNING.GERM_EXPOSURE.TYPES = [.. exposures];

			foreach (var type in TUNING.GERM_EXPOSURE.TYPES)
				if (type.excluded_effects != null
					&& type.excluded_effects.Contains("HistamineSuppression")
					&& !type.excluded_effects.Contains(BEffects.SUPER_ALLERGY_MED))
					type.excluded_effects.Add(BEffects.SUPER_ALLERGY_MED);
		}
	}
}
