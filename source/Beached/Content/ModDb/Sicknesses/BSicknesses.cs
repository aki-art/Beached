using Beached.Content.ModDb.Germs;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class BSicknesses
	{
		public static Sickness limpets;
		public static Sickness capped;
		public static Sickness poffMouth;

		[DbEntry]
		public static void Register(Database.Sicknesses __instance)
		{
			capped = __instance.Add(new CappedSickness());
			poffMouth = __instance.Add(new PoffMouthSickness());
			limpets = __instance.Add(new LimpetsSickness());

			var exposures = new List<ExposureType>(TUNING.GERM_EXPOSURE.TYPES)
			{
				new ExposureType
				{
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
				new ExposureType
				{
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
				new ExposureType
				{
					germ_id = PoffSporeGerms.ID,
					sickness_id = PoffMouthSickness.ID,
					exposure_threshold = 100,
					base_resistance = 0,
					infect_immediately = true,
					excluded_effects =
					[
						BEffects.POFFMOUTH_RECOVERY
					]
				}
			};


			TUNING.GERM_EXPOSURE.TYPES = exposures.ToArray();
		}
	}
}
