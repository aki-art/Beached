using System;
using UnityEngine;

namespace Beached.Content.Overlays
{
	public class ConductionOverlayMode : OverlayModes.Mode
	{
		public const string ID = "Beached_ConductionOverlay";

		public static Color GetColorForCell(SimDebugView _, int cell)
		{
			var t = BeachedGrid.GetElectricConduction(cell);
			return Color.Lerp(Color.black, ModAssets.Colors.electricBlue, t);
		}

		public override string GetSoundName() => "Decor";

		public override HashedString ViewMode() => ID;
	}
}
