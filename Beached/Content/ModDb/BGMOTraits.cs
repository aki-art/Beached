using Beached.Content.Defs.Foods;
using System.Linq;

namespace Beached.Content.ModDb
{
    internal class BGMOTraits
    {
        public const string GMO_GROUP = "Beached_GMOTraits";

        public const string MEATY = "Beached_GMOTraits_Meaty";
        public const string EVERLASTING = "Beached_GMOTraits_Everlasting";

        public static void Register()
        {
            var db = Db.Get();

            db.CreateTrait(
                MEATY,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_MEATY.NAME,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_MEATY.DESC,
                GMO_GROUP,
                true,
                null,
                true,
                false)
                .OnAddTrait += obj =>
                {
                    if(obj.TryGetComponent(out Butcherable butcherable))
                    {
                        for (int i = 0; i < butcherable.drops.Length; i++) {
                            var drop = butcherable.drops[i];
                            if(drop == MeatConfig.ID)
                            {
                                butcherable.drops[i] = HighQualityMeatConfig.ID;
                            }
                        }
                    }
                };

            db.CreateTrait(
                EVERLASTING,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_EVERLASTING.NAME,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_EVERLASTING.DESC,
                GMO_GROUP,
                true,
                null,
                true,
                false);
        }
    }
}
