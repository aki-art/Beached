using Beached.Content.Defs.Items;
using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class SandySeashellsConfig : IMultiEntityConfig
	{
		public const string SEASHELL = "Beached_SandySeashell";
		public const string SLICKSHELL = "Beached_SandySlickshell";

		public List<GameObject> CreatePrefabs()
		{
			return new()
			{
				CreateShell(
					SEASHELL,
					STRINGS.MISC.BEACHED_SANDY_SEASHELL.NAME,
					STRINGS.MISC.BEACHED_SANDY_SEASHELL.DESCRIPTION,
					"beached_sandy_seashell_kanim",
					SeaShellConfig.ID),
				CreateShell(
					SLICKSHELL,
					STRINGS.MISC.BEACHED_SANDY_SLICKSHELL.NAME,
					STRINGS.MISC.BEACHED_SANDY_SLICKSHELL.DESCRIPTION,
					"beached_sandy_slickshell_kanim",
					SeaShellConfig.ID)
			};
		}

		private static GameObject CreateShell(string ID, string name, string description, string anim, string harvestTag)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				name,
				description,
				30f,
				Assets.GetAnim(anim),
				"object",
				Grid.SceneLayer.Creatures,
				1,
				1,
				DECOR.BONUS.TIER1,
				NOISE_POLLUTION.NONE,
				SimHashes.Lime);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
			{
				ObjectLayer.Building
			};
			prefab.AddOrGet<Prioritizable>();
			prefab.AddOrGet<SandySeaShell>();
			prefab.AddOrGet<UprootedMonitor>();
			prefab.AddComponent<HarvestDesignatable>();
			prefab.AddComponent<Harvestable>();
			prefab.AddOrGet<SeedProducer>().Configure(harvestTag, SeedProducer.ProductionType.DigOnly);
			prefab.AddOrGet<BasicForagePlantPlanted>();
			prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;

			return prefab;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
