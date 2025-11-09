/*using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	internal class MediumWattageWireConfig : BaseWireConfig
	{
		public const string ID = "Beached_MediumWattageWire";

		public const int WATTAGE = 4000;
		public static readonly Wire.WattageRating W4000 = (Wire.WattageRating)WATTAGE;

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"beached_medium_wattate_wire_kanim",
				10,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0,
				[25f, 5f],
				[MATERIALS.METAL, BTags.BuildingMaterials.rubber.ToString()],
				1600f,
				BuildLocationRule.Anywhere,
				BUILDINGS.DECOR.PENALTY.TIER0,
				NOISE_POLLUTION.NONE);

			buildingDef.ThermalConductivity = 0.1f;
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.ObjectLayer = ObjectLayer.Wire;
			buildingDef.TileLayer = ObjectLayer.WireTile;
			buildingDef.ReplacementLayer = ObjectLayer.ReplacementWire;
			buildingDef.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			buildingDef.AudioSize = AUDIO.SIZE.SMALL;
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = Grid.SceneLayer.Wires;
			buildingDef.isKAnimTile = true;
			buildingDef.isUtility = true;
			buildingDef.DragBuild = true;

			buildingDef.AddSearchTerms(global::STRINGS.SEARCH_TERMS.POWER);
			buildingDef.AddSearchTerms(global::STRINGS.SEARCH_TERMS.WIRE);

			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, ID);

			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			/// <see cref="Patches.WirePatch.Wire_GetMaxWattageAsFloat_Patch"/>
			DoPostConfigureComplete(W4000, go);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
		}
	}
}
*/