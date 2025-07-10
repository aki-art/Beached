using Beached.Content.Scripts;

namespace Beached.Content
{
	public class ClimbableValidator : NavTableValidator
	{
		public ClimbableValidator()
		{
			ModCmps.gnawicaStalks.Register(OnClimbableChanged, OnClimbableChanged);
		}

		private void OnClimbableChanged(GnawicaStalk stalk) => onDirty?.Invoke(Grid.PosToCell(stalk));

		public override void UpdateCell(int cell, NavTable navTable, CellOffset[] boundingOffsets)
		{
			navTable.SetValid(
				cell,
				NavType.Ladder,
				IsClear(cell, boundingOffsets, false) && Beached_Grid.hasClimbable[cell]);
		}

		public override void Clear() => ModCmps.gnawicaStalks.Unregister(OnClimbableChanged, OnClimbableChanged);
	}
}
