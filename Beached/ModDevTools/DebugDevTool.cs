using Beached.Content;
using Beached.Content.Scripts;
using ImGuiNET;

namespace Beached.ModDevTools
{
    public class DebugDevTool : DevTool
    {
        private string testString = "test";

        public DebugDevTool()
        {
        }

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.Text("Notifications");

            if (ImGui.Button("Open test notification"))
            {
                BeachedTutorials.Instance.Test();
            }

            if (ImGui.Button("Debug Data trigger"))
            {
                BeachedWorldManager.Instance.Trigger(ModHashes.DebugDataChange);
            }

            ImGui.InputText("test input", ref testString, 64);
        }
    }
}
