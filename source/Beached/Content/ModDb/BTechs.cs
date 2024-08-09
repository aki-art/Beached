using Beached.Content.Defs.Buildings;
using Database;

namespace Beached.Content.ModDb
{
	public class BTechs
	{
		public const string HIDDEN_ATMOSPHERIC_FORCEFIELD_GENERATOR =
			"Beached_Tech_Hidden_AtmosphericForcefieldGenerator";

		public static void Register(Techs techs)
		{
			new Tech(
				HIDDEN_ATMOSPHERIC_FORCEFIELD_GENERATOR,
				new()
				{
					ForceFieldGeneratorConfig.ID
				},
				techs);
		}

		public static void UnlockTech(string id)
		{
			var tech = Db.Get().Techs.Get(id);
			if (tech == null) return;

			Research.Instance.GetOrAdd(tech).Purchased();
			Game.Instance.Trigger((int)GameHashes.ResearchComplete, tech);
		}
	}
}
