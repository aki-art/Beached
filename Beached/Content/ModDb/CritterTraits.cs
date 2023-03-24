using Beached.Content.Defs.Foods;
using UnityEngine;

namespace Beached.Content.ModDb
{
    internal class CritterTraits
    {
        public const string GMO_GROUP = "Beached_GMOTraits";

        public const string MEATY = "Beached_GMOTraits_Meaty";
        public const string EVERLASTING = "Beached_GMOTraits_Everlasting";
        public const string LASTING = "Beached_GMOTraits_Lasting";
        public const string PRODUCTIVE1 = "Beached_GMOTraits_Productive1";
        public const string PRODUCTIVE2 = "Beached_GMOTraits_Productive2";
        public const string PRODUCTIVE3 = "Beached_GMOTraits_Productive3";
        public const string BLAND = "Beached_GMOTraits_Bland";
        public const string FABULOUS = "Beached_GMOTraits_Fabulous";
        public const string HYPOALLERGENIC = "Beached_GMOTraits_HypoAllergenic";
        public const string CHONKER = "Beached_GMOTraits_Chonker";

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
                .OnAddTrait += OnMeaty;

            db.CreateTrait(
                EVERLASTING,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_EVERLASTING.NAME,
                STRINGS.CREATURES.TRAITS.BEACHED_GMOTRAIT_EVERLASTING.DESC,
                GMO_GROUP,
                true,
                null,
                true,
                false);

            if(Mod.isCritterTraitsRebornHere)
            {
                Integration.CritterTraitsReborn.addTraitToVisibleList(MEATY);
                Integration.CritterTraitsReborn.addTraitToVisibleList(EVERLASTING);
            }
        }

        private static void OnMeaty(GameObject obj)
        {
            if (obj.TryGetComponent(out Butcherable butcherable))
            {
                for (int i = 0; i < butcherable.drops.Length; i++)
                {
                    var drop = butcherable.drops[i];
                    if (drop == MeatConfig.ID)
                    {
                        butcherable.drops[i] = HighQualityMeatConfig.ID;
                    }
                }
            }

            if (obj.TryGetComponent(out KBatchedAnimController kbac))
            {
                kbac.animScale *= 1.1f;
            }

            if (obj.TryGetComponent(out KBoxCollider2D collider))
            {
                collider.size *= 1.1f;
            }
        }
    }
}
