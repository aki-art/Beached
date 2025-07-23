using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.AI;
using Beached.Integration;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class CrystalConfig : IMultiEntityConfig
	{
		public const string
			TEST_ID = "Beached_Crystal",

			ZEOLITE = "Beached_ZeoliteCrystal",
			KARAIRITE = "Beached_KatairiteCrystal",
			AQUAMARINE = "Beached_AquamarineCrystal",
			SUCROSE = "Beached_SucroseCrystal",
			SALT = "Beached_SaltCrystal",
			OXYROCK = "Beached_OxyRockCrystal",

			// not implemented
			SELENITE = "Beached_SeleniteCrystal",
			ZIRCON = "Beached_ZirconCrystal",
			NITRE = "Beached_NitreCrystal",
			BORAX = "Beached_ChemicalProcessing_BoraxCrystal";

		public static string GetClusterId(string crystal) => $"{crystal}Cluster";
		public List<GameObject> CreatePrefabs()
		{
			var result = new List<GameObject>()
			{
				CreatePrefab(TEST_ID, SimHashes.Obsidian, "test_crystal_b_kanim", null, 4, 10, 200),
				CreatePrefab(ZEOLITE, Elements.zeolite, "beached_zeolite_crystal_kanim", "beached_zeolite_cluster_kanim", 4, 30, 200),
				CreatePrefab(KARAIRITE, SimHashes.Katairite, "beached_katairite_crystal_kanim", null,  5, 10, 200),
				CreatePrefab(SUCROSE, SimHashes.Sucrose, "beached_sucrose_crystal_kanim", null,  5, 30, 200),
				CreatePrefab(SALT, SimHashes.Salt, "beached_salt_crystal_kanim", null,  5, 30, 200),
				CreatePrefab(AQUAMARINE, Elements.aquamarine, "beached_aquamarine_crystal_kanim", null,  5, 30, growthPercentPerCycle: 200),
				CreatePrefab(OXYROCK, SimHashes.OxyRock, "beached_oxylite_crystal_kanim", null,  5, 30, 200),
			};

			if (Mod.integrations.IsModPresent(Integrations.CHEMICAL_PROCESSING))
				result.Add(CreatePrefab(BORAX, Elements.ChemicalProcessing.solidBorax, "test_crystal_b_kanim", null, 4, 20, 20));

			return result;
		}

		public GameObject CreatePrefab(string ID, SimHashes elementId, string animationFile, string itemKanim, float maxLength, float yieldKgPerHarvest, float growthPercentPerCycle, Direction growth = Direction.None)
		{
			var anim = Assets.GetAnim(animationFile);

			var element = ElementLoader.FindElementByHash(elementId);

			if (element == null)
				return null;

			var prefab = EntityTemplates.CreatePlacedEntity(
			ID,
				Strings.Get($"STRINGS.ENTITIES.BEACHED_CRYSTALS.{ID.ToUpperInvariant()}.NAME"),
				Strings.Get($"STRINGS.ENTITIES.BEACHED_CRYSTALS.{ID.ToUpperInvariant()}.DESCRIPTION"),
				100f,
				anim,
				"growing",
				Grid.SceneLayer.Building,
				1,
				1,
				TUNING.DECOR.BONUS.TIER4,
				default,
				elementId);

			var modifiers = prefab.AddOrGet<Modifiers>();

			modifiers.initialAmounts.Add(BAmounts.CrystalGrowth.Id);

			prefab.AddOrGet<Effects>();
			prefab.AddOrGet<Crystal>().growthDirection = growth;

			/*	if (Mod.debugMode)
					prefab.AddOrGet<CrystalDebug>();*/
			//prefab.AddOrGet<CrystalFoundationMonitor>().needsFoundation = false;

			var growingCrystal = prefab.AddOrGet<GrowingCrystal>();
			growingCrystal.maxLength = maxLength;
			growingCrystal.defaultGrowthRatePercentPerCycle = growthPercentPerCycle / CONSTS.CYCLE_LENGTH;

			var harvestable = prefab.AddOrGet<HarvestableCrystal>();
			harvestable.crystalKgPerHarvest = yieldKgPerHarvest;
			harvestable.elementHarvested = elementId;

			if (itemKanim != null)
			{
				var clusterPrefab = BEntityTemplates.CreateAndRegisterClusterForCrystal(
					prefab,
					GetClusterId(ID),
					Assets.GetAnim(itemKanim));

				BEntityTemplates.CreateAndRegisterPreviewForCluster(
					clusterPrefab,
					ID + "Preview",
					anim);
			}

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
