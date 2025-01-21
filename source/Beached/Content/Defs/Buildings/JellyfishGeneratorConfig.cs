using Beached.Content.Scripts.Entities.AI;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
	public class JellyfishGeneratorConfig : IBuildingConfig
	{
		public const string ID = "Beached_JellyfishGenerator";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"egg_caterpillar_kanim",
				1000,
				10f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.ALL_METALS,
				9999f,
				BuildLocationRule.NotInTiles,
				BUILDINGS.DECOR.NONE,
				default);

			def.GeneratorWattageRating = 1600f;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;
			def.ExhaustKilowattsWhenActive = 2f;
			def.SelfHeatKilowattsWhenActive = 4f;
			def.Overheatable = false;
			def.Floodable = false;
			def.OverheatTemperature = 423.15f;
			def.PermittedRotations = PermittedRotations.FlipV;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			def.RequiresPowerOutput = true;
			def.PowerOutputOffset = new CellOffset(0, 0);
			def.PlayConstructionSounds = false;
			def.ShowInBuildMenu = false;

			return def;
		}

		public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefabTag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefabTag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<CritterGeneratorBuilding>().powerDistributionOrder = 9;
			go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
			go.AddOrGet<Modifiers>();
			go.AddOrGet<Effects>();
			go.GetComponent<KSelectable>().IsSelectable = false;
		}
	}
}
