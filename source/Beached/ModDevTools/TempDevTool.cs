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
			if(lockIcon != null)
			{
				ImGui.DragFloat("X", ref lockX);
				ImGui.DragFloat("Y", ref lockY);
				ImGui.DragFloat("Z", ref lockZ);
				ImGui.DragFloat("Scale", ref scale);

				lockIcon.transform.localPosition = new Vector3(lockX, lockY, lockZ);
				lockIcon.transform.localScale = Vector3.one * scale;
			}
		}
	}
}
#endif
