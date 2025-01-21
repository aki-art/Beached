using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class MudStomperConfig : IBuildingConfig
	{
		public const string ID = "Beached_MudStomper";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				1,
				"beached_mud_stomper_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				default);

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
			RecipeBuilder.Create(ID, global::STRINGS.ELEMENTS.DIRT.DESC, 40)
				.Input(Elements.mucus.CreateTag(), 10)
				.Input(SimHashes.Sand.CreateTag(), 90)
				.Output(SimHashes.Dirt.CreateTag(), 100)
				.Build();

			if (DlcManager.IsExpansion1Active())
			{
				RecipeBuilder.Create(ID, global::STRINGS.ELEMENTS.MUD.DESC, 40)
					.Input(SimHashes.Dirt.CreateTag(), 20)
					.Input(SimHashes.Water.CreateTag(), 20)
					.Output(SimHashes.Mud.CreateTag(), 40)
					.Build();
			}
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
			};
		}
	}
}
