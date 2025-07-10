using Beached.Content.Scripts.Entities.Plant;
using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class SmallAquariumPlot : PlantablePlot, IImguiDebug
	{
		[SerializeField] public Storage liquidStorage;

		private static readonly EffectorValues
			_noDecor = TUNING.DECOR.NONE,
			_bonusDecor = TUNING.DECOR.BONUS.TIER2;

		private static float
			_y = 0.28f,
			_r = 0.6f,
			_scale = 0.86f;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (occupyingObject != null)
			{
				ConfigureCoral(occupyingObject);
			}

			Subscribe((int)GameHashes.OccupantChanged, OnOccupantChanged);
			OnOccupantChanged(null);
		}

		private void OnOccupantChanged(object _)
		{
			GetComponent<DecorProvider>().SetValues(occupyingObject == null ? _noDecor : _bonusDecor);
		}

		public override void PositionOccupyingObject()
		{
			base.PositionOccupyingObject();
			ConfigureCoral(occupyingObject);
		}

		private void ConfigureCoral(GameObject newPlant)
		{
			if (newPlant == null)
				return;

			if (newPlant.TryGetComponent(out KBatchedAnimController kbac))
			{
				kbac.animScale = CONSTS.ANIM_SCALE * _scale;
				var pos = kbac.PositionIncludingOffset;
				kbac.GetBatchInstanceData().SetClipRadius(pos.x, pos.y + _y, _r * _r, true);
			}

			if (newPlant.TryGetComponent(out AquariumPlantedMonitor plantedMonitor))
				plantedMonitor.SetStorage(liquidStorage);
		}

		private static string targetSymbol = "liquid_middle_fg";

		public void OnImguiDraw()
		{
			if (ImGui.DragFloat("Y###SmallAquariumY", ref _y, 0.01f) || ImGui.DragFloat("r###SmallAquariumR", ref _r, 0.01f))
			{
				if (occupyingObject != null && occupyingObject.TryGetComponent(out KBatchedAnimController kbac))
				{
					var pos = kbac.PositionIncludingOffset;
					kbac.GetBatchInstanceData().SetClipRadius(pos.x, pos.y + _y, _r * _r, true);
					kbac.SetSceneLayer(Grid.SceneLayer.Building);
				}
			}
			if (ImGui.DragFloat("Scale###SmallAquariumScale", ref _scale, 0.01f) || ImGui.DragFloat("r###SmallAquariumR", ref _scale, 0.01f))
			{
				if (occupyingObject != null && occupyingObject.TryGetComponent(out KBatchedAnimController kbac))
				{
					kbac.animScale = CONSTS.ANIM_SCALE * _scale;
				}
			}
		}
	}
}
