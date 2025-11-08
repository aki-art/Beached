using Database;

namespace Beached.Content.ModDb
{
	public class BSkills
	{
		public static Skill archeology;
		public static Skill archeology2;
		public static Skill crystallography; // save harvest, gemcutting
		public static Skill geoChemistry;
		public static Skill animalHandling;
		public static Skill makiTraining1;
		public static Skill makiTraining2;
		public static Skill aquaCulture1;
		public static Skill aquaCulture2;
		// fungiCulture?

		public const string
			ANIMALHANDLING_ID = "Beached_AnimalHandling",
			ARCHEOLOGY_ID = "Beached_Archeology",
			ARCHEOLOGY2_ID = "Beached_Archeology2",
			CRYSTALLOGRAPHY_ID = "Beached_Crystallography",
			GEOCHEMISTRY_ID = "Beached_GeoChemistry",
			AQUACULTURE1_ID = "Beached_AquaCulture1",
			AQUACULTURE2_ID = "Beached_AquaCulture2",
			MAKITRAINING1_ID = "Beached_MakiTraining1",
			MAKITRAINING2_ID = "Beached_MakiTraining2";

		[DbEntry]
		public static void Register(Skills __instance)
		{
			archeology = __instance.Add(new Skill(
					ARCHEOLOGY_ID,
					STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME,
					STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.DESCRIPTION,
					0,
					BAccessories.ARCHEOLOGY_HAT,
					"beached_skillbadge_archeology_1",
					BSkillGroups.PRECISION_ID,
					[
						BSkillPerks.CanFindTreasures,
						BSkillPerks.IncreasePrecisionSmall
					],
					null,
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			archeology2 = __instance.Add(new Skill(
					ARCHEOLOGY2_ID,
					STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY2.NAME,
					STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY2.DESCRIPTION,
					1,
					"hat_role_mining2",
					"beached_skillbadge_archeology_2",
					BSkillGroups.PRECISION_ID,
					[
						BSkillPerks.CanFindMoreTreasures,
						BSkillPerks.IncreasePrecisionMedium
					],
					[
						ARCHEOLOGY_ID,
						__instance.Mining1.Id
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			crystallography = __instance.Add(new Skill(
					CRYSTALLOGRAPHY_ID,
					STRINGS.DUPLICANTS.ROLES.CRYSTALLOGRAPHY.NAME,
					STRINGS.DUPLICANTS.ROLES.CRYSTALLOGRAPHY.DESCRIPTION,
					1,
					"hat_role_mining2",
					"beached_skillbadge_archeology_2",
					BSkillGroups.PRECISION_ID,
					[
						BSkillPerks.CanSafelyHarvestClusters,
						BSkillPerks.CanCutGems,
						BSkillPerks.IncreasePrecisionLarge,
					],
					[
						ARCHEOLOGY_ID
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			geoChemistry = __instance.Add(new Skill(
					GEOCHEMISTRY_ID,
					STRINGS.DUPLICANTS.ROLES.GEOCHEMISTRY.NAME,
					STRINGS.DUPLICANTS.ROLES.GEOCHEMISTRY.DESCRIPTION,
					2,
					"hat_role_mining2",
					"beached_skillbadge_archeology_3",
					BSkillGroups.PRECISION_ID,
					[
						BSkillPerks.CanAnalyzeClusters,
						BSkillPerks.CanSynthetizeClusters
					],
					[
						CRYSTALLOGRAPHY_ID,
						__instance.Researching2.Id
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			aquaCulture1 = __instance.Add(new Skill(
					AQUACULTURE1_ID,
					STRINGS.DUPLICANTS.ROLES.AQUACULTURE1.NAME,
					STRINGS.DUPLICANTS.ROLES.AQUACULTURE1.DESCRIPTION,
					1,
					"hat_role_mining2",
					"beached_skillbadge_role_aquaculture2",
					Db.Get().SkillGroups.Farming.Id,
					[
						BSkillPerks.AquaCulture1
					],
					[
						__instance.Farming1.Id
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			aquaCulture2 = __instance.Add(new Skill(
					AQUACULTURE2_ID,
					STRINGS.DUPLICANTS.ROLES.AQUACULTURE2.NAME,
					STRINGS.DUPLICANTS.ROLES.AQUACULTURE2.DESCRIPTION,
					2,
					"hat_role_mining2",
					"beached_skillbadge_role_aquaculture3",
					Db.Get().SkillGroups.Farming.Id,
					[
						BSkillPerks.AquaCulture2
					],
					[
						AQUACULTURE1_ID
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			animalHandling = __instance.Add(new Skill(
					ANIMALHANDLING_ID,
					STRINGS.DUPLICANTS.ROLES.ANIMALHANDLING.NAME,
					STRINGS.DUPLICANTS.ROLES.ANIMALHANDLING.DESCRIPTION,
					0,
					"hat_role_mining2",
					"beached_skillbadge_role_pets1",
					Db.Get().SkillGroups.Ranching.Id,
					[
						BSkillPerks.AnimalHandling
					],
					null,
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			makiTraining1 = __instance.Add(new Skill(
					MAKITRAINING1_ID,
					STRINGS.DUPLICANTS.ROLES.MAKITRAINER1.NAME,
					STRINGS.DUPLICANTS.ROLES.MAKITRAINER1.DESCRIPTION,
					1,
					"hat_role_mining2",
					"beached_skillbadge_role_pets2",
					Db.Get().SkillGroups.Ranching.Id,
					[
						BSkillPerks.MakiTrainer1
					],
					[
						ANIMALHANDLING_ID
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			makiTraining2 = __instance.Add(new Skill(
					MAKITRAINING2_ID,
					STRINGS.DUPLICANTS.ROLES.MAKITRAINER2.NAME,
					STRINGS.DUPLICANTS.ROLES.MAKITRAINER2.DESCRIPTION,
					2,
					"hat_role_mining2",
					"beached_skillbadge_role_pets3",
					Db.Get().SkillGroups.Ranching.Id,
					[
						BSkillPerks.MakiTrainer2
					],
					[
						MAKITRAINING1_ID
					],
					BaseMinionConfig.GetMinionIDForModel(GameTags.Minions.Models.Standard),
					null,
					null));

			// Moves ranching to Animal Handling tree
			__instance.Ranching1.priorSkills =
			[
				ANIMALHANDLING_ID
			];
		}
	}
}
