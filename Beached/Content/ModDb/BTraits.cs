using Beached.Content.Defs.Items;
using Beached.Content.Scripts;
using Beached.Content.Scripts.LifeGoals;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
    public class BTraits
    {
        public const string DEXTEROUS = "Beached_Dexterous";

        public class LifeGoals
        {
            public const string JEWELLERY_AQUAMARINE = "Beached_Trait_WantsJewellery";
            public const string BEDROOM_SURFBOARD = "Beached_Trait_WantsSurfboardInBedroom";
        }

        public static List<string> LIFEGOALS = new();

        public static void Register()
        {
            var dexterousTrait = Db.Get().CreateTrait(
                DEXTEROUS,
                STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.NAME,
                STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.DESC,
                BAttributes.PRECISION_ID,
                true,
                null,
                true,
                true);

            dexterousTrait.Add(new AttributeModifier(
                BAttributes.PRECISION_ID,
                TRAITS.GOOD_ATTRIBUTE_BONUS,
                STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.NAME));

            AddJewelleryTrait(
                LifeGoals.JEWELLERY_AQUAMARINE,
                STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME,
                string.Format("This duplicant really wishes to express themselves by wearing a {0}.", STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME),
                MaxixePendantConfig.ID);

            AddBedroomTrait(
                LifeGoals.BEDROOM_SURFBOARD,
                "Surfin' and Snoozin'",
                string.Format("This duplicant sannot stop talking about how cool it would be to have a {0} in their bedroom.", global::STRINGS.BUILDINGS.PREFABS.MECHANICALSURFBOARD.NAME),
                MechanicalSurfboardConfig.ID);

            DUPLICANTSTATS.GOODTRAITS.Add(new DUPLICANTSTATS.TraitVal()
            {
                id = DEXTEROUS,
                rarity = DUPLICANTSTATS.RARITY_COMMON,
                dlcId = "",
                mutuallyExclusiveTraits = new List<string>
                {
                    //"Anemic"
                }
            });

            LIFEGOALS.Add(LifeGoals.JEWELLERY_AQUAMARINE);
        }

        private static void AddJewelleryTrait(string id, string name, string desc, Tag targetTag, Func<string> extendedDescFn = null)
        {
            var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
            trait.OnAddTrait = go =>
            {
                Log.Debug("on add traits");
                go.AddOrGet<LifeGoalTracker>().wantTag = targetTag;
                Log.Debug(targetTag);
                go.FindOrAddUnityComponent<EquipmentGoal>();
            };


            trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";

            if (extendedDescFn != null)
            {
                trait.ExtendedTooltip += extendedDescFn;
            }
        }

        private static void AddBedroomTrait(string id, string name, string desc, Tag targetTag, Func<string> extendedDescFn = null)
        {
            var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
            trait.OnAddTrait = go =>
            {
                Log.Debug("on add traits");
                go.AddOrGet<LifeGoalTracker>().wantTag = targetTag;
                Log.Debug(targetTag);
                go.FindOrAddUnityComponent<BedroomBuildingGoal>();
            };


            trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";

            if (extendedDescFn != null)
            {
                trait.ExtendedTooltip += extendedDescFn;
            }
        }
        public static Trait GetGoalForPersonality(Personality personality)
        {
            return Db.Get().traits.Get(LifeGoals.BEDROOM_SURFBOARD);
        }
    }
}
