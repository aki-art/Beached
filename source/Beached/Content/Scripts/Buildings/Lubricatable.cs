using Beached.Content.ModDb;
using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class Lubricatable : StateMachineComponent<Lubricatable.StatesInstance>
	{
		[SerializeField] public Storage mucusStorage;
		[SerializeField] public float massPerUseOrPerSecond;
		[SerializeField] public bool isTimedUse;
		[SerializeField] public ManualDeliveryKG delivery;
		[SerializeField] public bool consumeOnFabricationOrderComplete;
		[SerializeField] public event Func<object, bool> consumeOnComplete;

		[MyCmpGet] public Operational operational;

		[Serialize] public bool mucusRequested;

		public Func<Operational, bool> isInUse = operational => operational.IsActive;

		public Lubricatable()
		{
			consumeOnFabricationOrderComplete = true;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var attributes = gameObject.GetAttributes();
			attributes.Add(BAttributes.doorOpeningSpeed);
			attributes.Add(BAttributes.operatingSpeed);
		}

		public override void OnSpawn()
		{
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			smi.StartSM();

			if (delivery == null)
				delivery = GetComponent<ManualDeliveryKG>();

			UpdateDelivery();
			Trigger((int)GameHashes.OnStorageChange, gameObject);
		}

		private void OnRefreshUserMenu(object _)
		{
			var text = mucusRequested ? "Disable lubrication" : "Enable lubrication";
			var toolTip = mucusRequested
				? "Stop delivering Mucus to this buildings."
				: "Enchance this building by lubricating it with Mucus.";

			var button = new KIconButtonMenu.ButtonInfo("action_bottler_delivery", text, OnToggle, tooltipText: toolTip);

			Game.Instance.userMenu.AddButton(gameObject, button);
		}

		private void OnToggle()
		{
			mucusRequested = !mucusRequested;
			UpdateDelivery();
		}

		private void UpdateDelivery()
		{
			var shouldAcceptDelivery = mucusRequested && !mucusStorage.IsFull();
			delivery.Pause(!shouldAcceptDelivery, "user toggle");

			if (!mucusRequested)
				mucusStorage.DropAll();
		}

		public bool IsInUse() => (bool)isInUse?.Invoke(operational);

		public class StatesInstance(Lubricatable master) : GameStateMachine<States, StatesInstance, Lubricatable, object>.GameInstance(master)
		{
			public Storage mucusStorage = master.mucusStorage;
			public Door door = master.GetComponent<Door>();
		}

		public class States : GameStateMachine<States, StatesInstance, Lubricatable>
		{
			public State idle;
			public LubricatedStates lubricated;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.EventHandlerTransition(GameHashes.OnStorageChange, lubricated, HasLubricantInStorage);

				lubricated
					.DefaultState(lubricated.waitForUse)
					.EnterTransition(PickState)
					.EventHandlerTransition(GameHashes.OnStorageChange, idle, (smi, data) => !HasLubricantInStorage(smi, data))
					.ToggleTag(BTags.lubricated)
					.EventHandler(ModHashes.usedBuilding, OnBuildingUsed)
					.EventHandler(GameHashes.FabricatorOrderCompleted, OnFabricationComplete)
					.EventHandler(GameHashes.WorkableCompleteWork, OnWorkComplete)
					.ToggleStatusItem(BStatusItems.lubricated, smi => smi.master)
					.ToggleEffect(BEffects.LUBRICATED);

				lubricated.periodicResting
					.EventHandlerTransition(GameHashes.OperationalChanged, lubricated.periodicInUse, (smi, data) => smi.master.IsInUse());

				lubricated.periodicInUse
					.EventHandlerTransition(GameHashes.OperationalChanged, lubricated.periodicResting, (smi, data) => !smi.master.IsInUse())
					.Update(UseLubricant, UpdateRate.RENDER_200ms);
			}

			private void OnWorkComplete(StatesInstance smi, object data)
			{
				Log.Debug("work complete check. " + smi.master.name);
				Log.Debug(smi.master.consumeOnComplete != null);
				if (smi.master.consumeOnComplete != null
					&& data is Workable workable
					&& smi.master.consumeOnComplete(workable))
					UseLubricant(smi, 1f);
			}

			private void OnFabricationComplete(StatesInstance smi)
			{
				if (smi.master.consumeOnFabricationOrderComplete)
					UseLubricant(smi, 1f);
			}

			private void OnBuildingUsed(StatesInstance smi, object data)
			{
				if (smi.master.isTimedUse)
					return;

				UseLubricant(smi, 1f);
			}

			private void UseLubricant(StatesInstance smi, float partialAmount)
			{
				smi.mucusStorage.ConsumeIgnoringDisease(Elements.mucus.CreateTag(), smi.master.massPerUseOrPerSecond * partialAmount);
				if (DetailsScreen.Instance != null)
					DetailsScreen.Instance.Trigger((int)GameHashes.UIRefreshData);
			}

			private State PickState(StatesInstance smi)
			{
				if (smi.master.isTimedUse)
					return smi.master.IsInUse() ? smi.sm.lubricated.periodicInUse : smi.sm.lubricated.periodicResting;

				return smi.sm.lubricated.waitForUse;
			}

			private bool HasLubricantInStorage(StatesInstance smi, object data)
			{
				return smi.mucusStorage.GetMassAvailable(Elements.mucus) >= smi.master.massPerUseOrPerSecond;
			}

			public class LubricatedStates : State
			{
				public State waitForUse;
				public State periodicResting;
				public State periodicInUse;
			}
		}

		public static Lubricatable ConfigurePrefab(GameObject prefab, float capacityKg, float massUsedEachTime, bool isTimedUse)
		{
			if (prefab.TryGetComponent(out Lubricatable existingLubricatable))
			{
				existingLubricatable.mucusStorage.capacityKg = capacityKg;
				existingLubricatable.delivery.MinimumMass = massUsedEachTime;
				existingLubricatable.delivery.refillMass = massUsedEachTime;

				return existingLubricatable;
			}

			var storage = prefab.AddComponent<Storage>();
			storage.storageFilters = [Elements.mucus.CreateTag()];
			storage.capacityKg = capacityKg;
			storage.allowItemRemoval = false;

			var delivery = prefab.AddComponent<ManualDeliveryKG>();
			delivery.storage = storage;
			delivery.RequestedItemTag = Elements.mucus.CreateTag();
			delivery.allowPause = false;
			delivery.capacity = capacityKg;
			delivery.MinimumMass = massUsedEachTime;
			delivery.refillMass = massUsedEachTime;
			delivery.choreTypeIDHash = Db.Get().ChoreTypes.MachineTinker.IdHash;
			delivery.Pause(true, "");

			var lubricatable = prefab.AddComponent<Lubricatable>();
			lubricatable.delivery = delivery;
			lubricatable.mucusStorage = storage;
			lubricatable.massPerUseOrPerSecond = massUsedEachTime;
			lubricatable.isTimedUse = isTimedUse;

			prefab.AddOrGet<Effects>();

			return lubricatable;
		}

		public static string ResolveStatusItemTooltipString(string str, object data)
		{
			if (data is not Lubricatable lubricatable)
				return str;

			var maxUses = lubricatable.mucusStorage.capacityKg / lubricatable.massPerUseOrPerSecond;
			var usesRemaining = lubricatable.mucusStorage.GetMassAvailable(Elements.mucus) / lubricatable.massPerUseOrPerSecond;

			str = lubricatable.isTimedUse
				? string.Format(str, GameUtil.GetFormattedTime(usesRemaining))
				: string.Format(str, $"{Mathf.FloorToInt(usesRemaining)}/{maxUses} uses"); // TODO: string entry

			var effect = Db.Get().effects.Get(BEffects.LUBRICATED);
			foreach (var modifier in effect.SelfModifiers)
			{
				var isDoor = lubricatable.smi.door != null;
				var hasDoor = modifier.AttributeId == BAttributes.doorOpeningSpeed.Id && isDoor;
				var hasGenerator = modifier.AttributeId == Db.Get().Attributes.GeneratorOutput.Id && lubricatable.TryGetComponent(out Tinkerable _);
				var hasWorkable = modifier.AttributeId == BAttributes.operatingSpeed.Id && (!isDoor && lubricatable.TryGetComponent(out Workable workable));

				if (hasDoor || hasGenerator || hasWorkable)
				{
					var modifierStr = string.Format(global::STRINGS.DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, modifier.GetName(), modifier.GetFormattedString());
					str += $"{CONSTS.DOT_PREFIX}{modifierStr}\n";
				}
			}

			return str;
		}
	}
}
