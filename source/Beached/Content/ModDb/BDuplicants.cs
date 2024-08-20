using Beached.Content.Defs.Duplicants;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BDuplicants
	{
		public static Dictionary<string, IDuplicantConfig> duplicantConfigs;
		public static Dictionary<HashedString, string> headKanims;

		public static void Register(Database.Personalities personalities)
		{
			duplicantConfigs = new()
			{
				{ MinnowConfig.ID , new MinnowConfig() }, // TODO clean up
				{ VahanoConfig.ID , new VahanoConfig() }, // TODO clean up
			};

			headKanims = [];

			foreach (var config in duplicantConfigs.Values)
			{
				personalities.Add(config.CreatePersonality());
				headKanims[(HashedString)config.GetID()] = config.GetHeadAnim();
			}
		}

		public static void ModifyBodyData(Personality personality, ref KCompBuilder.BodyData bodyData)
		{
			if (duplicantConfigs.TryGetValue(personality.Id, out var config))
				config.ModifyBodyData(ref bodyData);
		}

		public static void OnTraitRoll(MinionStartingStats instance)
		{
			if (duplicantConfigs.TryGetValue(instance.personality.Id, out var config))
				config.OnTraitRoll(instance);
		}
	}
}
