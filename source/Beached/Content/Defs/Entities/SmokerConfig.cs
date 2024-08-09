using Beached.Content.Scripts.Entities;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class SmokerConfig : IEntityConfig
	{
		public const string ID = "Beached_Smoker";


		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Smoker",
				"Continously emits hot Carbon Dioxide.",
				100f,
				Assets.GetAnim("beached_smoker_kanim"),
				"idle",
				Grid.SceneLayer.Building,
				3,
				3,
				DECOR.NONE,
				NOISE_POLLUTION.NOISY.TIER1,
				SimHashes.Unobtanium);

			var emitter = prefab.AddComponent<ElementEmitter>();
			emitter.maxPressure = 2f;
			emitter.outputElement = new(
				0.01f,
				SimHashes.CarbonDioxide,
				MiscUtil.CelsiusToKelvin(180),
				false,
				false,
				0.5f,
				2.5f);
			emitter.showDescriptor = true;
			emitter.emitRange = 1;
			emitter.maxPressure = 10f;

			prefab.AddComponent<Smoker>();
			prefab.AddComponent<Demolishable>();

			return prefab;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
	}
}
