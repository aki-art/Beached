using Beached.Content.ModDb;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Medicines
{
	public class GummyPipsConfig : IEntityConfig
	{
		public const string ID = "Beached_GummyPips";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.PILLS.BEACHED_GUMMYPIPS.NAME,
				STRINGS.ITEMS.PILLS.BEACHED_GUMMYPIPS.DESC,
				1f,
				true,
				Assets.GetAnim("beached_gummypips_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.45f,
				true,
				0,
				SimHashes.Creature,
				null);

			var medicineInfo = new MedicineInfo(
				ID,
				BEffects.GUMMY_PIPS_BUFF,
				MedicineInfo.MedicineType.Booster,
				null,
				null);

			EntityTemplates.ExtendEntityToMedicine(prefab, medicineInfo);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
