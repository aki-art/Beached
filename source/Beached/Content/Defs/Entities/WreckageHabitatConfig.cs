using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	internal class WreckageHabitatConfig : IEntityConfig
	{
		public const string ID = "Beached_WreackageHabitat";
		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Wreckage",
				"...",
				400,
				Assets.GetAnim("beached_wreckage_kanim"),
				"habitat",
				Grid.SceneLayer.BuildingBack,
				7,
				6,
				DECOR.NONE,
				NOISE_POLLUTION.NONE,
				SimHashes.Rust,

				null,
				MiscUtil.CelsiusToKelvin(30));

			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];

			var workable = prefab.AddOrGet<Workable>();
			workable.synchronizeAnims = false;
			workable.resetProgressOnStop = true;

			var locker = prefab.AddComponent<SetLocker>();
			locker.numDataBanks = [1, 2];
			locker.dropOffset = new Vector2I(0, 1);

			LoreBearerUtil.AddLoreTo(prefab, LoreBearerUtil.UnlockSpecificEntry("journal_magazine", STRINGS.UI.BEACHED_USERMENUACTIONS.READLORE.SEARCH_WRECKEDHABITAT));

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
			if (inst.TryGetComponent(out SetLocker locker))
			{
				locker.possible_contents_ids = GetContents();
				locker.ChooseContents();
			}
		}

		public static string[][] GetContents()
		{
			return
			[
				[
					PrickleFlowerConfig.SEED_ID,
					PrickleFlowerConfig.SEED_ID,
					FieldRationConfig.ID,
					FieldRationConfig.ID,
					FieldRationConfig.ID
				]
			];
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
