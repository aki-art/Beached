using Beached.Content;
using UnityEngine;

namespace Beached.Cmps
{
    internal class ElementInteractions : KMonoBehaviour, ISim200ms
    {
        private const int CHUNK_EDGE = 16;
        private const int SALT_ATTEMPTS = 4;

        private static int widthInChunks;
        private static int heightInChunks;
        private static int chunkCount;

        public static ElementInteractions Instance;

        private static byte oxygenIdx;
        private static byte saltWaterIdx;
        private static byte brineIdx;
        private static Element saltyOxygen;

        public CellElementEvent SaltOffing;

        private readonly SpawnFXHashes saltFx = ModAssets.Fx.saltOff;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void Sim200ms(float dt)
        {
            if(!enabled)
            {
                //return;
            }

            UpdateCells();
        }

        public void UpdateCells()
        {
            for (var c = 0; c < chunkCount; c++)
            {
                for (var i = 0; i < SALT_ATTEMPTS; i++)
                {
                    var cell = GetRandomCellInChunk(c);
                    UpdateSalt(cell);
                }
            }
        }

        private void UpdateSalt(int cell)
        {
            if (!Grid.IsValidCell(cell))
            {
                return;
            }

            var element = Grid.Element[cell];
            var elementIdx = element.idx;

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
