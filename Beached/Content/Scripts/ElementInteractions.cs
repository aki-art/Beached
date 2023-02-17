using Beached.Content.Defs.Comets;
using Beached.Content.ModDb.Germs;
using Beached.Content.Scripts.Entities;
using UnityEngine;

namespace Beached.Content.Scripts
{
    /* "Fake" sim extension. Rotates loaded chunks and randomly updates a few cells Minecraft style. 
     * The game does these things in a separate C++ library, which cannot be modded, as is far superior in calculation speed.
     * Lacking that, this is a compromise between custom element behavior and not tanking the player's FPS doing iz from C#. */

    /* Loading Custom C++ extensions: ran into problems with Linux and Mac systems. 
     * Linux is not as lenient with path searching as Windows is, and can't find the extra dll if it's not in the same folder as the main dll. 
     * Problem with that is that the game tries to load every DLL as a mod, and crashes if a random c++ dll is in there. */
    [SkipSaveFileSerialization]
    public class ElementInteractions : KMonoBehaviour, ISim200ms
    {
        private const int CHUNK_EDGE = 16;
        private const int UPDATE_ATTEMPTS = 4;
        private const float FUNGAL_LIGHT_KILL_RATE = 0.5f;
        private const float SHRAPNEL_SPEED = 10f;
        private const float ACID_LOSS = 0.25f;

        private static int widthInChunks;
        private static int heightInChunks;
        private static int chunkCount;

        private static ushort oxygenIdx;
        private static ushort saltWaterIdx;
        private static ushort brineIdx;
        private static ushort acidIdx;
        private static ushort hydrogenIdx;
        private static byte grimSporeIdx;
        private static Element saltyOxygen;

        public CellElementEvent SaltOffing;

        private PlantableSeed mushroomPlantable;

        private readonly SpawnFXHashes saltFx = ModAssets.Fx.saltOff;

        public override void OnPrefabInit()
        {
            mushroomPlantable = Assets.GetPrefab(MushroomPlantConfig.SEED_ID).GetComponent<PlantableSeed>();
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

            if (Grid.IsLiquid(cell))
            {
                UpdateSaltWater(cell, element, elementIdx);
                if (elementIdx == acidIdx)
                {
                    UpdateAcid(cell);
                }
            }
            else if (Grid.IsGas(cell))
            {
                UpdateSpores(cell, diseaseCount);
            }
        }

        private void UpdateAcid(int cell)
        {
            var cellBelow = Grid.CellBelow(cell);
            var element = Grid.Element[cellBelow];

            if (element.HasTag(GameTags.Metal))
            {
                var acidMass = Grid.Mass[cell];
                var shrapnelCount = Random.Range(2, 4);
                var mass = Grid.Mass[cellBelow] / shrapnelCount;
                var position = Grid.CellToPos(cell);
                var hydrogenAmount = acidMass * ACID_LOSS * 0.1f;

                SimMessages.ReplaceElement(cellBelow, SimHashes.Hydrogen, CellEventLogger.Instance.DebugTool, hydrogenAmount, MiscUtil.CelsiusToKelvin(500), byte.MaxValue, 0);

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

            else if (element.HasTag(BTags.corrodable))
            {
                WorldDamage.Instance.ApplyDamage(cellBelow, Elements.corrosionData[element.id], cell);
                Game.Instance.SpawnFX(SpawnFXHashes.BleachStoneEmissionBubbles, cellBelow, 0);
            }
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
                        saltyOxygen = ElementLoader.FindElementByHash(Elements.saltyOxygen);
                    }

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
            grimSporeIdx = Db.Get().Diseases.GetIndex(BDiseases.mushroomSpore.id);

            // TODO: only revealed areas
            // TODO: liauid change?
        }

        public static void SetElements()
        {
            oxygenIdx = ElementLoader.GetElementIndex(SimHashes.Oxygen);
            saltWaterIdx = ElementLoader.GetElementIndex(SimHashes.SaltWater);
            brineIdx = ElementLoader.GetElementIndex(SimHashes.Brine);
            saltyOxygen = ElementLoader.FindElementByHash(Elements.saltyOxygen);
            acidIdx = ElementLoader.GetElementIndex(Elements.sulfurousWater);
            hydrogenIdx = ElementLoader.GetElementIndex(SimHashes.Hydrogen);
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
