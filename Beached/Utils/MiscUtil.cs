using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Beached.Utils
{
    public class MiscUtil
    {
        public static Direction GetOpposite(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => Direction.None,
            };
        }
        
        public static void AddToStaticReadonlyArray<ElemType, InstanceType>(string fieldName, params ElemType[] items)
        {
            var ref_ALL_ATTRIBUTES = AccessTools.FieldRefAccess<ElemType[]>(typeof(InstanceType), fieldName);

            var existingValues = new List<ElemType>(ref_ALL_ATTRIBUTES());
            existingValues.AddRange(items);

            ref_ALL_ATTRIBUTES() = existingValues.ToArray();
        }

        public static void AddToReadonlyArray<ElemType, InstanceType>(InstanceType instance, string fieldName, params ElemType[] items)
        {
            var ref_ALL_ATTRIBUTES = AccessTools.FieldRefAccess<ElemType[]>(typeof(InstanceType), fieldName);

            var existingValues = new List<ElemType>(ref_ALL_ATTRIBUTES(instance));
            existingValues.AddRange(items);

            ref_ALL_ATTRIBUTES(instance) = existingValues.ToArray();
        }

        public static void Explode(int cell, int radius)
        {
            var pos = Grid.CellToPos(cell);
            // just damages entities
            GameUtil.CreateExplosion(pos);


        }

        public static float CelsiusToKelvin(float celsius)
        {
            return GameUtil.GetTemperatureConvertedToKelvin(celsius, GameUtil.TemperatureUnit.Celsius);
        }

        public static bool IsNaturalCell(int cell)
        {
            return Grid.IsValidCell(cell) && Grid.Solid[cell] && Grid.Objects[cell, (int)ObjectLayer.FoundationTile] == null;
        }

        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            var prefab = global::Assets.GetPrefab(tag);

            if (prefab == null)
            {
                return null;
            }

            var go = GameUtil.KInstantiate(global::Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);

            return go;
        }

        public static GameObject Spawn(Tag tag, GameObject atGO, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            return Spawn(tag, atGO.transform.position, sceneLayer, setActive);
        }

        public static T GetWeightedRandom<T>(IEnumerable<T> enumerator, SeededRandom rand = null) where T : IWeighted
        {
            if (enumerator == null || enumerator.Count() == 0)
            {
                return default;
            }

            var totalWeight = enumerator.Sum(n => n.weight);
            var treshold = rand == null ? UnityEngine.Random.value : rand.RandomValue();
            treshold *= totalWeight;

            var num3 = 0.0f;

            foreach (var item in enumerator)
            {
                num3 += item.weight;
                if (num3 > treshold)
                {
                    return item;
                }
            }

            return enumerator.GetEnumerator().Current;
        }
    }
}
