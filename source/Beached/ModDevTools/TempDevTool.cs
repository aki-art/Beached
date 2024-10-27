#if DEVTOOLS
using ImGuiNET;
using UnityEngine;

namespace Beached.ModDevTools
{
	// random nonsense for quick tests
	public class TempDevTool : DevTool
	{
		public static Transform lockIcon;
		private float lockX = 179.0f;
		private float lockY = 0;
		private float lockZ = 0;
		private float scale = 1.35f;

		public override void RenderTo(DevPanel panel)
		{
			if(SelectTool.Instance.selectedCell != -1)
			{
				ProcGen.SubWorld.ZoneType zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(SelectTool.Instance.selectedCell);
				
				ImGui.Text("ZoneType: " + zoneType);
			}
		}
	}
}
#endif
