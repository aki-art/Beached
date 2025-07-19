using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.Plant;
using System;
using UnityEngine;

namespace Beached.Content.Defs.Flora.Gnawica
{
	public class GnawicaStemConfig : IEntityConfig
	{
		public const string ID = "Beached_GnawicaStem";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICASTALK.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_GNAWICASTALK.DESC,
				30f,
				Assets.GetAnim("beached_gnawica_stem_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				1,
				1,
				TUNING.DECOR.BONUS.TIER2);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 1f;

			var gnawicaStalk = prefab.AddOrGet<GnawicaStalk>();
			gnawicaStalk.connectionTag = ID;
			gnawicaStalk.objectLayer = (int)ObjectLayer.Building;

			prefab.AddOrGet<CodexEntryRedirector>().CodexID = "GNAWICA";

			var occupyArea = prefab.AddOrGet<OccupyArea>();
			occupyArea.objectLayers = [ObjectLayer.Building];

			prefab.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingBack);

			var piece = prefab.AddComponent<MultiPartPlantPiece>();
			piece.width = 1;
			piece.height = 1;
			piece.connectionPoint = CellOffset.none;

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject _) { }

		public void OnSpawn(GameObject _) { }
	}
}
