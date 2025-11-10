using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	public class Beached_DigToolSkillToggle : KMonoBehaviour
	{
		public static Beached_DigToolSkillToggle Instance { get; private set; }

		public const string
			DIG = "BEACHED_DIG",
			PRECISION = "BEACHED_PRECISION",
			PRECIOUS_ONLY = "BEACHED_PRECIOUSONLY";

		private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = [];
		private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;

		public override void OnPrefabInit()
		{
			ResetFilter(filterTargets);
			Subscribe(ModHashes.onDigtoolActivated, _ => OnActivateTool());
			Subscribe(ModHashes.onDigtoolDeActivated, _ => OnDeactivateTool());
			Instance = this;
		}

		public override void OnSpawn()
		{
		}

		public override void OnCleanUp()
		{
			Unsubscribe(ModHashes.onDigtoolActivated, _ => OnActivateTool());
			Unsubscribe(ModHashes.onDigtoolDeActivated, _ => OnDeactivateTool());

			Instance = null;
		}

		public bool IsActiveLayer(string layer)
		{
			return currentFilterTargets.ContainsKey(layer.ToUpper())
				&& currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On;
		}

		protected void ResetFilter(Dictionary<string, ToolParameterMenu.ToggleState> filters)
		{
			filters.Clear();
			GetDefaultFilters(filters);
			currentFilterTargets = filters;
		}

		public void OnActivateTool() => ShowOptions();

		public void OnDeactivateTool() => HideOptions();

		protected virtual void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
		{
			filters.Add(DIG, ToolParameterMenu.ToggleState.Off);
			filters.Add(PRECISION, ToolParameterMenu.ToggleState.Off);
			filters.Add(PRECIOUS_ONLY, ToolParameterMenu.ToggleState.On);
		}

		private void ShowOptions()
		{
			Log.Debug("Beached_DigToolSkillToggle ShowOptions");
			ToolMenu.Instance.toolParameterMenu.PopulateMenu(currentFilterTargets);
		}

		private void HideOptions()
		{
			ToolMenu.Instance.toolParameterMenu.ClearMenu();
		}

		public string GetActiveFilter()
		{
			foreach (var filter in currentFilterTargets)
			{
				if (filter.Value == ToolParameterMenu.ToggleState.On)
					return filter.Key;
			}

			return "none";
		}
	}
}
