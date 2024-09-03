using Beached.Content.Scripts.Buildings;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class ChimeConfig : IBuildingConfig
	{
		public const string ID = "Beached_Chime";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"beached_chime_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				[
					10f,
					6f
				],
				[
					MATERIALS.BUILDABLERAW,
					BTags.BuildingMaterials.chime.ToString()
				],
				BUILDINGS.MELTING_POINT_KELVIN.TIER2,
				BuildLocationRule.OnCeiling,
				DECOR.BONUS.TIER2,
				NOISE_POLLUTION.NOISY.TIER2);

			def.PermittedRotations = PermittedRotations.FlipH;

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<Chime>();
			//go.AddOrGet<CustomAudioPlayer>();
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();
		}
	}
}
