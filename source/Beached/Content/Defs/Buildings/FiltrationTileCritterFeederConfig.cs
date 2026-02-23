using Beached.Content.Scripts.Buildings;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class FiltrationTileCritterFeederConfig : IBuildingConfig
	{
		public const string ID = "Beached_FiltrationTileCritterFeeder";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"farmtilerotating_kanim",
				100,
				30f,
				[1f],
				MATERIALS.RAW_MINERALS,
				1600f,
				BuildLocationRule.Tile,
				BUILDINGS.DECOR.NONE,
				NOISE_POLLUTION.NONE);

			def.Overheatable = false;
			def.Floodable = false;
			def.OverheatTemperature = 423.15f;
			def.PermittedRotations = PermittedRotations.FlipV;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			def.PlayConstructionSounds = false;
			def.ShowInBuildMenu = false;

			return def;
		}


		public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefabTag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<AttachedCritterFeeder>();
			go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
			go.AddOrGet<Modifiers>();
			go.AddOrGet<Effects>();
			go.GetComponent<KSelectable>().IsSelectable = false;

			go.AddOrGet<CreatureFeeder>();
		}
	}
}
