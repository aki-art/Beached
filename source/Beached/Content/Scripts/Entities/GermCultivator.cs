using Beached.Content.ModDb;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class GermCultivator : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq] private KSelectable kSelectable;

		[SerializeField] public string germ;
		[SerializeField] public float rate;
		[SerializeField] public float maxDiseaseCount;

		public HashSet<int> targetCells = [];

		private byte germIdx;
		private int baseCell;
		private bool hasBeenCultivating;
		private Guid statusItem;

		private float EaseOutSine(float x) => Mathf.Sin((x * Mathf.PI) / 2f);

		public override void OnSpawn()
		{
			germIdx = Db.Get().Diseases.GetIndex(germ);
			baseCell = Grid.PosToCell(this);

			statusItem = kSelectable.AddStatusItem(BStatusItems.cultivatingGerms, this);
		}

		public void SetCells(HashSet<int> cells) => targetCells = cells;

		public void RemoveCell(int cell) => targetCells.Remove(cell);

		public void AddCell(int cell) => targetCells.Add(cell);

		public void RemoveCell(CellOffset offset) => targetCells.Remove(Grid.OffsetCell(baseCell, offset));

		public void AddCell(CellOffset offset) => targetCells.Add(Grid.OffsetCell(baseCell, offset));

		public void Sim200ms(float dt)
		{
			var isCultivatingAnything = false;
			foreach (var cell in targetCells)
			{
				var diseaseOnMe = Grid.DiseaseIdx[cell];
				if (diseaseOnMe == germIdx)
				{
					var diseaseCount = Grid.DiseaseCount[cell];

					if (diseaseCount > maxDiseaseCount)
						continue;

					var saturation = diseaseCount / maxDiseaseCount;
					saturation = Mathf.Clamp01(saturation);

					var multiplier = (1f - EaseOutSine(saturation)) * rate * dt;
					var delta = Mathf.CeilToInt(multiplier * diseaseCount);

					SimMessages.ModifyDiseaseOnCell(cell, germIdx, delta);

					isCultivatingAnything = true;
				}
			}
			/*
						if (isCultivatingAnything != hasBeenCultivating)
							kSelectable.ToggleStatusItem(BStatusItems.cultivatingGerms, statusItem, isCultivatingAnything, this);
			*/
			hasBeenCultivating = isCultivatingAnything;
		}

		public static string ResolveStatusItemString(string str, object data)
		{
			return data is GermCultivator cultivator
				? str.Replace("{Germ}", Db.Get().Diseases.Get(cultivator.germ).Name)
				: str;
		}
	}
}
