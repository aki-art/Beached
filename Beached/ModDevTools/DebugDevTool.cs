using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using ImGuiNET;
using System.Linq;
using UnityEngine;

namespace Beached.ModDevTools
{
    public class DebugDevTool : DevTool
    {
        private string testString = "test";
        private static Vector4 zoneTypeColor;
        private static Vector4 previousZoneTypeColor;
        private static int selectedZoneType;

        private string[] zoneTypes;

        public DebugDevTool()
        {
            RequiresGameRunning = true;
        }

        public override void RenderTo(DevPanel panel)
        {
            if(zoneTypes == null || zoneTypes.Length == 0)
            {
                zoneTypes = ZoneTypes.values.Select(z => z.ToString()).ToArray();
            }

            ImGui.Text("Notifications");

            if (ImGui.Button("Open test notification"))
            {
                Tutorials.Instance.Test();
            }

            if (ImGui.Button("Debug Data trigger"))
            {
                BeachedMod.Instance.Trigger(ModHashes.DebugDataChange);
            }

            ImGui.InputText("test input", ref testString, 64);

            ImGui.Spacing();
            ImGui.Text("Zone Colors");

            ImGui.SetColorEditOptions(ImGuiColorEditFlags.DisplayHex);
            ImGui.ColorPicker4("Color", ref zoneTypeColor);
            ImGui.ListBox("ZoneType", ref selectedZoneType, zoneTypes, zoneTypes.Length);

            if(zoneTypeColor != previousZoneTypeColor)
            {
                if (zoneTypeColor != null)
                {
                    var color = new Color(zoneTypeColor.x, zoneTypeColor.y, zoneTypeColor.z, zoneTypeColor.w);

                    World.Instance.zoneRenderData.zoneColours[(int)ZoneTypes.values.ElementAt(selectedZoneType)] = color;
                    World.Instance.zoneRenderData.OnActiveWorldChanged();

                    previousZoneTypeColor = zoneTypeColor;
                }
            }

            if(ImGui.Button("Recolor Zone"))
            {
                if (zoneTypeColor != null)
                {
                    var color = new Color(zoneTypeColor.x, zoneTypeColor.y, zoneTypeColor.z, zoneTypeColor.w);

                    World.Instance.zoneRenderData.zoneColours[(int)ZoneTypes.values.ElementAt(selectedZoneType)] = color;
                    World.Instance.zoneRenderData.OnActiveWorldChanged();
                }
            }

        }
    }
}
