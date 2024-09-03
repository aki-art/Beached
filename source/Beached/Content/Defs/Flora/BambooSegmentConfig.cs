using Beached.Content.Scripts;
using Beached.Content.Scripts.SegmentedEntities;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Flora
{
	public class BambooSegmentConfig : IEntityConfig
	{
		public const string ID = "Beached_BambooSegment";
		private static readonly List<KAnimHashedString> liveSegmentAnims =
		[
			"leaf_a",
			"leaf_b",
			"leaf_c",
		];

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"segment",//STRINGS.CREATURES.SPECIES.BAMBOO.NAME,
				STRINGS.CREATURES.SPECIES.BEACHED_BAMBOO.DESC,
				100f,
				Assets.GetAnim("beached_bamboo_kanim"),
				"idle",
				Grid.SceneLayer.BuildingBack,
				1,
				1,
				DECOR.BONUS.TIER1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers =
			[
				ObjectLayer.Building,
				ObjectLayer.Plants
			];

			var randomAnimSelector = prefab.AddOrGet<RandomSymbolVisible>();
			randomAnimSelector.targetSymbols = liveSegmentAnims;
			randomAnimSelector.minVisibleSymbols = 1;
			randomAnimSelector.maxVisibleSymbols = 3;
			/*
						var stackable = prefab.AddOrGet<Stackable>();
						stackable.stackTag = BTags.bamboo;
						stackable.animFileName = "beached_bamboo";
						stackable.allowMerging = true;
						stackable.lethalHighTemperatureK = GameUtil.GetTemperatureConvertedToKelvin(70, GameUtil.TemperatureUnit.Celsius);*/

			/*			var segment = prefab.AddOrGet<LongPlantSegment>();
						segment.animFileRoot = "beached_bamboo";
						segment.isRoot = false;
						segment.checkDirectionOnSpawn = Direction.Down;
			*/
			var ladder = prefab.AddOrGet<Ladder>();
			ladder.isPole = true;
			ladder.downwardsMovementSpeedMultiplier = 1.5f;
			ladder.upwardsMovementSpeedMultiplier = 0.5f;

			prefab.AddOrGet<EntitySegment>();

			prefab.AddTag(BTags.bamboo);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
