using Beached.Content.Defs.Buildings;
using Beached.Content.Defs.Equipment;
using Database;

namespace Beached.Content.ModDb
{
	public class BTechs
	{
		public const string
			HIDDEN = "Beached_Tech_Hidden",
			HYDRO_ELECTRONICS = "Beached_Currents",
			MATERIALS1 = "Bached_MaterialsI";

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
				tier = 2
			};


			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.POWER);
			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.GENERATOR);
			hydro.AddSearchTerms(global::STRINGS.SEARCH_TERMS.WATER);

			var mats1 = new Tech(
					MATERIALS1,
					[
						RubberBootsConfig.ID,
						SealedStorageConfig.ID,
						Elements.rubber.CreateTag().ToString()
					],
					techs);

			mats1.AddSearchTerms(global::STRINGS.SEARCH_TERMS.STORAGE);
			mats1.AddSearchTerms(STRINGS.MISC.TAGS.BEACHED_RUBBERMATERIAL);
		}
	}
}
