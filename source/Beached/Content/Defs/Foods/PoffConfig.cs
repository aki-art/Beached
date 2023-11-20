using Beached.Content.ModDb;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs.Foods
{
	public class PoffConfig : IMultiEntityConfig
	{
		public static Dictionary<SimHashes, (string raw, string cooked)> poffLookup = new();

		public static List<PoffInfo> configs = new()
		{
			new PoffInfo(SimHashes.Oxygen)
			{
				qualityRaw = 0,
				caloriesRaw = 800f,
				qualityCooked = 1,
				caloriesCooked = 1000f
			},
			new PoffInfo(Elements.saltyOxygen)
			{
				qualityRaw = 1,
				caloriesRaw = 800f,
				qualityCooked = 2,
				caloriesCooked = 1000f
			}.Effects(DlcManager.AVAILABLE_ALL_VERSIONS, BEffects.OCEAN_BREEZE),
			new PoffInfo(Elements.nitrogen)
			{
				qualityRaw = 0,
				caloriesRaw = 700f,
				qualityCooked = 0,
				caloriesCooked = 900f
			}.Effects(DlcManager.AVAILABLE_ALL_VERSIONS, BEffects.POFF_CLEANEDTASTEBUDS),
			new PoffInfo(SimHashes.Helium)
			{
				qualityRaw = 1,
				caloriesRaw = 900f,
				qualityCooked = 3,
				caloriesCooked = 1100f
			}.Effects(DlcManager.AVAILABLE_ALL_VERSIONS, BEffects.POFF_HELIUM),
			new PoffInfo(Elements.ammonia)
			{
				qualityRaw = 0,
				caloriesRaw = 800,
				spoilTime = FOOD.SPOIL_TIME.VERYSLOW,
				qualityCooked = 1,
				caloriesCooked = 1100,
			},
		};

		public class PoffInfo
		{
			public string rawID;
			public string cookedID;

			public SimHashes elementID;
			public Color color = Color.white;
			public string dlcId = DlcManager.VANILLA_ID;
			public string animName;
			public bool disallowCooking;
			public bool canRot = true;

			public int qualityRaw, qualityCooked;
			public float caloriesRaw, caloriesCooked;
			public List<string> effectRaw, effectCooked;

			public float spoilTime = FOOD.SPOIL_TIME.DEFAULT, spoilTimeCooked = -1;
			public float rotTemperature = FOOD.DEFAULT_ROT_TEMPERATURE, rotTemperatureCooked = -1;
			public float preserveTemperature = FOOD.DEFAULT_PRESERVE_TEMPERATURE, preserveTemperatureCooked = -1;

			public PoffInfo(SimHashes elementID, string animName = "barbeque_kanim")
			{
				this.elementID = elementID;
				this.animName = animName;
				rawID = GetRawId(elementID);
				cookedID = GetCookedId(elementID);
				poffLookup[elementID] = (rawID, cookedID);
			}

			public PoffInfo Effects(string[] dlcIds, params string[] effects)
			{
				if (DlcManager.IsDlcListValidForCurrentContent(dlcIds))
				{
					effectRaw ??= new();
					effectRaw.AddRange(effects);
					effectCooked ??= new();
					effectCooked.AddRange(effects);
				}

				return this;
			}
		}

		public static string GetRawId(SimHashes elementID) => $"Beached_{elementID}_Poff_Raw";

		public static string GetCookedId(SimHashes elementID) => $"Beached_{elementID}_Poff_Cooked";

		public List<GameObject> CreatePrefabs()
		{
			var prefabs = new List<GameObject>();

			foreach (var config in configs)
			{
				var element = ElementLoader.FindElementByHash(config.elementID);

				if (element == null)
					continue;

				config.color = element.substance.colour;

				var foodInfoRaw = new FoodInfo(
					config.rawID,
					config.dlcId,
					config.caloriesRaw,
					config.qualityRaw,
					config.preserveTemperature,
					config.rotTemperature,
					config.spoilTime,
					config.canRot);

				var rawPrefab = BEntityTemplates.CreateFood(
					config.rawID,
					config.animName,
					0.7f,
					0.5f,
					foodInfoRaw);

				if (config.effectRaw != null)
				{
					foodInfoRaw.AddEffects(config.effectRaw, DlcManager.AVAILABLE_ALL_VERSIONS); // already filtered by DLC
				}

				prefabs.Add(rawPrefab);

				if (element.id == Elements.nitrogen)
					rawPrefab.AddTag(BTags.palateCleanserFood);

				if (!config.disallowCooking)
				{
					var foodInfoCooked = new FoodInfo(
						config.cookedID,
						config.dlcId,
						config.caloriesCooked,
						config.qualityCooked,
						config.preserveTemperatureCooked == -1 ? config.preserveTemperature : config.preserveTemperatureCooked,
						config.rotTemperatureCooked == -1 ? config.rotTemperature : config.rotTemperatureCooked,
						config.spoilTimeCooked == -1 ? config.spoilTime : config.spoilTimeCooked,
						config.canRot);

					var cookedPrefab = BEntityTemplates.CreateFood(
						config.cookedID,
						config.animName,
						0.7f,
						0.5f,
						foodInfoCooked);

					if (config.effectCooked != null)
					{
						foodInfoCooked.AddEffects(config.effectCooked, DlcManager.AVAILABLE_ALL_VERSIONS); // already filtered by DLC
					}

					if (element.id == Elements.nitrogen)
						cookedPrefab.AddTag(BTags.palateCleanserFood);

					prefabs.Add(cookedPrefab);
				}
			}

			return prefabs;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
