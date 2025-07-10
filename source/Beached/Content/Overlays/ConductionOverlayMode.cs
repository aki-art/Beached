using UnityEngine;

namespace Beached.Content.Overlays
{
	public class ConductionOverlayMode : OverlayModes.Mode
	{
		// the game scrapes ID by reflection in devtools, do not change type or name
		public static readonly HashedString ID = "Beached_ConductionOverlay";

		public static Color GetColorForCell(SimDebugView _, int cell)
		{
			var t = Beached_Grid.GetElectricConduction(cell);
			return Color.Lerp(Color.black, ModAssets.Colors.electricBlue, t);
		}

		public override string GetSoundName() => "Decor";

		public override HashedString ViewMode() => ID;
	}
}
