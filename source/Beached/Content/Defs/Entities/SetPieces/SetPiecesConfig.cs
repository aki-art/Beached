using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.SetPieces
{
	public class SetPiecesConfig : IMultiEntityConfig
	{
		public const string
			AMMONIAVENT = "Beached_AmmoniaVentSetPiece",
			BEACH = "Beached_BeachSetPiece",
			BRAINPOI = "Beached_BrainPOISetPiece",
			FORCEFIELD_LAB = "Beached_ForceFieldLabSetPiece",
			FPP_THERMAL_VENT = "Beached_FPP_ThermalVentSetPiece",
			JEORGE_HIDEOUT = "Beached_JeorgeHideOutSetPiece",
			REEF = "Beached_ReefSetPiece",
			VAHANO_HIDEOUT = "Beached_VahanoHideOutSetPiece",
			ZEOLITE = "Beached_ZeoliteSetPiece",

			TEST = "Beached_TestSetPiece";

		public List<GameObject> CreatePrefabs()
		{
			return
			[
				CreateSetPiece(TEST, "farmtile_kanim", 11, 8, "test"),
				(ConfigureBeachSetPiece()),
				CreateSetPiece(ZEOLITE, "beached_zeolitecave_ui_kanim", 8, 8, "generic", "beached_zeolitebg")
			];
		}

		private GameObject ConfigureBeachSetPiece()
		{
			var beach = CreateSetPiece(BEACH, "farmtile_kanim", 17, 10, "beach");

			var emitter = beach.AddComponent<ElementEmitter>();
			emitter.outputElement = new ElementConverter.OutputElement(0.2f, Elements.saltyOxygen, MiscUtil.CelsiusToKelvin(29));
			emitter.emitRange = 3;
			emitter.SetEmitting(true);
			emitter.maxPressure = 2.0f;

			/*			var oxygenStorage = beach.AddComponent<Storage>();
						oxygenStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
						oxygenStorage.capacityKg = 100000;
						oxygenStorage.storageFilters = [Elements.saltyOxygen.CreateTag()];

						var elementSource = beach.AddComponent<ElementSourceVista>();
						elementSource.element = Elements.saltyOxygen;
						elementSource.initialStoredAmount = 100;
						elementSource.exchangeRatePerTile = 100;
						elementSource.depth = 5;
						elementSource.storage = oxygenStorage;
						elementSource.emissionShape = MiscUtil.MakeCellOffsetsFromMap(true, "",
							"  X  ",
							" XXX ",
							"XXOXX",
							" XXX ",
							"  X  ");*/

			return beach;
		}

		private GameObject CreateSetPiece(string ID, string uiAnim, int width, int height, string bgPrefabID, string spriteId = null)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				Strings.Get($"STRINGS.ENTITIES.SET_PIECES.{ID.ToUpperInvariant()}.NAME"),
				Strings.Get($"STRINGS.ENTITIES.SET_PIECES.{ID.ToUpperInvariant()}.DESCRIPTION"),
				100f,
				Assets.GetAnim(uiAnim),
				"",
				Grid.SceneLayer.Backwall,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2,
				additionalTags:
				[
					BTags.setPiece
				]);

			var setPiece = prefab.AddComponent<SetPiece>();
			setPiece.setPiecePrefabID = bgPrefabID;
			setPiece.width = width;
			setPiece.height = height;
			setPiece.sprite = spriteId;

			prefab.AddComponent<Vista>();

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
