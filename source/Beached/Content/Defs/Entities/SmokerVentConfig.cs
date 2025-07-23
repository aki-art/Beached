using Beached.Content.Scripts.Entities;
using System;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class SmokerVentConfig : IEntityConfig
	{
		public const string ID = "Beached_SmokerVent";


		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.BEACHED_SMOKERVENT.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_SMOKERVENT.DESCRIPTION,
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

			prefab.AddComponent<Beached_Smoker>();
			prefab.AddComponent<Demolishable>();
			prefab.AddOrGet<BuildingAttachPoint>().points =
			[
				new BuildingAttachPoint.HardPoint(CellOffset.none, BTags.buildingAttachmentSmoker,  null)
			];

			return prefab;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }

		[Obsolete]
		public string[] GetDlcIds() => null;
	}
}
