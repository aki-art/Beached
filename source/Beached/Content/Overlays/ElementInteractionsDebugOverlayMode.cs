using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Content.Overlays
{
	public class ElementInteractionsOverlayMode : OverlayModes.Mode
	{
		public const string ID = "Beached_ElementInteractionsOverlay";

		public static Color GetColorForCell(SimDebugView _, int cell)
		{
			var chunkIdx = ElementInteractions.CellToChunkIdx(cell);

			var color = ElementInteractions.simActiveChunks.Contains(chunkIdx) ? Color.green : Color.red;

			return Color.Lerp(Color.black, color, Mathf.Max(0.1f, chunkIdx % 4 / 4f));
		}

		public override string GetSoundName() => "Decor";

		public override HashedString ViewMode() => ID;
	}
}
