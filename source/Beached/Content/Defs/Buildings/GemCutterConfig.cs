using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class GemCutterConfig : IBuildingConfig
	{
		public const string ID = "Beached_GemCutter";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				2,
				"beached_gemcutter_kanim",
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
				Assets.GetAnim((HashedString) "anim_idle_fastfeet_kanim")
			];

			Prioritizable.AddRef(go);
			BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
			ConfigureRecipes();
		}

		private void ConfigureRecipes()
		{
			var standardTime = 60f;

			// tier 1
			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_HEMATITENECKLACE.DESCRIPTION, standardTime)
				.Input(SimHashes.IronOre.CreateTag(), 50f)
				.Output(HematiteNecklaceConfig.ID, 1)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(5)
				.Build();

			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_ZEOLITEPENDANT.DESCRIPTION, standardTime)
				.Input(Elements.zeolite.CreateTag(), 50f)
				.Output(ZeolitePendantConfig.ID, 1)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(10)
				.Build();

			// tier 2
			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_HADEANZIRCONAMULET.DESCRIPTION, standardTime)
				.Input(RareGemsConfig.HADEAN_ZIRCON, 1f)
				.Input(BTags.Groups.preciousMetals, 50f)
				.Output(HadeanZirconAmuletConfig.ID, 1)
				.SortOrder(100)
				.Build();

			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.DESCRIPTION, standardTime)
				.Input(RareGemsConfig.MAXIXE, 1f)
				.Input(BTags.Groups.preciousMetals, 50f)
				.Output(MaxixePendantConfig.ID, 1)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(100)
				.Build();

			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.DESCRIPTION, standardTime)
				.Input(RareGemsConfig.STRANGE_MATTER, 1f)
				.Input(BTags.Groups.preciousMetals, 50f)
				.Output(StrangeMatterAmuletConfig.ID, 1)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(110)
				.Build();

			RecipeBuilder.Create(ID, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_PEARLNECKLACE.DESCRIPTION, standardTime)
				.Input(RareGemsConfig.MOTHER_PEARL, 1f)
				.Input(Elements.pearl.CreateTag(), 50f)
				.Output(PearlNecklaceConfig.ID, 1)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
				.SortOrder(110)
				.Build();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().prefabSpawnFn += game_object =>
			{
				var workable = game_object.GetComponent<ComplexFabricatorWorkable>();
				workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
				workable.AttributeConverter = BAttributeConverters.precisionSpeed;
				workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
				workable.SkillExperienceSkillGroup = BSkillGroups.PRECISION_ID;
				workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
				workable.requiredSkillPerk = BSkillPerks.CAN_CUT_GEMS_ID;
			};
		}
	}
}
