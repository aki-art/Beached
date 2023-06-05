using UnityEngine;
using UnityEngine.UI;

namespace Beached.Content.Scripts.Entities
{
	public class CrystalDebug : KMonoBehaviour
	{
		private LineRenderer originLineRenderer;
		private LineRenderer foundationRenderer;
		private LineRenderer lineOfSightCheckRenderer;
		private Text foundationText;

		[MyCmpReq]
		private Crystal crystal;

		public override void OnSpawn()
		{
			originLineRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.red, Color.red);
			foundationRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.green, Color.green);
			lineOfSightCheckRenderer = ModDebug.AddSimpleLineRenderer(transform, Color.magenta, Color.blue);

			ModDebug.Square(originLineRenderer, transform.position, 0.03f);

			UpdateVisualizers();

			GetComponent<KBatchedAnimController>().TintColour = new Color(1, 1, 1, 0.5f);
		}

		public void UpdateVisualizers()
		{
			var foundationDir = MiscUtil.GetOpposite(crystal.growthDirection);
			var foundationPos = Grid.CellToPosCCC(Grid.GetCellInDirection(Grid.PosToCell(this), foundationDir), Grid.SceneLayer.Building);

			if (foundationText == null)
			{
				foundationText = ModDebug.AddText(foundationPos, Color.green, crystal.growthDirection.ToString());
			}
			else
			{
				foundationText.text = crystal.growthDirection.ToString();
				foundationText.transform.position = foundationPos;
			}

			ModDebug.Square(foundationRenderer, foundationPos, 0.5f);
		}

		public void UpdateDebugLines(int x, int y, int testX, int testY)
		{
			lineOfSightCheckRenderer.positionCount = 1;
			var z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX);

			lineOfSightCheckRenderer.SetPositions(new[] {
				new Vector3(x, y, z),
				new Vector3(testX, testY, z)
			});
		}
	}
}
