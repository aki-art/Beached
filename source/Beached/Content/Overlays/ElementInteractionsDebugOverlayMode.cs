using System.Collections.Generic;
using UnityEngine;
using static Beached.Content.Scripts.Buildings.Chime;

namespace Beached.Content.Overlays
{
	public class ElementInteractionsOverlayMode : OverlayModes.Mode
	{
		// the game scrapes ID by reflection in devtools, do not change type or name
		public static readonly HashedString ID = "Beached_ElementInteractionsOverlay";

		// todo: pin overlay to overlays menu

		public static float flowMult = 10f;

		public static Color GetColorForCell(SimDebugView _, int cell)
		{
			//var chunkIdx = ElementInteractions.CellToChunkIdx(cell);
			//var color = ElementInteractions.simActiveChunks.Contains(chunkIdx) ? Color.green : Color.red;
			//Color.Lerp(Color.black, color, Mathf.Max(0.1f, chunkIdx % 4 / 4f));

			var flow = Flow(cell) * flowMult;

			var isGas = Grid.IsGas(cell);

			if (isGas && !showGases && !showAll)
				return Color.black;

			if (!isGas && !showLiquids && !showAll)
				return Color.black;

			var targetColor = isGas ? Color.magenta : Color.green;

			var x = Color.Lerp(Color.black, Color.red, Mathf.Abs(flow.x));
			var y = Color.Lerp(Color.black, Color.green, Mathf.Abs(flow.y));

			return x + y;
			//return Color.Lerp(Color.black, x + _y, flow.magnitude);
		}

		private static bool showAll;
		private static bool showGases;
		private static bool showLiquids;

		public ElementInteractionsOverlayMode()
		{
			legendFilters = CreateDefaultFilters();
		}

		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			var filters = new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{ "All", ToolParameterMenu.ToggleState.On },
				{ "Gas", ToolParameterMenu.ToggleState.On },
				{ "Liquids", ToolParameterMenu.ToggleState.On }
			};

			return filters;
		}

		public override void OnFiltersChanged()
		{
			showAll = InFilter("All", legendFilters);
			showGases = InFilter("Gas", legendFilters);
			showLiquids = InFilter("Liquids", legendFilters);
		}

		public override List<LegendEntry> GetCustomLegendData()
		{
			return [
				new LegendEntry("Flow", "", Color.gray),
				new LegendEntry("Gas Flow", "", Color.magenta),
				new LegendEntry("Liquid Flow", "", Color.green)
				];
		}

		unsafe static public Vector2f Flow(int cell)
		{
			var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
			var flowTexVec = vecPtr[cell];
			return new Vector2f(flowTexVec.X, flowTexVec.Y);
		}

		public override string GetSoundName() => "Decor";

		public override HashedString ViewMode() => ID;
	}
}
