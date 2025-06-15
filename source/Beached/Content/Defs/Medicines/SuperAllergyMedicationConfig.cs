using Beached.Content.ModDb;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Medicines
{
	public class SuperAllergyMedicationConfig : IEntityConfig
	{
		public const string ID = "Beached_SuperAllergyMedication";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.PILLS.BEACHED_SUPERALLERGYMEDICATION.NAME,
				STRINGS.ITEMS.PILLS.BEACHED_SUPERALLERGYMEDICATION.DESC,
				1f,
				true,
				Assets.GetAnim("pill_allergies_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true,
				0,
				SimHashes.Creature,
				null);

			var medicineInfo = new MedicineInfo(
				ID,
				BEffects.SUPER_ALLERGY_MED,
				MedicineInfo.MedicineType.CureSpecific,
				null,
				["Allergies"]);

			EntityTemplates.ExtendEntityToMedicine(prefab, medicineInfo);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
