using Beached.Content.Overlays;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class DevElectricEmitterConfig : IBuildingConfig
	{
		public const string ID = "Beached_DevElectricEmitter";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_dev_electric_emitter_kanim",
				100,
				3f,
				TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				MATERIALS.ALL_METALS,
				2400f,
				BuildLocationRule.Anywhere,
				default,
				default);

			def.ViewMode = ConductionOverlayMode.ID;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "large";
			def.Floodable = false;
			def.Overheatable = false;
			def.DebugOnly = true;

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<ElectricEmitter>();
			go.AddComponent<DevElectricEmitter>();
		}
	}
}
