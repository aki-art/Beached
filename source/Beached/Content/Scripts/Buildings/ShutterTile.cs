namespace Beached.Content.Scripts.Buildings
{
	public class ShutterTile : StateMachineComponent<ShutterTile.StatesInstance>
	{
		public override void OnSpawn()
		{
			var cell = this.NaturalBuildingCell();
			Grid.FakeFloor.Add(cell);
			SimMessages.SetCellProperties(cell, (int)Sim.Cell.Properties.SolidImpermeable);
			Grid.RenderedByWorld[cell] = false;

			smi.StartSM();
		}

		public override void OnCleanUp()
		{
			var placementCell = this.NaturalBuildingCell();
			SimMessages.ClearCellProperties(placementCell, (int)Sim.Cell.Properties.SolidImpermeable | (int)Sim.Cell.Properties.LiquidImpermeable);
			Grid.RenderedByWorld[placementCell] = Grid.Element[placementCell].substance.renderedByWorld;
			Grid.FakeFloor.Remove(placementCell);

			if (Grid.Element[placementCell].IsSolid)
				SimMessages.ReplaceAndDisplaceElement(placementCell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0.0f);

			Game.Instance.SetDupePassableSolid(placementCell, false, Grid.Solid[placementCell]);
			Grid.CritterImpassable[placementCell] = false;
			Grid.DupeImpassable[placementCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(placementCell);
		}

		private static int meshLike = (int)Sim.Cell.Properties.SolidImpermeable;
		private static int solidLike = (int)Sim.Cell.Properties.SolidImpermeable;

		private void SetSimState(bool isOpen)
		{
			var component = this.GetComponent<PrimaryElement>();
			var mass = component.Mass;

			var cell = this.NaturalBuildingCell();
			World.Instance.groundRenderer.MarkDirty(cell);

			if (isOpen)
			{
				var handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorOpened));

				SimMessages.Dig(cell, handle.index, true);
				SimMessages.ClearCellProperties(cell, (int)Sim.Cell.Properties.SolidImpermeable);
			}
			else
			{
				var handle1 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorClosed));
				var temperature = component.Temperature;
				if ((double)temperature <= 0.0)
					temperature = component.Temperature;

				SimMessages.ReplaceAndDisplaceElement(cell, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, callbackIdx: handle1.index);
				SimMessages.SetCellProperties(cell, (int)Sim.Cell.Properties.SolidImpermeable);
			}
		}

		private void OnSimDoorOpened()
		{
		}

		private void OnSimDoorClosed()
		{
		}

		protected void Open()
		{
			SetSimState(true);
		}

		protected void Close()
		{
			SetSimState(false);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, ShutterTile, object>.GameInstance
		{
			public Operational operational;

			public StatesInstance(ShutterTile master) : base(master)
			{
				master.TryGetComponent(out operational);
			}
		}


		public class States : GameStateMachine<States, StatesInstance, ShutterTile>
		{
			public State off;
			public State on;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				on
					.Enter(smi => smi.master.Open())
					.PlayAnim("on")
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational);


				off
					.Enter(smi => smi.master.Close())
					.PlayAnim("closed")
					.EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational);

			}
		}
	}
}
