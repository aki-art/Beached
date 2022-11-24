using Klei.AI;
using System.Collections.Generic;
using TUNING;

namespace Beached.Content.ModDb
{
    public class BTraits
    {
        public const string DEXTEROUS = "Beached_Dexterous";

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
        }
    }
}
