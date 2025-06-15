

using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Corallium : KMonoBehaviour, ISim4000ms
	{
		public void Sim4000ms(float dt)
		{
			if (gameObject.HasTag(GameTags.Stored))
				return;

			if (Random.value < 0.25f)
			{
				MiscUtil.TrySpreadCoralliumToTile(Grid.PosToCell(this), CellOffset.down);
			}
		}
	}
}
