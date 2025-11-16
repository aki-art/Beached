using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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


		public static int OffsetCell(int cell, CellOffset offset, int times)
		{
			return Grid.OffsetCell(cell, offset.x * times, offset.y * times);
		}

		public static int OffsetCell(int cell, Direction direction, int times)
		{
			var offset = direction.ToCellOffset();
			return Grid.OffsetCell(cell, offset.x * times, offset.y * times);
		}

		public static string DirectionToString(Direction direction)
		{
			return direction switch
			{
				Direction.Up => "↑",
				Direction.Right => "→",
				Direction.Down => "↓",
				Direction.Left => "→",
				_ => "X",
			};
		}

		public static int GetLocalIndex(CodeInstruction instruction)
		{
			if (instruction.operand is LocalBuilder local)
				return local.LocalIndex;

			var opCode = instruction.opcode;

			if (opCode == OpCodes.Ldloc_0)
				return 0;
			else if (opCode == OpCodes.Ldloc_1)
				return 1;
			else if (opCode == OpCodes.Ldloc_2)
				return 2;
			else if (opCode == OpCodes.Ldloc_3)
				return 3;

			return -1;
		}

		public static readonly List<CellOffset> cardinalOffsetsUnordered = [
			CellOffset.up,
			CellOffset.down,
			CellOffset.right,
			CellOffset.left,
			];

		public static void TrySpreadCoralliumToTile(int sourceCell, CellOffset direction)
		{
			var targetCell = Grid.OffsetCell(sourceCell, direction);

			var targetElement = Grid.Element[targetCell];

			if (!targetElement.IsSolid)
				return;

			if (targetElement.hardness > 50f)
				return;

			if (!targetElement.HasTag(GameTags.BuildableRaw))
				return;


			cardinalOffsetsUnordered.Shuffle();

			foreach (var offset in cardinalOffsetsUnordered)
			{
				var cell = Grid.OffsetCell(targetCell, offset);

				if (cell == sourceCell)
					continue;

				if (Grid.IsSubstantialLiquid(cell))
				{
					SimMessages.ReplaceElement(
						targetCell,
						Elements.corallium,
						ElementInteractions.CoralSpread,
						Grid.Mass[targetCell],
						Grid.Temperature[targetCell],
						Grid.DiseaseIdx[targetCell],
						Grid.DiseaseCount[targetCell]);
				}
			}
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

		public static List<CellOffset> CellOffsets(int x1, int y1, int x2, int y2)
		{
			var result = new List<CellOffset>();
			for (var x = x1; x <= x2; x++)
			{
				for (var y = y1; y <= y2; y++)
				{
					result.Add(new CellOffset(x, y));
				}
			}

			return result;
		}

		public static List<CellOffset> MakeCellOffsetsFromMap(bool fillCenter, params string[] pattern)
		{
			var xCenter = 0;
			var yCenter = 0;
			var result = new List<CellOffset>();

			for (var y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (var x = 0; x < line.Length; x++)
				{
					if (line[x] == CENTER)
					{
						xCenter = x;
						yCenter = y;

						break;
					}
				}
			}

			for (var y = 0; y < pattern.Length; y++)
			{
				var line = pattern[y];
				for (var x = 0; x < line.Length; x++)
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
