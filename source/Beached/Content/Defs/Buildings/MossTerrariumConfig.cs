using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class MossTerrariumConfig : IBuildingConfig
	{
		public const string ID = "Beached_MossTerrarium";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				2,
				3,
				"beached_mossterrarium_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				[200f, 50f],
				[MATERIALS.BUILDABLERAW, BTags.BuildingMaterials.moss.ToString()],
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				NOISE_POLLUTION.NONE);

			return def;
		}



		public override void DoPostConfigureComplete(GameObject go)
		{

		}
	}
}
