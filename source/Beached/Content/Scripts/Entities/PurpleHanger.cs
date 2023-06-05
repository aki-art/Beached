using Beached.Content.Defs.Flora;
using ImGuiNET;
using KSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beached.Content.Scripts.Entities;

public class PurpleHanger : KMonoBehaviour, ISim4000ms, IImguiDebug
{
	[Serialize] public bool isBottomPiece;
	[Serialize] public bool isTopPiece;
	[SerializeField] public int maxInitialLength;
	[MyCmpGet] public KBatchedAnimController kbac;

	public override void OnPrefabInit()
	{
		Subscribe((int)GameHashes.NewGameSpawn, OnGameSpawn);
	}

	public override void OnSpawn()
	{
		Subscribe(ModHashes.blockUpdate, OnUpdate);
		OnUpdate(null);
	}

	private void OnGameSpawn(object o)
	{
		OnUpdate(null);
		GrowToRandomLength();
	}

	private static bool IsSolidGround(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	private static bool IsUnoccupiedCell(int cell)
	{
		return Grid.IsValidCell(cell)
			   && (Grid.IsGas(cell) || Grid.Mass[cell] == 0)
			   && !Grid.ObjectLayers[(int)ObjectLayer.Building].ContainsKey(cell);
	}

	public void GrowToRandomLength()
	{
		if (!isBottomPiece) return;

		var originCell = Grid.PosToCell(this);

		var length = Random.Range(0, maxInitialLength);
		for (var i = 1; i < length; i++)
		{
			var cell = Grid.OffsetCell(originCell, 0, -i);
			if (!IsUnoccupiedCell(cell))
			{
				return;
			}

			MiscUtil.Spawn(PurpleHangerConfig.ID, Grid.CellToPos(cell) + new Vector3(0.5f, 0));
		}

		OnUpdate(null);
	}

	public void GrowOne()
	{
		var originCell = Grid.PosToCell(this);
		var cell = Grid.CellBelow(originCell);

		if (!IsUnoccupiedCell(cell))
		{
			return;
		}

		MiscUtil.Spawn(PurpleHangerConfig.ID, Grid.CellToPos(cell));
	}

	private void OnUpdate(object o)
	{
		var pos = Grid.PosToCell(this);
		var posBelow = Grid.CellBelow(pos);
		isBottomPiece = !(Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(posBelow, out var go) &&
						  go.PrefabID() == PurpleHangerConfig.ID);

		isTopPiece = IsSolidGround(Grid.CellAbove(pos));

		if (isBottomPiece)
		{
			kbac.Play("idle_bottom");
		}
		else if (isTopPiece)
		{
			kbac.Play("idle_top");
		}
		else
		{
			kbac.Play("idle_middle");
		}
	}

	public void Sim4000ms(float dt)
	{
		OnUpdate(null);
	}

	public void OnImguiDraw()
	{
		if (ImGui.Button("Grow Purpicle to random length"))
		{
			GrowToRandomLength();
		}
		else if (ImGui.Button(("Grow Bamboo by 1")))
		{
			GrowOne();
		}
	}
}
