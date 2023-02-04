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

        public override void RenderTo(DevPanel panel)
        {
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
        }
    }
}
