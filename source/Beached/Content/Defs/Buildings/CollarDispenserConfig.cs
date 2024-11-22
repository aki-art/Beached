using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class CollarDispenserConfig : IBuildingConfig
	{
		public const string ID = "Beached_CollarDispenser";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"object_dispenser_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE,
				NOISE_POLLUTION.NONE);

			def.POIUnlockable = true;

			return def;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			var treeFilterable = go.AddOrGet<TreeFilterable>();
			treeFilterable.uiHeight = TreeFilterable.UISideScreenHeight.Short;

			var flatFilterable = go.AddOrGet<SimpleFlatFilterable>();
			flatFilterable.headerText = "test";

			var dispenser = go.AddOrGet<CollarDispenser>();

			go.AddComponent<Storage>();
		}
	}
}
