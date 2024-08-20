using Database;
using Klei.AI;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
	public class BSkillGroups
	{
		public static SkillGroup Precision;
		public static string PRECISION_ID = "Beached_Skillgroup_Precision";

		[DbEntry]
		public static void Register(SkillGroups __instance)
		{
			Precision = new SkillGroup(
				PRECISION_ID,
				BChoreGroups.HANDYWORK_ID,
				STRINGS.DUPLICANTS.CHOREGROUPS.BEACHED_CHOREGROUP_HANDYWORK.NAME,
				ModAssets.Sprites.ERRAND_MINERALOGY,
				ModAssets.Sprites.ARCHETYPE_MINERALOGY);

			var index = __instance.resources.FindIndex(s => s.Id == __instance.Mining.Id);

			if (index == -1)
			{
				Log.Warning("Mining skill not found");
				Precision = __instance.Add(Precision);
			}
			else
			{
				__instance.resources.Insert(index + 1, Precision);
			}

			Precision.relevantAttributes =
			[
				BAttributes.handSteadiness
			];

			Precision.requiredChoreGroups =
			[
				BChoreGroups.HANDYWORK_ID
			];

			DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.Add(PRECISION_ID,
			[
					"DiggingDown",
					"ArtDown",
					"SlowLearner"
			]);
		}
	}
}
