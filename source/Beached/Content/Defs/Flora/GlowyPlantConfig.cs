using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class GlowyPlantConfig : IEntityConfig
	{
		public const string ID = "Beached_GlowyPlant";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWYPLANT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GLOWYPLANT.DESC,
				100f,
				Assets.GetAnim("beached_glowyplant_kanim"),
				"night_idle_full",
				Grid.SceneLayer.BuildingBack,
				3,
				1,
				DECOR.BONUS.TIER2,
				defaultTemperature: CREATURES.TEMPERATURE.HOT_1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				CREATURES.TEMPERATURE.FREEZING_10,
				CREATURES.TEMPERATURE.FREEZING_9,
				CREATURES.TEMPERATURE.HOT_2,
				CREATURES.TEMPERATURE.HOT_3,
				null,
				false,
				0f,
				0.15f,
				"PlantMeat",
				true,
				true,
				true,
				false,
				2400f,
				0f,
				2200f,
				ID + "Original",
				STRINGS.CREATURES.SPECIES.BEACHED_DEWPALM.NAME);

			var light = prefab.AddComponent<Light2D>();
			light.Range = 2f;
			light.shape = LightShape.Circle;
			light.Color = new Color(0, 1f, 0.6f);
			light.drawOverlay = false;
			light.Lux = 400;
			light.Offset = new Vector2(0, 0.3f);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
