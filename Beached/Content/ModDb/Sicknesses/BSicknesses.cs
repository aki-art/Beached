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


        public static void Register(Database.Sicknesses sicknesses)
        {
            /*            limpets = sicknesses.Add(new LimpetsSickness());

                        var exposures = new List<ExposureType>(TUNING.GERM_EXPOSURE.TYPES)
                        {
                            new ExposureType
                            {
                                germ_id = LimpetEggGerms.ID,
                                sickness_id = LimpetsSickness.ID,
                                exposure_threshold = 100,
                                base_resistance = 0,
                                infect_immediately = true,
                                excluded_effects = new List<string>
                                {
                                    BEffects.LIMPETS_DUPLICANT_RECOVERY
                                }
                            }
                        };*/

            capped = sicknesses.Add(new CappedSickness());
            poffMouth = sicknesses.Add(new PoffMouthSickness());

            var exposures = new List<ExposureType>(TUNING.GERM_EXPOSURE.TYPES)
            {
                new ExposureType
                {
                    germ_id = CapSporeGerms.ID,
                    sickness_id = CappedSickness.ID,
                    exposure_threshold = 100,
                    base_resistance = 0,
                    infect_immediately = true,
                    excluded_effects = new List<string>
                    {
                        BEffects.CAPPED_RECOVERY
                    }
                },
                new ExposureType
                {
                    germ_id = PoffSporeGerms.ID,
                    sickness_id = PoffMouthSickness.ID,
                    exposure_threshold = 100,
                    base_resistance = 0,
                    infect_immediately = true,
                    excluded_effects = new List<string>
                    {
                        BEffects.POFFMOUTH_RECOVERY
                    }
                }
            };


            TUNING.GERM_EXPOSURE.TYPES = exposures.ToArray();
        }
    }
}
