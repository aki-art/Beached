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

		public static void Register(SkillGroups skillGroups)
		{
			Precision = new SkillGroup(
							PRECISION_ID,
							//BChoreGroups.HANDYWORK_ID,
							Db.Get().ChoreGroups.Research.Id,
							STRINGS.DUPLICANTS.CHOREGROUPS.BEACHED_CHOREGROUP_HANDYWORK.NAME,
							ModAssets.Sprites.ERRAND_MINERALOGY,
							ModAssets.Sprites.ARCHETYPE_MINERALOGY);

			var index = skillGroups.resources.FindIndex(s => s.Id == skillGroups.Mining.Id);

			if (index == -1)
			{
				Log.Warning("Mining skill not found");
				Precision = skillGroups.Add(Precision);
			}
			else
			{
				skillGroups.resources.Insert(index + 1, Precision);
			}

			Precision.relevantAttributes = new List<Attribute>
			{
				BAttributes.handSteadiness
			};

			Precision.requiredChoreGroups = new List<string>
			{
                //BChoreGroups.HANDYWORK_ID
                Db.Get().ChoreGroups.Research.Id
			};

			DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.Add(PRECISION_ID, new List<string>()
			{
					"DiggingDown",
					"ArtDown",
					"SlowLearner"
			});
		}
	}
}
