using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class MirrorConfig : IBuildingConfig
	{
		public const string ID = "Beached_Mirror";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				3,
				"beached_mirror_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.REFINED_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NOISY.TIER1);

			def.Floodable = false;
			def.SceneLayer = Grid.SceneLayer.BuildingBack;
			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<Mirror>();
		}
	}
}
