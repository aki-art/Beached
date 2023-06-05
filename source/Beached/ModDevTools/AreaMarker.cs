using ImGuiNET;
using UnityEngine;

namespace Beached.ModDevTools
{
	public class AreaMarker
	{
		public int width;
		public int height;
		public Vector3 bottomLeft;
		public Color color;

		public LineRenderer areaMarker;

		public AreaMarker(int width, int height, Vector3 bottomLeft, Color color)
		{
			this.width = width;
			this.height = height;
			this.bottomLeft = bottomLeft;
			areaMarker = ModDebug.AddSimpleLineRenderer(Game.Instance.transform, color, color, 0.4f);
			areaMarker.loop = true;
			areaMarker.positionCount = 4;
			UpdateMarker();

			areaMarker.gameObject.SetActive(true);
		}

		public void DrawAreaMarkerControls()
		{
			areaMarker.gameObject.SetActive(true);

			ImGui.InputInt("width", ref width);
			ImGui.InputInt("height", ref height);
			ImGui.Spacing();

			if (ImGui.Button("Set Bottom Left corner"))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (Grid.IsValidCell(cell))
				{
					bottomLeft = Grid.CellToPos(cell) with { z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX) };
					UpdateMarker();
				}
			}

			if (ImGui.Button("Set Bottom Right corner"))
			{
				var cell = SelectTool.Instance.selectedCell;
				if (Grid.IsValidCell(cell))
				{
					Grid.CellToXY(cell, out int x, out int y);
					width = x - (int)bottomLeft.x;
					height = y - (int)bottomLeft.y;

					UpdateMarker();
				}
			}

			ImGui.Spacing();
		}

		public void UpdateMarker()
		{
			areaMarker.SetPositions(new[]
			{
				bottomLeft,
				new (bottomLeft.x + width, bottomLeft.y, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX)),
				new (bottomLeft.x + width, bottomLeft.y + height, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX)),
				new (bottomLeft.x, bottomLeft.y + height, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX))
			});
		}

		public void SetActive(bool active)
		{
			areaMarker.gameObject.SetActive(active);
		}
	}
}
