using Beached.Content.ModDb.Germs;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	internal class LimpetRockConfig : IEntityConfig
	{
		public const string ID = "Beached_LimpetRock";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.ENTITIES.BEACHED_LIMPETROCK.NAME,
				STRINGS.ENTITIES.BEACHED_LIMPETROCK.DESCRIPTION,
				200,
				Assets.GetAnim("beached_limpet_rock_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				3,
				2,
				DECOR.NONE,
				element: SimHashes.SedimentaryRock);

			var diseaseDropper = prefab.AddOrGetDef<DiseaseDropper.Def>();
			diseaseDropper.diseaseIdx = Db.Get().Diseases.GetIndex(LimpetEggGerms.ID);
			diseaseDropper.emitFrequency = 1f;
			diseaseDropper.averageEmitPerSecond = 1000;
			diseaseDropper.singleEmitQuantity = 100000;

			prefab.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = LimpetEggGerms.ID;

			prefab.AddOrGet<Demolishable>();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
