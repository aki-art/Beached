using Beached.Content.Defs.Buildings;
using Database;

namespace Beached.Content.ModDb
{
	public class BTechs
	{
		public const string HIDDEN =
			"Beached_Tech_Hidden";

		public static void Register(Techs techs)
		{
			new Tech(
				HIDDEN,
				[
					ForceFieldGeneratorConfig.ID,
					CollarDispenserConfig.ID,
				],
				techs);

			Beached.Log.Debug("Added tech HIDDEN");
		}
	}
}
