using Beached.Content.Defs.Flora;
using ImGuiNET;
using KSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beached.Content.Scripts.Entities;

public class Bamboo : KMonoBehaviour, IImguiDebug
{
	[Serialize] public bool isBottomPiece;
	[Serialize] public bool isTopPiece;
	// [Serialize] public Ref<Bamboo> bottomPiece;
	[SerializeField] public int maxInitialLength;


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
			var cell = Grid.OffsetCell(originCell, 0, i);
			if (!IsUnoccupiedCell(cell))
			{
				return;
			}

			MiscUtil.Spawn(BambooConfig.ID, Grid.CellToPos(cell) + new Vector3(0.5f, 0));
		}
	}

	public void GrowOne()
	{
		var originCell = Grid.PosToCell(this);
		var cell = Grid.CellAbove(originCell);

		if (!IsUnoccupiedCell(cell))
		{
			return;
		}

		MiscUtil.Spawn(BambooConfig.ID, Grid.CellToPos(cell));
	}

	private void OnUpdate(object o)
	{
		isBottomPiece = IsSolidGround(Grid.CellBelow(Grid.PosToCell(this)));
	}

	public void OnImguiDraw()
	{
		if (ImGui.Button("Grow Bamboo to random length"))
		{
			GrowToRandomLength();
		}
		else if (ImGui.Button(("Grow Bamboo by 1")))
		{
			GrowOne();
		}
	}
}