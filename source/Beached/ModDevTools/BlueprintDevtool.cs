#if DEVTOOLS
using Beached.Content.ModDb;
using ImGuiNET;

namespace Beached.ModDevTools
{
	public class BlueprintDevtool : DevTool
	{
		public override void RenderTo(DevPanel panel)
		{
			var techs = Db.Get().Techs;
			var forceFieldTech = techs.Get(BTechs.HIDDEN_ATMOSPHERIC_FORCEFIELD_GENERATOR);

			ImGui.Text("Forcefield researched: " + forceFieldTech.IsComplete());

			if (ImGui.Button("Unlock Forcefield"))
			{
				Research.Instance.GetOrAdd(forceFieldTech).Purchased();
				Game.Instance.Trigger((int)GameHashes.ResearchComplete, forceFieldTech);
			}
		}
	}
}
#endif