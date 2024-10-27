using Beached.Content.ModDb;
using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.Plant
{
	public class RubberTappable : StateMachineComponent<RubberTappable.StatesInstance>
	{
		[SerializeField] public string trackSymbol;
		[SerializeField] public float materialPerCycle;
		[SerializeField] public Storage materialStorage;
		[SerializeField] public SimHashes element;

		private KBatchedAnimController bucketKbac;

		[Serialize] public bool isTapped;
		[Serialize] public bool tapOrdered;

		public string SidescreenButtonText
		{
			get
			{
				if (isTapped)
					return STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.REMOVE_TAP;

				if (tapOrdered)
					return STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.CANCEL_TAP;

				return STRINGS.UI.BEACHED_USERMENUACTIONS.TAPPABLE.TAP;
			}
		}

		public void OnSidescreenButtonPressed()
		{
			if (tapOrdered)
			{
				CancelTapOrder();
				return;
			}

			if(isTapped)
			{
				UnTap();
				return;
			}

			OrderTap();
		}

		private void OrderTap()
		{
			smi.GoTo(smi.sm.tapOrdered);
		}

		private void UnTap()
		{
			smi.GoTo(smi.sm.notTapped);
		}

		private void CancelTapOrder()
		{
			smi.GoTo(smi.sm.notTapped);
		}

		public override void OnSpawn() => smi.StartSM();

		public static string ResolveStatusItemString(string str, object data)
		{
			if (data is RubberTappable tappable)
			{
				var mass = tappable.materialStorage.MassStored();
				var formattedMass = GameUtil.GetFormattedMass(mass);
				return string.Format(str, formattedMass);
			}

			return str;
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, RubberTappable, object>.GameInstance
		{
			public KBatchedAnimController bucketKbac;
			public KBatchedAnimController kbac;
			public PrimaryElement primaryElement;
			public KBatchedAnimTracker tracker;
			public Storage latexStorage;
			public HashedString trackSymbol;
			public float latexPerSecond;
			public Element element;

			public StatesInstance(RubberTappable master) : base(master)
			{
				master.TryGetComponent(out KBatchedAnimController kbac);
				master.TryGetComponent(out PrimaryElement primaryElement);

				trackSymbol = master.trackSymbol;
				latexPerSecond = smi.master.materialPerCycle / CONSTS.CYCLE_LENGTH;
				latexStorage = master.materialStorage;
				element = ElementLoader.FindElementByHash(master.element);

				kbac.SetSymbolVisiblity(master.trackSymbol, false);
			}

			public void SetupBucket()
			{
				var gameObject = new GameObject("Beached_RubberBucket");
				gameObject.SetActive(false);

				var column = (Vector3)kbac
					.GetSymbolTransform(trackSymbol, out bool _)
					.GetColumn(3) with
				{
					z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
				};

				gameObject.transform.SetPosition(column);
				bucketKbac = gameObject.AddComponent<KBatchedAnimController>();
				bucketKbac.AnimFiles =
				[
					Assets.GetAnim( "beached_rubber_bucket_kanim")
				];

				tracker = gameObject.AddComponent<KBatchedAnimTracker>();
				tracker.symbol = trackSymbol;
				tracker.forceAlwaysVisible = true;

				tracker.SetAnimControllers(bucketKbac, kbac);

				bucketKbac.initialAnim = "collecting";

				kbac.SetSymbolVisiblity((KAnimHashedString)trackSymbol, false);
			}
		}

		public class States : GameStateMachine<States, StatesInstance, RubberTappable>
		{
			public State notTapped;
			public State tapOrdered;
			public State untapping;
			public CollectingStates collecting;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = notTapped;

				notTapped
					.Enter(DisableOverlay);

				tapOrdered
					.GoTo(collecting); // TODO chore

				collecting
					.Enter(EnableOverlay);

				collecting.blocked
					.EventTransition(GameHashes.WiltRecover, collecting)
					.ToggleStatusItem(BStatusItems.collectingRubberHalted);

				collecting.running
					.EventTransition(GameHashes.Wilt, collecting.blocked)
					.UpdateTransition(collecting.full, FillBucket, UpdateRate.SIM_1000ms)
					.ToggleStatusItem(BStatusItems.collectingRubber);

				collecting.full
					.EventHandlerTransition(GameHashes.OnStorageChange, collecting.running, (smi, _) => !smi.latexStorage.IsFull())
					.ToggleStatusItem(BStatusItems.collectingRubberFull);

				untapping
					.Enter(DropStorage)
					.GoTo(notTapped);
			}

			private void DropStorage(StatesInstance smi)
			{
				smi.master.materialStorage.DropAll();
			}

			public class CollectingStates : State
			{
				public State running;
				public State blocked;
				public State full;
			}

			private bool FillBucket(StatesInstance smi, float dt)
			{
				if (smi.latexStorage.IsFull())
					return true;

				var material = smi.element.substance.SpawnResource(
					smi.transform.position,
					smi.latexPerSecond * dt,
					smi.primaryElement.Temperature,
					byte.MaxValue,
					0,
					true);

				smi.latexStorage.Store(material, true);

				return false;
			}

			private void EnableOverlay(StatesInstance smi)
			{
				if (smi.bucketKbac == null)
					smi.SetupBucket();

				smi.kbac.SetSymbolVisiblity(smi.master.trackSymbol, true);
				smi.master.bucketKbac.enabled = true;
			}

			private void DisableOverlay(StatesInstance smi)
			{
				if (smi.master.bucketKbac == null)
					return;

				smi.kbac.SetSymbolVisiblity(smi.master.trackSymbol, false);
				smi.master.bucketKbac.enabled = false;
			}
		}
	}
}
