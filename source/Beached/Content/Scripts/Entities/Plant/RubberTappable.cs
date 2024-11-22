using Beached.Content.ModDb;
using Beached.Utils;
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
		[SerializeField] public Storage metalStorage;
		[SerializeField] public SimHashes element;

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

		public void SetTappedState(bool tapped)
		{
			isTapped = tapped;
			Trigger(ModHashes.sidesSreenRefresh);
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
			tapOrdered = true;
			smi.GoTo(smi.sm.tapOrdered);
		}

		private void UnTap()
		{
			SetTappedState(false);
			smi.GoTo(smi.sm.notTapped);
		}

		private void CancelTapOrder()
		{
			smi.GoTo(smi.sm.notTapped);
		}

		public override void OnSpawn()
		{
			smi.StartSM();

			if (tapOrdered)
				smi.GoTo(smi.sm.tapOrdered);
			else if (isTapped)
				smi.GoTo(smi.sm.growing);

			Trigger(ModHashes.sidesSreenRefresh);
		}

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
				kbac = master.GetComponent<KBatchedAnimController>();
				primaryElement = master.GetComponent<PrimaryElement>();

				trackSymbol = master.trackSymbol;
				latexPerSecond = smi.master.materialPerCycle / CONSTS.CYCLE_LENGTH;
				latexStorage = master.materialStorage;
				element = ElementLoader.FindElementByHash(master.element);

				kbac.SetSymbolVisiblity(master.trackSymbol, false);
			}

			public void SetupBucket()
			{
				Beached.Log.Debug("setting up bucket");

				var gameObject = new GameObject("Beached_RubberBucket");
				gameObject.SetActive(false);

				var column = (Vector3)kbac
					.GetSymbolTransform(trackSymbol, out bool _)
					.GetColumn(3) with
				{
					z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
				};

				gameObject.transform.SetParent(transform);
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

				bucketKbac.initialAnim = "place";
			}
		}

		public class States : GameStateMachine<States, StatesInstance, RubberTappable>
		{
			public State notTapped;
			public State tapOrdered;
			public State untapping;
			public State growing;
			public CollectingStates collecting;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = notTapped;

				notTapped
					.Exit(EnableOverlay)
					.Enter(DisableOverlay);

				tapOrdered
					.Enter(smi => smi.bucketKbac.Play("place"))
					.Exit(smi => smi.master.tapOrdered = false)
					.ToggleFetch(CreateFetch, growing);

				growing
					.EventHandlerTransition(GameHashes.Grow, collecting, IsGrown)
					.EnterTransition(collecting, IsGrown);

				collecting
					.Enter(smi => smi.master.SetTappedState(true))
					.Enter(smi => smi.bucketKbac.Play("collecting"))
					.DefaultState(collecting.running);

				collecting.blocked
					.EventTransition(GameHashes.WiltRecover, collecting)
					.ToggleStatusItem(BStatusItems.collectingRubberHalted);

				collecting.running
					.EventTransition(GameHashes.Wilt, collecting.blocked)
					.UpdateTransition(collecting.full, FillBucket, UpdateRate.SIM_1000ms)
					.ToggleStatusItem(BStatusItems.collectingRubber);

				collecting.full
					.EventHandlerTransition(GameHashes.OnStorageChange, collecting.running, (smi, _) => !smi.latexStorage.IsFull())
					// todo: toggle clear chore
					.ToggleStatusItem(BStatusItems.collectingRubberFull);

				untapping
					.Enter(DropStorage)
					.GoTo(notTapped);
			}

			private bool IsGrown(StatesInstance smi)
			{
				// TODO
				return true;
			}

			private bool IsGrown(StatesInstance smi, object _) => IsGrown(smi);

			private FetchList2 CreateFetch(StatesInstance smi)
			{
				// todo: pickable metal ore
				var fetchList = new FetchList2(smi.master.metalStorage, Db.Get().ChoreTypes.Fetch);
				fetchList.Add([Elements.zincOre.CreateTag()], amount: 50f);

				return fetchList;
			}

			private void DropStorage(StatesInstance smi)
			{
				smi.master.materialStorage.DropAll();
				smi.master.metalStorage.DropAll();
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
				 
				//smi.kbac.SetSymbolVisiblity(smi.master.trackSymbol, true);
				smi.bucketKbac.gameObject.SetActive(true);
				smi.bucketKbac.enabled = true;
			}

			private void DisableOverlay(StatesInstance smi)
			{
				if (smi.bucketKbac == null)
					return;

				//smi.kbac.SetSymbolVisiblity(smi.master.trackSymbol, false);
				smi.bucketKbac.gameObject.SetActive(false);
				smi.bucketKbac.enabled = false;
			}
		}
	}
}
