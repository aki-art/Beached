using Beached.Content.Defs.Comets;
using Beached.Content.Defs.Flora;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.ClassExtensions;
using Beached.Content.Scripts.Entities.Comets;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	/* "Fake" sim extension. Rotates loaded chunks and randomly updates a few cells Minecraft style. 
     * The game does these things in a separate C++ library, which cannot be modded, as is far superior in calculation speed.
     * Lacking that, this is a compromise between custom element behavior and not tanking the player's FPS doing iz from C#. */

	/* Loading Custom C++ extensions: ran into problems with Linux and Mac systems. 
     * Linux is not as lenient with path searching as Windows is, and can't find the extra dll if it's not in the same folder as the main dll. 
     * Problem with that is that the game tries to load every DLL as a mod, and crashes if a random c++ dll is in there. 
     * 
     * Although if i release 3 separate versions of the mod for each platform, this may still be on the table */
	[SkipSaveFileSerialization]
	public class ElementInteractions : KMonoBehaviour, ISim200ms
	{
		private const int CHUNK_EDGE = 4;
		private const float FUNGAL_LIGHT_KILL_RATE = 0.5f;
		private const float CORALLIUM_SPREAD_CHANCE = 0.05f;
		private const float SHRAPNEL_SPEED = 10f;
		private const float ACID_LOSS = 0.25f;

		private static int widthInChunks;
		private static int heightInChunks;
		private static int chunkCount;

		private static ushort oxygenIdx;
		private static ushort saltWaterIdx;
		private static ushort brineIdx;
		private static ushort acidIdx;
		private static ushort coralliumIdx;
		private static ushort hydrogenIdx;
		private static Element saltyOxygen;

		public CellElementEvent SaltOffing;
		public static CellElementEvent CoralSpread = new CellElementEvent("Beached_CoralSpread", "Spreading coral by ElementInteractions", false);

		private readonly SpawnFXHashes saltFx = ModAssets.Fx.saltOff;

		private static Dictionary<byte, FungusInfo> fungusInfos = [];

		public static HashSet<int> simActiveChunks = [];

		public struct FungusInfo
		{
			public PlantableSeed plantableSeed;
			public Tag seedTag;
			public SpawnFXHashes spawnFXHashes;

			public FungusInfo(Tag seedTag, SpawnFXHashes spawnFXHashes)
			{
				this.seedTag = seedTag;
				this.spawnFXHashes = spawnFXHashes;
				var seedPrefab = Assets.TryGetPrefab(seedTag);

				if (seedPrefab != null)
					plantableSeed = seedPrefab.GetComponent<PlantableSeed>();
			}

			public readonly bool IsValid() => plantableSeed != null;
		}

		public void Sim200ms(float dt)
		{
			foreach (var worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered)
				{
					var min = worldContainer.worldOffset;
					var max = worldContainer.worldOffset + worldContainer.worldSize;

					for (int x = min.x; x <= max.x; x += CHUNK_EDGE)
					{
						for (int y = min.y; y <= max.y; y += CHUNK_EDGE)
						{
							var chunkIdx = WorldXYToChunkIdx(x, y);
							simActiveChunks.Add(chunkIdx);
						}
					}
				}
			}

			if (!enabled)
				return;

			UpdateCells();
		}

		public static int CellToChunkIdx(int cell)
		{
			Grid.CellToXY(cell, out var x, out var y);
			return WorldXYToChunkIdx(x, y);
		}

		public static int WorldXYToChunkIdx(int x, int y) => XYToChunk(x / CHUNK_EDGE, y / CHUNK_EDGE);

		public void UpdateCells()
		{
			for (var c = 0; c < chunkCount; c++)
			{
				var cell = GetRandomCellInChunk(c);
				UpdateCell(cell);
			}
		}

		private void UpdateCell(int cell)
		{
			if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
				return;

			var element = Grid.Element[cell];
			var elementIdx = element.idx;
			var diseaseCount = Grid.DiseaseCount[cell];

			if (element.IsLiquid)
			{
				UpdateSaltWater(cell, element, elementIdx);

				if (elementIdx == acidIdx)
					UpdateAcid(cell);
			}
			else if (element.IsGas)
				UpdateSpores(cell, diseaseCount);
			else if (element.idx == coralliumIdx)
				TrySpreadCorallium(cell);
		}
		private void TrySpreadCorallium(int cell)
		{
			if (Random.value < CORALLIUM_SPREAD_CHANCE)
			{
				MiscUtil.TrySpreadCoralliumToTile(cell, MiscUtil.cardinalOffsetsUnordered.GetRandom());
			}
		}

		private void UpdateAcid(int cell)
		{
			var cellBelow = Grid.CellBelow(cell);
			var element = Grid.Element[cellBelow];
			var isSolid = element.IsSolid;

			if (element.HasTag(GameTags.Metal))
			{
				var acidMass = Grid.Mass[cell];
				var shrapnelCount = Random.Range(2, 4);
				var mass = Grid.Mass[cellBelow] / shrapnelCount;
				var position = Grid.CellToPos(cell);
				var hydrogenAmount = acidMass * ACID_LOSS * 0.1f;

				var temperature = Mathf.Min(Grid.Temperature[cellBelow], 773.15f);
				SimMessages.ReplaceElement(cellBelow, SimHashes.Hydrogen, CellEventLogger.Instance.DebugTool, hydrogenAmount, temperature, byte.MaxValue, 0);

				for (var i = 0; i < shrapnelCount; i++)
				{
					var go = Util.KInstantiate(Assets.GetPrefab(ShrapnelConfig.ID), position);
					go.SetActive(true);

					var angle = (float)Random.Range(-45, 315) % 360;
					var rads = angle * Mathf.PI / 180f;

					var shrapnel = go.GetComponent<Shrapnel>();
					shrapnel.Velocity = new Vector2(-Mathf.Cos(rads), Mathf.Sin(rads)) * SHRAPNEL_SPEED;
					shrapnel.SetExplosionMass(mass);

					var primaryElement = go.GetComponent<PrimaryElement>();
					primaryElement.SetElement(element.id);
					primaryElement.Mass = mass;

					var kbac = shrapnel.GetComponent<KBatchedAnimController>();
					kbac.Rotation = (float)(-(float)angle) - 90f;
				}

				SimMessages.ConsumeMass(cell, Elements.sulfurousWater, acidMass * ACID_LOSS, 1);
				Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, cellBelow, 0);
				// TODO: sound fx
			}

			if (isSolid)
			{
				var acidVulnerability = element.AcidVulnerability();

				if (acidVulnerability > 0)
				{
					WorldDamage.Instance.ApplyDamage(cellBelow, acidVulnerability, -1);
					Game.Instance.SpawnFX(SpawnFXHashes.BleachStoneEmissionBubbles, cellBelow, 0);
				}
			}
		}

		private void UpdateSpores(int cell, int diseaseCount)
		{
			if (diseaseCount > 0)
			{
				var germIdx = Grid.DiseaseIdx[cell];

				if (fungusInfos.TryGetValue(germIdx, out var info))
				{
					if (Grid.LightCount[cell] > 0)
					{
						SimMessages.ModifyDiseaseOnCell(cell, germIdx, -GermsToKill(diseaseCount, FUNGAL_LIGHT_KILL_RATE));
						return;
					}

					var rollForMushroomGrowing = Random.value < 0.1f;
					if (rollForMushroomGrowing)
					{
						if (MiscUtil.IsNaturalCell(Grid.CellBelow(cell))
							&& !Grid.IsSolidCell(Grid.CellAbove(cell))
							&& info.IsValid()
							&& info.plantableSeed.TestSuitableGround(cell))
						{
							var seed = MiscUtil.Spawn(info.seedTag, Grid.CellToPos(cell));
							if (seed.TryGetComponent(out PlantableSeed plantableSeed))
							{
								plantableSeed.GetComponent<PlantableSeed>().TryPlant();
								Game.Instance.SpawnFX(info.spawnFXHashes, cell, 0);
							}
						}
					}
				}
			}
		}

		public static float GetFungalGermAverageChangeRateInLight()
		{
			return 5f * (1f / (CHUNK_EDGE * CHUNK_EDGE) * FUNGAL_LIGHT_KILL_RATE); // TODO: sketchy math
		}

		private int GermsToKill(int count, float strength)
		{
			if (count == 1)
				return 1;

			return (int)Mathf.Clamp(count * strength, 0, count);
		}

		private void UpdateSaltWater(int cell, Element element, ushort elementIdx)
		{
			if (elementIdx == saltWaterIdx || elementIdx == brineIdx)
			{
				var liquidMass = Grid.Mass[cell];

				if (liquidMass < 0.3f)
					return;

				var cellAbove = Grid.CellAbove(cell);

				if (!Grid.IsValidCell(cellAbove))
					return;

				if (Grid.Element[cellAbove].idx == oxygenIdx)
				{
					saltyOxygen ??= ElementLoader.FindElementByHash(Elements.saltyOxygen);

					Game.Instance.SpawnFX(saltFx, Grid.CellToPosCTC(cell, Grid.SceneLayer.FXFront), 0f);

					var mass = Mathf.Min(liquidMass, 0.005f);

					SimMessages.ReplaceElement(
						cellAbove,
						Elements.saltyOxygen,
						CellEventLogger.Instance.DebugTool,
						Grid.Mass[cellAbove] + mass,
						Grid.Temperature[cellAbove],
						Grid.DiseaseIdx[cellAbove],
						Grid.DiseaseCount[cellAbove]);

					SimMessages.ConsumeMass(cell, element.id, liquidMass - 0.005f, 1);
				}
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			SetChunks();
			SetElements();

			fungusInfos = new()
			{
				{
					Db.Get().Diseases.GetIndex(BDiseases.mushroomSpore.id),
					new FungusInfo(MushroomPlantConfig.SEED_ID, ModAssets.Fx.grimcapPoff)
				},
				{
					Db.Get().Diseases.GetIndex(BDiseases.poffSpore.id),
					new FungusInfo(PoffShroomConfig.SEED_ID, ModAssets.Fx.grimcapPoff)
				},
			};

			// TODO: only revealed areas
			// TODO: liquid change?
		}

		public static void SetElements()
		{
			oxygenIdx = ElementLoader.GetElementIndex(SimHashes.Oxygen);
			saltWaterIdx = ElementLoader.GetElementIndex(SimHashes.SaltWater);
			brineIdx = ElementLoader.GetElementIndex(SimHashes.Brine);
			saltyOxygen = ElementLoader.FindElementByHash(Elements.saltyOxygen);
			acidIdx = ElementLoader.GetElementIndex(Elements.sulfurousWater);
			hydrogenIdx = ElementLoader.GetElementIndex(SimHashes.Hydrogen);
			coralliumIdx = ElementLoader.GetElementIndex(Elements.corallium);
		}

		private int GetRandomCellInChunk(int chunk)
		{
			ChunkOffset(chunk, out var x, out var y);
			x += Random.Range(0, CHUNK_EDGE);
			y += Random.Range(0, CHUNK_EDGE);

			return Grid.XYToCell(x, y);
		}

		private static void SetChunks()
		{
			widthInChunks = Grid.WidthInCells / CHUNK_EDGE;
			heightInChunks = Grid.HeightInCells / CHUNK_EDGE;
			chunkCount = widthInChunks * heightInChunks;
		}

		public static int XYToChunk(int x, int y)
		{
			return x + y * widthInChunks;
		}

		public static void ChunkToXY(int chunk, out int x, out int y)
		{
			x = chunk % widthInChunks;
			y = chunk / widthInChunks;
		}

		public static void ChunkOffset(int chunk, out int x, out int y)
		{
			x = chunk % widthInChunks * CHUNK_EDGE;
			y = chunk / widthInChunks * CHUNK_EDGE;
		}
	}
}
