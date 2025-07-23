using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class FlowModifier : KMonoBehaviour
	{
		[SerializeField] public List<(CellOffset, float)> overrides;

		public override void OnSpawn()
		{
			if (overrides != null)
			{
				var cell = Grid.PosToCell(this);
				foreach (var (offset, flow) in overrides)
				{
					var targetCell = Grid.OffsetCell(cell, offset);
					if (Grid.IsValidCell(targetCell))
						Beached_Grid.flowOverrides[targetCell] += flow;
				}
			}
		}

		public override void OnCleanUp()
		{
			if (overrides != null)
			{
				var cell = Grid.PosToCell(this);
				foreach (var (offset, flow) in overrides)
				{
					var targetCell = Grid.OffsetCell(cell, offset);
					if (Grid.IsValidCell(targetCell))
						Beached_Grid.flowOverrides[targetCell] = Mathf.Max(0, Beached_Grid.flowOverrides[targetCell] - flow);
				}
			}
		}
	}
}
