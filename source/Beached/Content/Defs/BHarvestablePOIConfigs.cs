using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content
{
	public class BHarvestablePOIConfigs : IMultiEntityConfig
	{
		// Beached_HarvestableSpacePOI_ gets inserted before these for the asteroid field
		public const string
			AMMONITE = "Ammonite",
			PEARLESCENT = "PearlescentAsteroidField",
			MUCUS = "SlimyField",
			ZIRCONIUM = "Zirconium",
			SHATTERED_METEOR = "ShatteredMeteor";

		public List<GameObject> CreatePrefabs()
		{
			return new List<GameObject>()
			{
				new HarvestablePOIBuilder(PEARLESCENT, "beached_pearlescent_harvestable_asteroid_kanim")
					.Capacity(54000, 81000)
					.Recharge(30000, 60000)
					.Element(SimHashes.Sand, 1.2f)
					.Element(Elements.pearl, 0.2f)
					.Element(Elements.bismuth, 0.7f)
					.Element(Elements.siltStone, 1f)
					.Build(),

				new HarvestablePOIBuilder(AMMONITE, "beached_ammonite_harvestable_poi_kanim")
					.Capacity(54000, 81000)
					.Recharge(30000, 60000)
					.Element(SimHashes.Lime, 1f)
					.Element(Elements.rot, 1f)
					.Build()
			};
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
