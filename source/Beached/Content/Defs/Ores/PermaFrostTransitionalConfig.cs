using Beached.Content.ModDb.Germs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Ores
{
	public class PermaFrostTransitionalConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.permaFrost_Transitional;

		public static float disasePartition = 0.5f;

		// 0, 0 (self) last so any leftover germs are put on the liquid output
		private static readonly List<CellOffset> disaseEmissionOffsets =
			[
				new CellOffset(1, 0),
				new CellOffset(0, 1),
				new CellOffset(-1, 0),
				new CellOffset(0, -1),
				new CellOffset(0, 0)
			];

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID, [BTags.OniTwitch_surpriseBoxForceDisabled]);

			if (prefab.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.prefabSpawnFn += OnPrefabSpawn;

			return prefab;
		}

		private static IEnumerator SplitNextFrame(GameObject go)
		{
			yield return SequenceUtil.WaitForNextFrame;

			if (go == null || go.IsNullOrDestroyed())
				yield return null;

			var fossil = ElementLoader.FindElementByHash(SimHashes.Fossil);
			if (go.TryGetComponent(out PrimaryElement primaryElement))
			{
				var disaseCount = (int)(primaryElement.DiseaseCount * disasePartition);
				var origin = Grid.PosToCell(go);
				var cellsToGo = disaseEmissionOffsets.Count + 1;

				foreach (var offset in disaseEmissionOffsets)
				{
					var cell = Grid.OffsetCell(origin, offset);
					var existingDisease = Grid.DiseaseIdx[cell];

					var partialCount = disaseCount / cellsToGo;
					cellsToGo--;

					if (Grid.IsValidCellInWorld(cell, go.GetMyWorldId()))
						continue;

					if (existingDisease != byte.MaxValue && existingDisease != IceWrathGerms.cachedRunTimeIndex)
					{
						continue;
					}

					if (!(Grid.IsGas(cell) || Grid.IsLiquid(cell)))
						continue;

					SimMessages.ModifyDiseaseOnCell(cell, IceWrathGerms.cachedRunTimeIndex, partialCount + Grid.DiseaseCount[cell]);
					disaseCount -= partialCount;
				}

				fossil.substance.SpawnResource(
					go.transform.position,
					primaryElement.Mass,
					primaryElement.Temperature,
					primaryElement.DiseaseIdx,
					(int)(primaryElement.DiseaseCount * (1f - disasePartition)));
			}

			Util.KDestroyGameObject(go);

			yield return null;
		}

		private void OnPrefabSpawn(GameObject go)
		{
			go.GetComponent<KPrefabID>().StartCoroutine(SplitNextFrame(go));
		}
	}
}
