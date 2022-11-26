using ImGuiNET;

namespace Beached.ModDevTools
{
    public class WorldGenDevTool : DevTool
    {
        private string testString = "test";

        protected override void Render()
        {
            ImGui.Text("Settings here");

            ImGui.Text("text inside");
            ImGui.InputText("test input", ref testString, 64);

        }
    }
}
