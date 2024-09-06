using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class WoodCarvingConfig : IBuildingConfig
	{
		public const string ID = "Beached_WoodCarving";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
			   ID,
			   2,
			   3,
			   "beached_woodcarving_owl_kanim",
			   BUILDINGS.HITPOINTS.TIER2,
			   BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
			   BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
			   MATERIALS.WOODS,
			   BUILDINGS.MELTING_POINT_KELVIN.TIER1,
			   BuildLocationRule.OnFloor,
			   DECOR.BONUS.TIER3,
			   NOISE_POLLUTION.NONE
		   );

			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			def.BaseTimeUntilRepair = -1f;
			def.ViewMode = OverlayModes.Decor.ID;
			def.DefaultAnimState = "idle";
			def.PermittedRotations = PermittedRotations.FlipH;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<BuildingComplete>().isArtable = true;
			go.AddTag(GameTags.Decoration);
			go.AddTag(BTags.MaterialColor_noPaint);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<Sculpture>().defaultAnimName = "idle";
		}
	}
}
