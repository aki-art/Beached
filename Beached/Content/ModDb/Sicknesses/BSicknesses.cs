using Beached.Content.ModDb.Germs;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
    public class BSicknesses
    {
        public static Sickness limpets;
        public static Sickness capped;


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
                }
            };

            TUNING.GERM_EXPOSURE.TYPES = exposures.ToArray();
        }
    }
}
