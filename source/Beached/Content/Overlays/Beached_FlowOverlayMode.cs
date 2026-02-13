using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Overlays
{
	public class Beached_FlowOverlayMode : OverlayModes.Mode
	{
		[Tooltip("the game scrapes ID by reflection in devtools, do not change type or name")]
		public static readonly HashedString ID = nameof(Beached_FlowOverlayMode).Replace("Mode", "");

		// todo: pin overlay to overlays menu

		public static float flowMult = 10f;

		public static Color GetColorForCell(SimDebugView _, int cell)
		{
			//var chunkIdx = ElementInteractions.CellToChunkIdx(cell);
			//var color = ElementInteractions.simActiveChunks.Contains(chunkIdx) ? Color.green : Color.red;
			//Color.Lerp(Color.black, color, Mathf.Max(0.1f, chunkIdx % 4 / 4f));

			var flow = Beached_Grid.GetFlow(cell) * flowMult;

			var isGas = Grid.IsGas(cell);

			if (isGas && !showGases && !showAll)
				return Color.black;

			if (!isGas && !showLiquids && !showAll)
				return Color.black;

			var targetColor = isGas ? ModAssets.Colors.liquidOverlayBlue : ModAssets.Colors.gasOverlayPurple;

			var x = Color.Lerp(Color.black, targetColor, Mathf.Abs(flow));

			return x;
			//return Color.Lerp(Color.black, x + _y, flow.magnitude);
		}

		private static bool showAll;
		private static bool showGases;
		private static bool showLiquids;

		public Beached_FlowOverlayMode()
		{
			legendFilters = CreateDefaultFilters();
		}

		public const string
			ALL = "BEACHED_ALL",
			GAS = "BEACHED_GAS",
			LIQUID = "BEACHED_LIQUID";

		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			var filters = new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{ ALL, ToolParameterMenu.ToggleState.On },
				{ GAS, ToolParameterMenu.ToggleState.On },
				{ LIQUID, ToolParameterMenu.ToggleState.On }
			};

			return filters;
		}

		public override void OnFiltersChanged()
		{
			showAll = InFilter(ALL, legendFilters);
			showGases = InFilter(GAS, legendFilters);
			showLiquids = InFilter(LIQUID, legendFilters);
		}

		public override List<LegendEntry> GetCustomLegendData()
		{
			return [
				new LegendEntry("Gas Flow", "", ModAssets.Colors.gasOverlayPurple),
				new LegendEntry("Liquid Flow", "",  ModAssets.Colors.liquidOverlayBlue)
				];
		}

		public override string GetSoundName() => "Decor";

		public override HashedString ViewMode() => ID;
	}
}
