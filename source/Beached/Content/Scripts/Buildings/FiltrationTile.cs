using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: old code, needs cleanup
namespace Beached.Content.Scripts.Buildings
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class FiltrationTile : StateMachineComponent<FiltrationTile.StatesInstance>
	{
		private const float MAX_BUILDUP = 300f;
		private const float SPILL_RATE = 20f;
		private const float MAX_PRESSURE = 1000f;

		private int cell = -1;

		[SerializeField] public List<Tag> filteredElements;

		[MyCmpGet][NonSerialized] public Structure structure;
		[MyCmpGet] private PassiveElementConsumer elementConsumer;
		[MyCmpGet] private Operational operational;
		[MyCmpGet] private Storage storage;

		private ElementConverter[] converters;
		public Chore cleanChore;
		private FiltrationTileWorkable filtrationTileWorkable;
		private Vector3 emitOffset = Vector3.up;

		public enum State
		{
			Invalid,
			Ready,
			Blocked,
			OverPressure
		}

		public override void OnSpawn()
		{
			cell = Grid.CellBelow(Grid.PosToCell(transform.GetPosition()));
			var top = new CellOffset[][]
			{
				[CellOffset.up]
			};

			filtrationTileWorkable = gameObject.AddComponent<FiltrationTileWorkable>();
			filtrationTileWorkable.overrideAnims = [Assets.GetAnim("anim_interacts_outhouse_kanim")];
			filtrationTileWorkable.SetOffsetTable(top);
			filtrationTileWorkable.workTime = 20f;
			filtrationTileWorkable.workLayer = Grid.SceneLayer.BuildingFront;

			converters = GetComponents<ElementConverter>();
			foreach (var converter in converters)
			{
				converter.SetStorage(storage);
				filteredElements.AddRange(new List<ElementConverter.ConsumedElement>(converter.consumedElements).Select(e => e.Tag).ToList());
			}

			storage.SetOffsetTable(top);
			smi.StartSM();

			Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged);
		}

		// critters cannot reach inside a solid tile so they get this
		private void OnStorageChanged(object obj)
		{
			if (storage.IsEmpty())
				return;


			foreach (var item in storage.items)
			{
				var y = this.transform.position.y + 1.0f;
				if (item.transform.position.y < y)
				{
					item.transform.SetPosition(item.transform.position + Vector3.up);
				}
			}
		}

		private void EmitLiquid(int cell)
		{
			foreach (var stored_item in storage.items)
			{
				var primary_element = stored_item.GetComponent<PrimaryElement>();

				if (primary_element.Element.IsLiquid && !filteredElements.Contains(primary_element.Element.tag) && primary_element.Mass > 0f)
				{
					var spill = Mathf.Min(primary_element.Mass, SPILL_RATE);
					storage.ConsumeAndGetDisease(primary_element.ElementID.CreateTag(), spill, out _, out var diseaseInfo, out var temperature);

					if ((Grid.IsValidCell(cell) && !Grid.Solid[cell]))
					{
						FallingWater.instance.AddParticle(cell, (byte)ElementLoader.elements.IndexOf(primary_element.Element), spill, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, spill, temperature, diseaseInfo.idx, diseaseInfo.count);
					}

					break;
				}
			}
		}

		public bool HasEnoughMass()
		{
			return Array.Exists(converters, c => c.HasEnoughMassToStartConverting());
		}

		public bool IsClogged()
		{
			return storage.GetAmountAvailable(GameTags.Solid) >= MAX_BUILDUP;
		}

		private void Filter(float dt)
		{
			Debug.Log("Filtering");
			if (storage.items.Count != 0 && !Grid.Solid[cell])
			{
				EmitLiquid(cell);
			}
		}

		private void CheckState()
		{
			Debug.Log("Checking state");
			switch (GetEndPointState())
			{
				case State.Blocked:
					smi.GoTo(smi.sm.blocked);
					break;
				case State.OverPressure:
					smi.GoTo(smi.sm.overPressure);
					break;
				default:
					smi.GoTo(smi.sm.waiting);
					break;
			}
		}
		private bool IsValidOutputCell(int output_cell)
		{
			var valid = false;
			if (structure == null || !structure.IsEntombed())
			{
				if (!Grid.Solid[output_cell])
				{
					valid = (Grid.Mass[output_cell] < MAX_PRESSURE);
				}
			}
			return valid;
		}

		public State GetEndPointState()
		{
			var new_state = State.Ready;
			if (!IsValidOutputCell(cell))
			{
				new_state = (Grid.Solid[cell] ? State.Blocked : State.OverPressure);
			}
			return new_state;
		}

		public void CreateCleanChore()
		{
			if (cleanChore != null) cleanChore.Cancel("dupe");

			cleanChore = new WorkChore<FiltrationTileWorkable>(
				chore_type: Db.Get().ChoreTypes.EmptyStorage,
				target: filtrationTileWorkable,
				chore_provider: null,
				run_until_complete: true,
				on_complete: new Action<Chore>(OnCleanComplete),
				on_begin: null,
				on_end: null,
				allow_in_red_alert: true,
				schedule_block: null,
				ignore_schedule_block: false,
				only_when_operational: true,
				override_anims: filtrationTileWorkable.overrideAnims[0],
				is_preemptable: false,
				allow_in_context_menu: true,
				allow_prioritization: true,
				priority_class: PriorityScreen.PriorityClass.basic,
				priority_class_value: 5,
				ignore_building_assignment: true,
				add_to_daily_report: true);
		}

		private void OnCleanComplete(Chore chore)
		{
			if (storage == null) return;
			var items = new List<GameObject>();
			storage.Find(GameTags.Solid, items);

			foreach (var item in items)
			{
				storage.Drop(item, true);
				item.transform.SetPosition(item.transform.GetPosition() + emitOffset);
			}
			//base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

		public void CancelCleanChore()
		{
			if (cleanChore != null)
			{
				cleanChore.Cancel("Cancelled");
				cleanChore = null;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, FiltrationTile>
		{
			public State waiting;
			public State filtering;
			public State needsEmptying;
			public State notoperational;
			public State blocked;
			public State overPressure;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = waiting;

				waiting
					.Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
					.Enter(smi => Debug.Log("Waiting"))
					.EventTransition(GameHashes.OnStorageChange, filtering, smi => smi.master.HasEnoughMass());

				filtering
					.Enter(smi => Debug.Log("filtering"))
					.Enter(smi => smi.master.operational.SetActive(true))
					.Update("Filtering", (smi, dt) => { smi.master.Filter(dt); }, UpdateRate.SIM_200ms)
					.Update("CheckState", (smi, dt) => { smi.master.CheckState(); }, UpdateRate.SIM_1000ms)
					.EventTransition(GameHashes.OnStorageChange, waiting, smi => !smi.master.HasEnoughMass())
					.EventTransition(GameHashes.OnStorageChange, needsEmptying, smi => smi.master.IsClogged())
					.Exit(smi => smi.master.elementConsumer.EnableConsumption(false))
					.Exit(smi => smi.master.operational.SetActive(false));

				needsEmptying
					.Enter(smi => smi.master.CreateCleanChore())
					.ToggleStatusItem(
						name: "Needs emptying",
						tooltip: BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP,
						icon: "status_item_need_supply_out",
						icon_type: StatusItem.IconType.Custom,
						notification_type: NotificationType.BadMinor)
					.EventTransition(GameHashes.OnStorageChange, waiting, smi => !smi.master.IsClogged())
					.Exit(smi => smi.master.CancelCleanChore());

				blocked
					.ToggleStatusItem(Db.Get().BuildingStatusItems.LiquidVentObstructed)
					.Update("CheckState", (smi, dt) => { smi.master.CheckState(); }, UpdateRate.SIM_1000ms);

				overPressure
					.ToggleStatusItem(Db.Get().BuildingStatusItems.LiquidVentOverPressure)
					.Update("CheckState", (smi, dt) => { smi.master.CheckState(); }, UpdateRate.SIM_1000ms);
			}

		}


		public class StatesInstance : GameStateMachine<States, StatesInstance, FiltrationTile, object>.GameInstance
		{
			Operational operational;
			public StatesInstance(FiltrationTile smi) : base(smi)
			{
				operational = master.GetComponent<Operational>();
			}

			public void Test(string msg)
			{
				Debug.Log(msg);
			}

		}
	}

}
