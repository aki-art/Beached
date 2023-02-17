using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using ImGuiNET;
using System.Linq;
using Beached.Content.Scripts.Entities;
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
            if (zoneTypes == null || zoneTypes.Length == 0)
            {
                zoneTypes = ZoneTypes.values.Select(z => z.ToString()).ToArray();
            }

            if (SelectTool.Instance != null)
            {
                var selectedObject = SelectTool.Instance.selected;

                if (selectedObject != null)
                {
                    if (selectedObject.TryGetComponent(out Bamboo bamboo))
                    {
                        if (ImGui.Button("Grow Bamboo to random length"))
                        {
                            bamboo.GrowToRandomLength();
                        }
                        else if (ImGui.Button(("Grow Bamboo by 1")))
                        {
                            bamboo.GrowOne();
                        }
                    }
                    else if (selectedObject.TryGetComponent(out PurpleHanger hanger))
                    {
                        if (ImGui.Button("Grow Purpicle to random length"))
                        {
                            hanger.GrowToRandomLength();
                        }
                        else if (ImGui.Button(("Grow Bamboo by 1")))
                        {
                            hanger.GrowOne();
                        }
                    }

                    ImGui.Spacing();
                }
            }

            ImGui.Text("Notifications");

            if (ImGui.Button("Open test notification"))
            {
                Tutorials.Instance.Test();
            }

            if (ImGui.Button("Debug Data trigger"))
            {
                BeachedMod.Instance.Trigger(ModHashes.debugDataChange);
            }

            ImGui.InputText("test input", ref testString, 64);

            ImGui.Spacing();
            ImGui.Text("Zone Colors");

            ImGui.SetColorEditOptions(ImGuiColorEditFlags.DisplayHex);
            ImGui.ColorPicker4("Color", ref zoneTypeColor);
            ImGui.ListBox("ZoneType", ref selectedZoneType, zoneTypes, zoneTypes.Length);

            if (zoneTypeColor != previousZoneTypeColor)
            {
                if (zoneTypeColor != null)
                {
                    var color = new Color(zoneTypeColor.x, zoneTypeColor.y, zoneTypeColor.z, zoneTypeColor.w);

                    World.Instance.zoneRenderData.zoneColours[(int)ZoneTypes.values.ElementAt(selectedZoneType)] = color;
                    World.Instance.zoneRenderData.OnActiveWorldChanged();

                    previousZoneTypeColor = zoneTypeColor;
                }
            }

            if (ImGui.Button("Recolor Zone"))
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
