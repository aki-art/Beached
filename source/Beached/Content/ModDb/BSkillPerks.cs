using Database;

namespace Beached.Content.ModDb
{
	public class BSkillPerks
	{
		public static SkillPerk
			IncreasePrecisionSmall,
			IncreasePrecisionMedium,
			IncreasePrecisionLarge,
			CanFindTreasures,
			CanFindMoreTreasures,
			CanSafelyHarvestClusters,
			CanAnalyzeClusters,
			CanSynthetizeClusters,
			CanCutGems,
			AquaCulture1,
			AquaCulture2,
			AnimalHandling,
			MakiTrainer1,
			MakiTrainer2,
			CanMoveCritters;

		public const string
			INCREASE_PRECISION_SMALL_ID = "Beached_SkillPerk_IncreasePrecisionSmall",
			INCREASE_PRECISION_MEDIUM_ID = "Beached_SkillPerk_IncreasePrecisionMedium",
			INCREASE_PRECISION_LARGE_ID = "Beached_SkillPerk_IncreasePrecisionLarge",
			CAN_FIND_TREASURES_ID = "Beached_SkillPerk_CanFindTreasures",
			CAN_FIND_MORE_TREASURES_ID = "Beached_SkillPerk_CanFindMoreTreasures",
			CAN_SAFELY_HARVEST_CLUSTERS_ID = "Beached_SkillPerk_CanSafelyHarvestClusters",
			CAN_ANALYZE_CLUSTERS_ID = "Beached_SkillPerk_CanAnalyzeClusters",
			CAN_SYNTHETIZE_CLUSTERS_ID = "Beached_SkillPerk_CanSynthetizeClusters",
			CAN_CUT_GEMS_ID = "Beached_SkillPerk_CanCutGems",
			ANIMAL_HANDLING_ID = "Beached_SkillPerk_AnimalHandling",
			AQUACULTURE1_ID = "Beached_SkillPerk_AquaCulture1",
			AQUACULTURE2_ID = "Beached_SkillPerk_AquaCulture2",
			MAKITRAINER1_ID = "Beached_SkillPerk_MakiTrainer1",
			MAKITRAINER2_ID = "Beached_SkillPerk_MakiTrainer2",
			CAN_MOVE_CRITTERS_ID = "Beached_SkillPerk_CanMoveCritters";

		[DbEntry]
		public static void Register(SkillPerks __instance)
		{
			CanMoveCritters = __instance.Add(new SimpleSkillPerk(CAN_MOVE_CRITTERS_ID, STRINGS.DUPLICANTS.ROLES.ANIMALHANDLING.CAN_MOVE_CRITTERS));

			CanFindTreasures = __instance.Add(new SimpleSkillPerk(CAN_FIND_TREASURES_ID, STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.CAN_FIND_TREASURES));

			CanFindMoreTreasures = __instance.Add(new SimpleSkillPerk(
				CAN_FIND_MORE_TREASURES_ID,
				STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY2.CAN_FIND_MORE_TREASURES));

			CanCutGems = __instance.Add(new SimpleSkillPerk(
				CAN_CUT_GEMS_ID,
				STRINGS.DUPLICANTS.ROLES.CRYSTALLOGRAPHY.CAN_CUT_GEMS));

			IncreasePrecisionSmall = __instance.Add(new SkillAttributePerk(
				INCREASE_PRECISION_SMALL_ID,
				BAttributes.PRECISION_ID,
				2,
				STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME));

			IncreasePrecisionMedium = __instance.Add(new SkillAttributePerk(
				INCREASE_PRECISION_MEDIUM_ID,
				BAttributes.PRECISION_ID,
				2,
				STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY2.NAME));

			IncreasePrecisionLarge = __instance.Add(new SkillAttributePerk(
				INCREASE_PRECISION_LARGE_ID,
				BAttributes.PRECISION_ID,
				2,
				STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME));

			CanSafelyHarvestClusters = __instance.Add(new SimpleSkillPerk(CAN_SAFELY_HARVEST_CLUSTERS_ID, STRINGS.DUPLICANTS.ROLES.CRYSTALLOGRAPHY.HARVEST_CLUSTERS));

			CanAnalyzeClusters = __instance.Add(new SkillAttributePerk(
				CAN_ANALYZE_CLUSTERS_ID,
				BAttributes.PRECISION_ID,
				2,
				STRINGS.DUPLICANTS.ROLES.GEOCHEMISTRY.NAME));

			CanSynthetizeClusters = __instance.Add(new SimpleSkillPerk(CAN_SYNTHETIZE_CLUSTERS_ID, STRINGS.DUPLICANTS.ROLES.GEOCHEMISTRY.CAN_SYNTHETIZE_CLUSTERS));

			AquaCulture1 = __instance.Add(new SkillAttributePerk(
				AQUACULTURE1_ID,
				Db.Get().Attributes.Botanist.Id,
				2,
				STRINGS.DUPLICANTS.ROLES.AQUACULTURE1.DESCRIPTION));

			AquaCulture2 = __instance.Add(new SkillAttributePerk(
				AQUACULTURE2_ID,
				Db.Get().Attributes.Botanist.Id,
				2,
				STRINGS.DUPLICANTS.ROLES.AQUACULTURE2.NAME));

			AnimalHandling = __instance.Add(new SkillAttributePerk(
				AQUACULTURE2_ID,
				Db.Get().Attributes.Ranching.Id,
				2,
				STRINGS.DUPLICANTS.ROLES.ANIMALHANDLING.NAME));

			MakiTrainer1 = __instance.Add(new SimpleSkillPerk(
				AQUACULTURE1_ID,
				STRINGS.DUPLICANTS.ROLES.MAKITRAINER1.DESCRIPTION));

			MakiTrainer2 = __instance.Add(new SimpleSkillPerk(
				AQUACULTURE1_ID,
				STRINGS.DUPLICANTS.ROLES.MAKITRAINER2.DESCRIPTION));
		}
	}
}
