using Beached.Content;
using Beached.Content.Scripts;
using ImGuiNET;

namespace Beached.ModDevTools
{
    public class WorldGenDevTool : DevTool
    {
        private string testString = "test";

        public WorldGenDevTool()
        {
        }

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.Text("Settings here");

            ImGui.Text("text inside");

            if (ImGui.Button("Open test notification"))
            {
                BeachedTutorials.Instance.Test();
            }

            ImGui.InputFloat("Shelf A X", ref MiniFridgeShelfDisplay.offsets[0].x);
            ImGui.InputFloat("Shelf A Y", ref MiniFridgeShelfDisplay.offsets[0].y);
            ImGui.InputFloat("Shelf A Z", ref MiniFridgeShelfDisplay.offsets[0].z);
            ImGui.Spacing();
            ImGui.InputFloat("Shelf B X", ref MiniFridgeShelfDisplay.offsets[1].x);
            ImGui.InputFloat("Shelf B Y", ref MiniFridgeShelfDisplay.offsets[1].y);
            ImGui.Spacing();
            ImGui.InputFloat("Shelf C X", ref MiniFridgeShelfDisplay.offsets[2].x);
            ImGui.InputFloat("Shelf C Y", ref MiniFridgeShelfDisplay.offsets[2].y);
            ImGui.Spacing();
            ImGui.InputFloat("Shelf D X", ref MiniFridgeShelfDisplay.offsets[3].x);
            ImGui.InputFloat("Shelf D Y", ref MiniFridgeShelfDisplay.offsets[3].y);
            ImGui.Spacing();

            if (ImGui.Button("Debug Data trigger"))
            {
                BeachedWorldManager.Instance.Trigger(ModHashes.DebugDataChange);
            }

            ImGui.InputText("test input", ref testString, 64);
        }
    }
}
