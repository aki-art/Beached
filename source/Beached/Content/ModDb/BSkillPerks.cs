using Database;

namespace Beached.Content.ModDb
{
    public class BSkillPerks
    {
        public static SkillPerk CanFindTreasures;
        public static SkillPerk CanFindMoreTreasures;
        public static SkillPerk CanSafelyHarvestClusters;
        public static SkillPerk CanAnalyzeClusters;
        public static SkillPerk AquaCulture1;
        public static SkillPerk AquaCulture2;
        public static SkillPerk AnimalHandling;
        public static SkillPerk MakiTrainer1;
        public static SkillPerk MakiTrainer2;

        public const string CANFINDTREASURES_ID = "Beached_SkillPerk_CanFindTreasures";
        public const string CANFINDMORETREASURES_ID = "Beached_SkillPerk_CanFindMoreTreasures";
        public const string CANSAFELYHARVESTCLUSTERS_ID = "Beached_SkillPerk_CanSafelyHarvestClusters";
        public const string CANANALYZECLUSTERS_ID = "Beached_SkillPerk_CanAnalyzeClusters";
        public const string ANIMALHANDLING_ID = "Beached_SkillPerk_AnimalHandling";
        public const string AQUACULTURE1_ID = "Beached_SkillPerk_AquaCulture1";
        public const string AQUACULTURE2_ID = "Beached_SkillPerk_AquaCulture2";
        public const string MAKITRAINER1_ID = "Beached_SkillPerk_MakiTrainer1";
        public const string MAKITRAINER2_ID = "Beached_SkillPerk_MakiTrainer2";

        public static void Register(SkillPerks skillPerks)
        {
            CanFindTreasures = skillPerks.Add(new SkillAttributePerk(
                CANFINDTREASURES_ID,
                BAttributes.PRECISION_ID,
                2,
                STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME));

            CanFindMoreTreasures = skillPerks.Add(new SkillAttributePerk(
                CANFINDMORETREASURES_ID,
                BAttributes.PRECISION_ID,
                2,
                STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY2.NAME));

            CanSafelyHarvestClusters = skillPerks.Add(new SkillAttributePerk(
                CANSAFELYHARVESTCLUSTERS_ID,
                BAttributes.PRECISION_ID,
                2,
                STRINGS.DUPLICANTS.ROLES.CRYSTALLOGRAPHY.NAME));

            CanAnalyzeClusters = skillPerks.Add(new SkillAttributePerk(
                CANANALYZECLUSTERS_ID,
                BAttributes.PRECISION_ID,
                2,
                STRINGS.DUPLICANTS.ROLES.GEOCHEMISTRY.NAME));

            AquaCulture1 = skillPerks.Add(new SkillAttributePerk(
                AQUACULTURE1_ID,
                Db.Get().Attributes.Botanist.Id,
                2,
                STRINGS.DUPLICANTS.ROLES.AQUACULTURE1.NAME));

            AquaCulture2 = skillPerks.Add(new SkillAttributePerk(
                AQUACULTURE2_ID,
                Db.Get().Attributes.Botanist.Id,
                2,
                STRINGS.DUPLICANTS.ROLES.AQUACULTURE2.NAME));

            AnimalHandling = skillPerks.Add(new SkillAttributePerk(
                AQUACULTURE2_ID,
                Db.Get().Attributes.Ranching.Id,
                2,
                STRINGS.DUPLICANTS.ROLES.ANIMALHANDLING.NAME));

            MakiTrainer1 = skillPerks.Add(new SimpleSkillPerk(
                AQUACULTURE1_ID,
                STRINGS.DUPLICANTS.ROLES.MAKITRAINER1.NAME));

            MakiTrainer2 = skillPerks.Add(new SimpleSkillPerk(
                AQUACULTURE1_ID,
                STRINGS.DUPLICANTS.ROLES.MAKITRAINER2.NAME));
        }
    }
}
