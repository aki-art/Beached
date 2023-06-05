using Beached.Content.Defs.Duplicants;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
    public class BDuplicants
    {
        public static Dictionary<string, IDuplicantConfig> duplicantConfigs;

        public static void Register(Database.Personalities personalities)
        {
            duplicantConfigs = new()
            {
                { MinnowConfig.ID , new MinnowConfig() },
                { MikaConfig.ID , new MikaConfig() },
            };

            foreach (var config in duplicantConfigs.Values)
            {
                personalities.Add(config.CreatePersonality());
            }
        }

        public static void ModifyBodyData(Personality personality, ref KCompBuilder.BodyData bodyData)
        {
            Log.Debug("trying to modify body: " + personality.Id);
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
