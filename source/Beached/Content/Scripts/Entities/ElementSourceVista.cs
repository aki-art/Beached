/*using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ElementSourceVista : StateMachineComponent<ElementSourceVista.StatesInstance>, IImguiDebug
	{
		[SerializeField] public Storage storage;
		[SerializeField] public float exchangeRatePerTile;
		[SerializeField] public SimHashes element;
		[SerializeField] public float howManyCycles;
		[SerializeField] public int depth;
		[SerializeField] public List<CellOffset> emissionShape;
		[SerializeField] public CellOffset offset;

		[Serialize] public bool isWalledUp;
		[Serialize] public bool hasInitialized;

		public float elementViscosity;
		public bool isGas;
		public bool isValid;
		public int volume;
		public ushort elementIdx;
		public float massPerCell;

		private int[] cells;

		private static readonly CellModifyMassEvent cellModifyEvent = new CellModifyMassEvent("Beached_VistaExchance", "Vista elmeent exchange");

		private static readonly List<CellOffset> defaultEmissionShape = MiscUtil.MakeCellOffsetsFromMap(true, "",
				"  X  ",
				" XXX ",
				"XXOXX",
				" XXX ",
				"  X  ");

		public ElementSourceVista()
		{
			offset = CellOffset.none;
			emissionShape = defaultEmissionShape;
		}

		public override void OnSpawn()
		{
			var element = ElementLoader.FindElementByHash(this.element);
			if (element == null)
			{
				isValid = false;
			}

			if (!hasInitialized)
			{
				Initialize(element);
			}

			smi.StartSM();

			var centerCell = Grid.PosToCell(this);
			cells = emissionShape.Select(offset => Grid.OffsetCell(centerCell, offset)).ToArray();

			if (isValid)
			{
				volume = cells.Length * depth;
				isGas = element.IsGas;
				elementIdx = ElementLoader.GetElementIndex(this.element);
				elementViscosity = element.IsGas ? element.flow * 10f : element.viscosity / 100f;
			}
		}

		private void Initialize(Element element)
		{
			var starterAmount = element.substance.SpawnResource(transform.position, howManyCycles, element.defaultValues.temperature, byte.MaxValue, 0, true);

			storage.Store(starterAmount);

			hasInitialized = true;
		}

		protected void DoExchange(float dt)
		{
			if (!isValid)
				return;

			if (storage == null) Log.Warning("storage null");
			if (cells == null) Log.Warning("occupyArea null");

			massPerCell = storage.MassStored() / volume;

			Log.Debug("masspercell" + massPerCell);

			foreach (var cell in cells)
			{
				if (!Grid.IsValidCell(cell))
					continue;

				var mass = Grid.Mass[cell];
				var elementOnPosition = Grid.Element[cell].tag;

				Log.Debug($"checking cell {cell} - {mass}");

				if (Mathf.Approximately(mass, massPerCell))
				{
					Log.Debug("presure equal");
					return;
				}

				var deltaMass = (massPerCell - mass) * exchangeRatePerTile * elementViscosity * dt;

				Log.Debug($"deltaMass: {deltaMass}");

				if (Mathf.Abs(deltaMass) < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
					continue;

				var diseaseIdx = Grid.DiseaseIdx[cell];
				var diseaseCount = Grid.DiseaseCount[cell];

				var totalMass = mass + deltaMass;
				totalMass = Mathf.Max(totalMass, 0);

				SimMessages.ModifyMass(cell, totalMass, diseaseIdx, diseaseCount, cellModifyEvent);

				if (deltaMass > 0)
				{
					Log.Debug($"dropping: {deltaMass}");
					storage.DropSome(element.CreateTag(), deltaMass);
				}
				else
				{
					Log.Debug($"storing: {deltaMass}");
					storage.ForceStore(elementOnPosition, deltaMass);
				}
			}
		}

		public void ToggleWall(bool isWalled)
		{
			isWalledUp = isWalled;
			smi.GoTo(isWalledUp ? smi.sm.walledUp : smi.sm.idle);
		}

		public void OnImguiDraw()
		{
			ImGui.Text($"pressure: {massPerCell}");
			if (ImGui.Button("Top up"))
				Initialize(ElementLoader.FindElementByHash(element));
		}

		public class StatesInstance(ElementSourceVista master)
			: GameStateMachine<States, StatesInstance, ElementSourceVista, object>.GameInstance(master)
		{
		}

		public class States : GameStateMachine<States, StatesInstance, ElementSourceVista>
		{
			public State idle;
			public State walledUp;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.EnterTransition(walledUp, smi => smi.master.isWalledUp)
					.Update((smi, dt) => smi.master.DoExchange(dt));

				walledUp
					.DoNothing();
			}
		}
	}
}
*/