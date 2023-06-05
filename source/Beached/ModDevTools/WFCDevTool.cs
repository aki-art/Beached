using ImGuiNET;
using UnityEngine;

namespace Beached.ModDevTools
{
	public class WFCDevTool : DevTool
	{
		public AreaMarker sampleAreaMarker;
		public AreaMarker runAreaMarker;

		public WFCDevTool() => RequiresGameRunning = true;

		public override void RenderTo(DevPanel panel)
		{
			if (sampleAreaMarker == null || runAreaMarker == null)
			{
				InitMarkers();
			}

			sampleAreaMarker.DrawAreaMarkerControls();
			//runAreaMarker.DrawAreaMarkerControls();

			var tex = new Texture2D(sampleAreaMarker.width, sampleAreaMarker.height, TextureFormat.ARGB32, false);

			if (ImGui.Button("Save Area"))
			{
				var data = new ushort[sampleAreaMarker.width * sampleAreaMarker.height];
				for (int x = 0; x < sampleAreaMarker.width; x++)
				{
					for (int y = 0; y < sampleAreaMarker.height; y++)
					{
						var sampleCell = Grid.PosToCell(new Vector3(x, y) + sampleAreaMarker.bottomLeft);
						var col = new Color32((byte)Grid.ElementIdx[sampleCell], 0, 0, 255);
						tex.SetPixel(x, y, col);
						/*
												var idx = (int)(y + 0.05f) * sampleAreaMarker.width + x;

												data[idx] = Grid.ElementIdx[sampleCell];*/
					}
				}

				tex.Apply();
				ModAssets.SaveImage(tex, "wfc_sample");
			}

			if (ImGui.Button("Observe"))
			{

			}

			if (ImGui.Button("Run"))
			{

			}

			if (isRequestingToClosePanel)
			{
				sampleAreaMarker.SetActive(false);
				runAreaMarker.SetActive(false);
			}
		}
		private void InitMarkers()
		{
			var width = ClusterManager.Instance.activeWorld.Width;
			var height = ClusterManager.Instance.activeWorld.Height;
			sampleAreaMarker = new AreaMarker(48, 48, Vector3.zero, Color.red);
			runAreaMarker = new AreaMarker(width / 2, height, new Vector3(width / 2, 0), Color.cyan);
		}
	}
}
