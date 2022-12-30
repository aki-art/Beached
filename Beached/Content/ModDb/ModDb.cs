using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb
{
    public class ModDb
    {
        public static NotificationType BeachedTutorialMessage = (NotificationType)351;

        private static readonly Dictionary<string, float> acidVulnerabilities = new()
        {
            { HatchMetalConfig.ID.ToString(), CONSTS.ACID_VULNERABILITY.EXTREME },
            { DreckoPlasticConfig.ID, CONSTS.ACID_VULNERABILITY.IMMUNE },
            { "RollerSnakeSteel", CONSTS.ACID_VULNERABILITY.EXTREME },
            { "RollerSnake", CONSTS.ACID_VULNERABILITY.EXTREME },
        };

        private static readonly Dictionary<string, float> acidVulnerabilitiesBySpecies = new()
        {
            { GameTags.Creatures.Species.BeetaSpecies.ToString(), CONSTS.ACID_VULNERABILITY.MILDLY_ANNOYING },
            { GameTags.Creatures.Species.CrabSpecies.ToString(), CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
            { GameTags.Creatures.Species.DivergentSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.DreckoSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.GlomSpecies.ToString(), CONSTS.ACID_VULNERABILITY.MILDLY_ANNOYING },
            { GameTags.Creatures.Species.HatchSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.LightBugSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.MoleSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.MooSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.OilFloaterSpecies.ToString(), CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
            { GameTags.Creatures.Species.PacuSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.PuftSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { BTags.Species.Snail.ToString(), CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
            { GameTags.Creatures.Species.SquirrelSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
            { GameTags.Creatures.Species.StaterpillarSpecies.ToString(), CONSTS.ACID_VULNERABILITY.BAD },
        };


        public static float GetAcidVulnerability(GameObject go)
        {
            if (acidVulnerabilities.TryGetValue(go.PrefabID().ToString(), out var value))
            {
                return value;
            }

            if (go.TryGetComponent(out CreatureBrain b))
            {
                Log.Debug("Species: " + b.species);
            }
            if (go.TryGetComponent(out CreatureBrain brain) && acidVulnerabilitiesBySpecies.TryGetValue(brain.species.ToString(), out var value2))
            {
                return value2;
            }

            return 0;
        }
    }
}
