using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{

	public class BeachVista : StateMachineComponent<BeachVista.StatesInstance>, IElementEmitter
	{
		[SerializeField] public Storage storage;
		[SerializeField] public float emitPerSecond;
		[SerializeField] public float maxPressure;
		[SerializeField] public List<CellOffset> emissionShape;
		[SerializeField] public SimHashes element;
		[SerializeField] public float howManyCycles;

		[Serialize] public bool hasInitialized;

		private Tag _elementTag;
		private int gameStartedHandler;

		public SimHashes Element => element;

		public float AverageEmitRate => emitPerSecond;

		public override void OnPrefabInit()
		{
			gameStartedHandler = Beached_Mod.Instance.Subscribe(ModHashes.gameStartedForFirstTime, OnNewGameReady);
		}

		private void OnNewGameReady(object obj)
		{
			Initialize();
		}

		public override void OnSpawn()
		{
			if (!hasInitialized)
				Initialize();

			_elementTag = element.CreateTag();
			smi.StartSM();
		}

		public override void OnCleanUp()
		{
			Beached_Mod.Instance.gameObject.Unsubscribe(gameStartedHandler);
		}

		private void Initialize()
		{
			var element = ElementLoader.FindElementByHash(this.element);

			// per second consumption
			var airConsumptionRate = Db.Get().Attributes.AirConsumptionRate;

			var dailyOxyNeed = 0.0f;

			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				var rate = airConsumptionRate.Lookup(minion);
				if (rate != null)
					dailyOxyNeed += rate.GetTotalValue();
			}

			dailyOxyNeed *= CONSTS.CYCLE_LENGTH;
			dailyOxyNeed = Mathf.Max(dailyOxyNeed, CONSTS.CYCLE_LENGTH * 3.0f * TUNING.DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND);

			storage.AddGasChunk(
				element.id,
				dailyOxyNeed * howManyCycles,
				element.defaultValues.temperature,
				byte.MaxValue,
				0,
				false);

			hasInitialized = true;
		}

		public class StatesInstance(BeachVista master) : GameStateMachine<States, StatesInstance, BeachVista, object>.GameInstance(master)
		{
			public Storage storage = master.storage;
			public StatusItem emittingElementStatusItem = Db.Get().BuildingStatusItems.EmittingElement;

			public float GetEmittedMass()
			{
				return master.emitPerSecond;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, BeachVista>
		{
			public State empty;
			public EmittingState emit;
			public BoolParameter hasFreeTiles;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = emit;

				empty
					.ToggleStatusItem("Dormant", "This cave has been permanently exhausted of it's atmosphere, and will not emit any anymore. It can be demolished or kept around for Decor.")
					.EventHandlerTransition(GameHashes.OnStorageChange, emit.emitting, (smi, data) =>
					{
						return smi.storage.GetMassAvailable(smi.master.element) > 0.0f;
					});

				emit
					.DefaultState(emit.emitting)
					.UpdateTransition(empty, EmitStorageContent, UpdateRate.SIM_1000ms);

				emit.emitting
					.ToggleStatusItem(smi => smi.emittingElementStatusItem, smi => smi.master)
					.ParamTransition(hasFreeTiles, emit.blocked, IsFalse);

				emit.blocked
					.ToggleStatusItem("Blocked", "")
					.ParamTransition(hasFreeTiles, emit.emitting, IsTrue);
			}

			public class EmittingState : State
			{
				public State emitting;
				public State blocked;
			}

			/// <returns>Return true if nothing can be emitted anymore, and this feature is ready to retire.</returns>
			private bool EmitStorageContent(StatesInstance smi, float dt)
			{
				if (smi.storage.IsEmpty() || smi.storage.items.Count == 0)
					return true;

				var item = smi.storage.items[0];

				if (item == null)
					return true;

				var amountPerTile = (smi.master.emitPerSecond * dt);

				var freeTiles = 0;

				smi.master.emissionShape.Shuffle();

				foreach (var offset in smi.master.emissionShape)
				{
					var cell = Grid.OffsetCell(Grid.PosToCell(smi), offset);

					if (Grid.Mass[cell] > smi.master.maxPressure)
						continue;

					if (smi.storage.DropSome(smi.master._elementTag, amountPerTile, true, true, offset.ToVector3()))
					{
						Game.Instance.SpawnFX(SpawnFXHashes.OxygenEmissionBubbles, cell, 0.0f);

						freeTiles++;

						smi.sm.hasFreeTiles.Set(true, smi);

						return false;
					}
				}

				if (freeTiles == 0)
					smi.sm.hasFreeTiles.Set(false, smi);

				return false;
			}
		}
	}
}
