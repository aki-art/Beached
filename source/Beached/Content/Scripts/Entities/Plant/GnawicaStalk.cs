using Beached.Content.Scripts.Entities.Plant;
using ImGuiNET;
using System;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class GnawicaStalk : MultiPartPlantPiece, IImguiDebug
	{
		[SerializeField] public Tag connectionTag;
		[SerializeField] public int objectLayer;

		[MyCmpReq] private KBatchedAnimController kbac;

		private int _cell;
		private Connection _connections;

		public Connection Connections => _connections;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Beached_Grid.hasClimbable[Grid.PosToCell(this)] = true;
			ModCmps.gnawicaStalks.Add(this);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			_cell = Grid.PosToCell(this);
			RefreshConnections();
			AlertAllNeighbors();
		}

		public override void OnCleanUp()
		{
			var cell = Grid.PosToCell(this);
			//if ((Object)Grid.Objects[num, (int)ObjectLayer.] == (Object)null)
			//{
			Beached_Grid.hasClimbable[cell] = false;
			//}

			if (!Game.IsQuitting())
				AlertAllNeighbors();

			ModCmps.gnawicaStalks.Remove(this);
		}

		private void AlertAllNeighbors()
		{
			AlertNeighbour(Direction.Up);
			AlertNeighbour(Direction.Down);
			AlertNeighbour(Direction.Right);
			AlertNeighbour(Direction.Left);
		}

		private void AlertNeighbour(Direction direction)
		{
			var cell = Grid.GetCellInDirection(_cell, direction);
			if (Grid.IsValidCell(cell)
				&& Grid.ObjectLayers[objectLayer].TryGetValue(cell, out var go)
				&& go.HasTag(connectionTag) &&
				go.TryGetComponent(out GnawicaStalk stalk))
				stalk.RefreshConnection(direction.Opposite());
		}

		private bool IsConnectedTo(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.ObjectLayers[objectLayer].TryGetValue(cell, out var go) && go.HasTag(connectionTag);
		}

		public void RefreshConnection(Direction direction)
		{
			var newConnections = _connections;

			switch (direction)
			{
				case Direction.Up:
					if (IsConnectedTo(Grid.CellAbove(_cell)))
						newConnections |= Connection.Top;
					else newConnections &= ~Connection.Top;
					break;
				case Direction.Right:
					if (IsConnectedTo(Grid.CellRight(_cell)))
						newConnections |= Connection.Right;
					else newConnections &= ~Connection.Right;
					break;
				case Direction.Down:
					if (IsConnectedTo(Grid.CellBelow(_cell)))
						newConnections |= Connection.Bottom;
					else newConnections &= ~Connection.Bottom;
					break;
				case Direction.Left:
					if (IsConnectedTo(Grid.CellLeft(_cell)))
						newConnections |= Connection.Left;
					else newConnections &= ~Connection.Left;
					break;
			}

			_connections = newConnections;

			RefreshAnimation();
		}

		private void RefreshAnimation()
		{
			var anim = Convert.ToString(((byte)_connections), 2).PadLeft(4, '0'); // the animations were prepared to match this pattern
			kbac.Play(anim);
		}

		[Flags]
		public enum Connection
		{
			None = 0,
			Left = 1,
			Bottom = 2,
			Right = 4,
			Top = 8,
		}

		public void RefreshConnections()
		{
			RefreshConnection(Direction.Up);
			RefreshConnection(Direction.Right);
			RefreshConnection(Direction.Down);
			RefreshConnection(Direction.Left);
		}

		public void OnImguiDraw()
		{
			ImGui.Text("Left: " + _connections.HasFlag(Connection.Left));
			ImGui.Text("Bottom: " + _connections.HasFlag(Connection.Bottom));
			ImGui.Text("Right: " + _connections.HasFlag(Connection.Right));
			ImGui.Text("Top: " + _connections.HasFlag(Connection.Top));
			ImGui.Text("Code: " + Convert.ToString(((byte)_connections), 2).PadLeft(4, '0'));
		}
	}
}
