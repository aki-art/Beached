using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class SpinorilaConfig : IEntityConfig
	{
		public const string ID = "Beached_Spinorila";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_SPINORILA.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_SPINORILA.DESC,
				100f,
				Assets.GetAnim("beached_spinorila_kanim"),
				"idle_full",
				Grid.SceneLayer.BuildingBack,
				3,
				3,
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

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
