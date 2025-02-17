﻿using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Klei.AI.Disease;

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

		public static float PerCycle(float amount) => amount / CONSTS.CYCLE_LENGTH;

		public static void AddSimpleButton(GameObject target, string icon, string text, string tooltip, System.Action onClick, bool isInteractable = true)
		{
			Game.Instance.userMenu.AddButton(
				target,
				new KIconButtonMenu.ButtonInfo(
					icon, text, onClick, tooltipText: tooltip, is_interactable: isInteractable));
		}

		public static RangeInfo RangeInfoCelsius(float minViable, float minGrowth, float maxGrowth, float maxViable)
		{
			return new RangeInfo(
				CelsiusToKelvin(minViable),
				CelsiusToKelvin(minGrowth),
				CelsiusToKelvin(maxGrowth),
				CelsiusToKelvin(maxViable));
		}

		public const char CENTER = 'O';
		public const char FILLED = 'X';

		public static List<CellOffset> MakeCellOffsetsFromMap(bool fillCenter, params string[] pattern)
		{
			var xCenter = 0;
			var yCenter = 0;
			var result = new List<CellOffset>();

			for (int y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (int x = 0; x < line.Length; x++)
				{
					if (line[x] == CENTER)
					{
						xCenter = x;
						yCenter = y;

						break;
					}
				}
			}

			for (int y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (int x = 0; x < line.Length; x++)
				{
					if (line[x] == FILLED
						|| (fillCenter && line[x] == CENTER))
						result.Add(new CellOffset(x - xCenter, y - yCenter));
				}
			}

			return result;
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
