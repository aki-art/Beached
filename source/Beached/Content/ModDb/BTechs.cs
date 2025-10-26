using Beached.Content.Defs.Buildings;
using Database;

namespace Beached.Content.ModDb
{
	public class BTechs
	{
		public const string
			HIDDEN = "Beached_Tech_Hidden",
			HYDRO_ELECTRONICS = "Beached_Currents";

		public static void Register(Techs techs)
		{
			new Tech(
				HIDDEN,
				[
					ForceFieldGeneratorConfig.ID,
					CollarDispenserConfig.ID,
				],
				techs);

			var hydro = new Tech(
					HYDRO_ELECTRONICS,
					[
						WaterGeneratorConfig.ID,
						ChimeConfig.ID
					],
					techs)
			{
				//requiredTech = [techs.Get(FUtility.CONSTS.RESEARCH.POWER.POWER_REGULATION)],
				tier = 2
			};

			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.POWER);
			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.GENERATOR);
			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.WATER);

		}

		public static void PostInit(Techs techs)
		{
			var hydro = techs.Get(HYDRO_ELECTRONICS);

			var advancedPower = techs.Get(FUtility.CONSTS.TECH.POWER.ADVANCED_POWER_REGULATION);

			advancedPower.requiredTech ??= [];
			advancedPower.requiredTech.Add(hydro);
		}
	}
}
