using Beached.Content.ModDb;
using Beached.Content.Scripts;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class SandBoxConfig : IBuildingConfig
	{
		public const string ID = "Beached_Sandbox";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
			ID,
			2,
			2,
			"beached_sandbox_base_kanim",
			BUILDINGS.HITPOINTS.TIER2,
			BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
			[200f],
			[BTags.BuildingMaterials.sand.ToString()],
			BUILDINGS.MELTING_POINT_KELVIN.TIER1,
			BuildLocationRule.OnFloor,
			DECOR.BONUS.TIER2,
			NOISE_POLLUTION.NONE);

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
			go.AddTag(RoomConstraints.ConstraintTags.RecBuilding);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var sandBox = go.AddComponent<SandBox>();
			sandBox.defaultAnimName = "idle";
			sandBox.degradationTime = CONSTS.CYCLE_LENGTH;
			sandBox.specificEffect = BEffects.SANDBOX;
			sandBox.trackingEffect = BEffects.SANDBOX_RECENT;
		}
	}
}
