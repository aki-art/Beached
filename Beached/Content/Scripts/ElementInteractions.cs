using Beached.Content.ModDb.Germs;
using Beached.Utils;
using UnityEngine;

namespace Beached.Content.Scripts
{
    /* "Fake" sim extension. Rotates loaded chunks and randomly updates a few cells Minecraft style. 
     * The game does these things in a separate C++ library, which cannot be modded, as is far superior in calculation speed.
     * Lacking that, this is a compromise between custom element behavior and not tanking the player's FPS doing iz from C#. */

    /* Loading Custom C++ extensions: ran into problems with Linux and Mac systems. 
     * Linux is not as lenient with path searching as Windows is, and can't find the extra dll if it's not in the same folder as the main dll. 
     * Problem with that is that the game tries to load every DLL as a mod, and crashes if a random c++ dll is in there. */
    public class ElementInteractions : KMonoBehaviour, ISim200ms
    {
        private const int CHUNK_EDGE = 16;
        private const int UPDATE_ATTEMPTS = 4;
        private const float FUNGAL_LIGHT_KILL_RATE = 0.5f;

        private static int widthInChunks;
        private static int heightInChunks;
        private static int chunkCount;

        public static ElementInteractions Instance;

        private static byte oxygenIdx;
        private static byte saltWaterIdx;
        private static byte brineIdx;
        private static byte grimSporeIdx;
        private static Element saltyOxygen;

        public CellElementEvent SaltOffing;

        private PlantableSeed mushroomPlantable;

        private readonly SpawnFXHashes saltFx = ModAssets.Fx.saltOff;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            mushroomPlantable = Assets.GetPrefab(MushroomPlantConfig.SEED_ID).GetComponent<PlantableSeed>();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void Sim200ms(float dt)
        {
            if (!enabled)
            {
                return;
            }

            UpdateCells();
        }

        public void UpdateCells()
        {
            for (var c = 0; c < chunkCount; c++)
            {
                for (var i = 0; i < UPDATE_ATTEMPTS; i++)
                {
                    var cell = GetRandomCellInChunk(c);
                    UpdateCell(cell);
                }
            }
        }

        private void UpdateCell(int cell)
        {
            if (!Grid.IsValidCell(cell))
            {
                return;
            }

            var element = Grid.Element[cell];
            var elementIdx = element.idx;
            var diseaseCount = Grid.DiseaseCount[cell];

            UpdateSaltWater(cell, element, elementIdx);
            UpdateSpores(cell, diseaseCount);
        }

        private void UpdateSpores(int cell, int diseaseCount)
        {
            if (diseaseCount > 0)
            {
                var germIdx = Grid.DiseaseIdx[cell];

                if (germIdx == grimSporeIdx)
                {
                    if (Grid.LightCount[cell] > 0)
                    {
                        SimMessages.ModifyDiseaseOnCell(cell, germIdx, -GermsToKill(diseaseCount, FUNGAL_LIGHT_KILL_RATE));
                    }
                    else
                    {
                        var roll = Random.value < 0.1f;
                        if (roll)
                        {
                            if (MiscUtil.IsNaturalCell(Grid.CellBelow(cell)) &&
                                !Grid.IsSolidCell(Grid.CellAbove(cell)) &&
                                mushroomPlantable.TestSuitableGround(cell))
                            {
                                var seed = MiscUtil.Spawn(MushroomPlantConfig.SEED_ID, Grid.CellToPos(cell));
                                seed.GetComponent<PlantableSeed>().TryPlant();

                                Game.Instance.SpawnFX(ModAssets.Fx.grimcapPoff, cell, 0);
                            }
                        }
                    }
                }
            }
        }

        public static float GetFungalGermAverageChangeRateInLight()
        {
            return 5f * (UPDATE_ATTEMPTS / (CHUNK_EDGE * CHUNK_EDGE) * FUNGAL_LIGHT_KILL_RATE);
        }

        private int GermsToKill(int count, float strength)
        {
            if (count == 1)
            {
                return 1;
            }

            return (int)Mathf.Clamp(count * strength, 0, count);
        }

        private void UpdateSaltWater(int cell, Element element, ushort elementIdx)
        {
            if (elementIdx == saltWaterIdx || elementIdx == brineIdx)
            {
                var liquidMass = Grid.Mass[cell];

                if (liquidMass < 0.3f)
                {
                    return;
                }

                var cellAbove = Grid.CellAbove(cell);

                if (!Grid.IsValidCell(cellAbove))
                {
                    return;
                }

                if (Grid.Element[cellAbove].idx == oxygenIdx)
                {
                    if (saltyOxygen == null)
                    {
                        saltyOxygen = ElementLoader.FindElementByHash(Elements.SaltyOxygen);
                    }

                    Game.Instance.SpawnFX(saltFx, Grid.CellToPosCTC(cell, Grid.SceneLayer.FXFront), 0f);

                    var mass = Mathf.Min(liquidMass, 0.005f);

                    SimMessages.ReplaceElement(
                        cellAbove,
                        Elements.SaltyOxygen,
                        CellEventLogger.Instance.DebugTool,
                        Grid.Mass[cellAbove] + mass,
                        Grid.Temperature[cellAbove],
                        Grid.DiseaseIdx[cellAbove],
                        Grid.DiseaseCount[cellAbove]);

                    SimMessages.ConsumeMass(cell, element.id, liquidMass - 0.005f, 1);
                }
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            SetChunks();
            SetElements();
            grimSporeIdx = Db.Get().Diseases.GetIndex(BDiseases.mushroomSpore.id);

            // TODO: only revealed areas
            // TODO: liauid change?
        }

        public static void SetElements()
        {
            oxygenIdx = (byte)ElementLoader.GetElementIndex(SimHashes.Oxygen);
            saltWaterIdx = (byte)ElementLoader.GetElementIndex(SimHashes.SaltWater);
            brineIdx = (byte)ElementLoader.GetElementIndex(SimHashes.Brine);
            saltyOxygen = ElementLoader.FindElementByHash(Elements.SaltyOxygen);
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
