using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class PoffShroom : StateMachineComponent<PoffShroom.StatesInstance>
	{
		public const float MAX_GAS_RATIO = 0.1f;

		[MyCmpReq]
		private WiltCondition wiltCondition;

		public struct GasInfo
		{
			public Tag item;
			public Color color;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			GetComponent<ElementConsumer>().EnableConsumption(false);
		}

		public static Dictionary<SimHashes, GasInfo> infos;

		public class StatesInstance : GameStateMachine<States, StatesInstance, PoffShroom, object>.GameInstance
		{
			public Storage storage;
			public KBatchedAnimController kbac;
			public ElementConsumer elementConsumer;
			public KAnimHashedString[] symbolNames = ["simple_ball", "bulb_back", "bulb_front"];

			public StatesInstance(PoffShroom master) : base(master)
			{
				storage = master.GetComponent<Storage>();
				kbac = master.GetComponent<KBatchedAnimController>();
				elementConsumer = master.GetComponent<ElementConsumer>();
			}
		}

		public class States : GameStateMachine<States, StatesInstance, PoffShroom>
		{
			public State dead;
			public AliveState alive;

			public override void InitializeStates(out BaseState default_state)
			{
				serializable = SerializeType.Both_DEPRECATED;
				default_state = alive;

				alive
					.InitializeStates(masterTarget, dead)
					.DefaultState(alive.mature);

				alive.mature
					.ToggleStatusItem("alive.mature", "")
					.Enter(smi => smi.elementConsumer.EnableConsumption(true))
					.EventTransition(GameHashes.Wilt, alive.wilting, smi => smi.master.wiltCondition.IsWilting())
					.EventHandlerTransition(GameHashes.OnStorageChange, alive.full, UpdateColor)
					.Exit(smi => smi.elementConsumer.EnableConsumption(false));

				alive.full
					.EventTransition(GameHashes.Wilt, alive.wilting, smi => smi.master.wiltCondition.IsWilting());

				alive.wilting
					.ToggleStatusItem("alive.wilting", "")
					.EventTransition(GameHashes.WiltRecover, alive.mature, smi => !smi.master.wiltCondition.IsWilting());

				dead
					.ToggleStatusItem("dead", "");
			}

			private PrimaryElement GetTopGas(List<GameObject> items)
			{
				if (items.Count == 0)
				{
					return null;
				}

				if (items.Count == 1)
				{
					return items[0].GetComponent<PrimaryElement>();
				}


				PrimaryElement result = null;
				var mass = 0f;

				foreach (var item in items)
				{
					if (item.TryGetComponent(out PrimaryElement pe) && pe.Mass > mass)
					{
						result = pe;
						mass = pe.Mass;
					}
				}

				return result;
			}

			private bool UpdateColor(StatesInstance smi, object data)
			{
				var items = smi.storage.GetItems();

				var gas = GetTopGas(items);

				if (gas == null)
				{
					foreach (var symbol in smi.symbolNames)
						smi.kbac.SetSymbolTint(symbol, Color.black);

					return false;
				}

				var valid = gas.Mass / smi.storage.MassStored() > MAX_GAS_RATIO;
				var gasColor = valid ? (Color)gas.Element.substance.uiColour : Color.yellow;

				var fullness = smi.storage.MassStored() / smi.storage.capacityKg;
				fullness = Mathf.Clamp01(fullness);

				Log.Debug(fullness);

				var color = Color.Lerp(Color.white, gasColor, fullness);

				foreach (var symbol in smi.symbolNames)
					smi.kbac.SetSymbolTint(symbol, color);

				return smi.storage.IsFull();
			}

			public class AliveState : PlantAliveSubState
			{
				public State mature;
				public State full;
				public State wilting;
			}
		}
	}
}
