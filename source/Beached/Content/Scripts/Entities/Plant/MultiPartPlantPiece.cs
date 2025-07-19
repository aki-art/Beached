using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	public class MultiPartPlantPiece : KMonoBehaviour
	{
		[Serialize][SerializeField] public CellOffset connectionPoint;
		[Serialize] public int towardsRoot;
		[Serialize] public bool locked;

		[SerializeField] public int width;
		[SerializeField] public int height;

		public MultiPartPlantPiece()
		{
			width = 1;
			height = 1;
		}
	}
}
