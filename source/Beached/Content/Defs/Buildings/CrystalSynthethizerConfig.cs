using Beached.Content.Defs.Entities;
using Beached.Content.ModDb;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class CrystalSynthetizerConfig : IBuildingConfig
	{
		public const string ID = "Beached_CrystalSynthetizer";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				2,
				"craftingStation_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				default);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 120f;
			def.ViewMode = OverlayModes.Power.ID;
			def.PowerInputOffset = new CellOffset(1, 0);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<Prioritizable>();

			var fabricator = go.AddOrGet<ComplexFabricator>();
			fabricator.heatedTemperature = 318.15f;
			fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();
			go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims =
			[
				Assets.GetAnim("anim_idle_fastfeet_kanim")
			];

			Prioritizable.AddRef(go);
			BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
			ConfigureRecipes();
		}

		private void ConfigureRecipes()
		{
			float standardTime = 80f;

			RecipeBuilder.Create(ID, STRINGS.ENTITIES.BEACHED_CRYSTALS.BEACHED_ZEOLITECRYSTAL.DESCRIPTION, standardTime)
				.Input(Elements.zeolite.CreateTag(), 200f)
				.Input(Elements.calcium.CreateTag(), 50f)
				.Output(CrystalConfig.GetClusterId(CrystalConfig.ZEOLITE))
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(5)
				.Build();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().prefabSpawnFn += game_object =>
			{
				var workable = game_object.GetComponent<ComplexFabricatorWorkable>();
				workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
				workable.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
				workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
				workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
				workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
				workable.requiredSkillPerk = BSkillPerks.CAN_ANALYZE_CLUSTERS_ID;
			};
		}
	}
}
