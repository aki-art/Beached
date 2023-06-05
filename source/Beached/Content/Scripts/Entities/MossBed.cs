using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class MossBed : StateMachineComponent<MossBed.SMInstance>
	{
		private const int MAX_LEVEL = 3;

		public const float WATER_REQUIREMENT_KG = 100f;
		public const float GROWTH_TIME_SECONDS = 10f; //(CONSTS.CYCLE_LENGTH * 2f) / MAX_LEVEL;

		[Serialize]
		public float totalConsumedMass;

		[MyCmpReq]
		private ElementConverter converter;

		[MyCmpReq]
		private KBatchedAnimController kbac;

		private static readonly KAnimHashedString[] anims = new KAnimHashedString[]
		{
			"moss_0",
			"moss_1",
			"moss_2"
		};

		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();

			converter.onConvertMass += OnConsumeWater;
		}

		private void OnConsumeWater(float amount)
		{
			totalConsumedMass += amount;
			var progress = totalConsumedMass / WATER_REQUIREMENT_KG;

			if (progress >= 1f)
			{
				smi.sm.MossGrown.Trigger(smi);
				return;
			}

			UpdateAnimation(progress);
		}

		private void ConvertSelfToMoss()
		{
			var cell = Grid.PosToCell(this);
			var pe = GetComponent<PrimaryElement>();
			var temp = pe.Temperature;
			var diseaseIdx = pe.DiseaseIdx;
			var diseaseCount = pe.DiseaseCount;

			Util.KDestroyGameObject(this);

#if ELEMENTS
			SimMessages.AddRemoveSubstance(
				cell,
				Elements.moss,
				null,
				255,
				temp,
				diseaseIdx,
				diseaseCount);

#endif
			Game.Instance.SpawnFX(ModAssets.Fx.mossplosion, cell, 0);
		}

		private void UpdateAnimation(float progress)
		{
			var animIndex = Mathf.FloorToInt(progress * MAX_LEVEL);
			animIndex = Mathf.Clamp(animIndex, 0, MAX_LEVEL - 1);
			kbac.Play(anims[animIndex]);
		}

		public class States : GameStateMachine<States, SMInstance, MossBed>
		{
			public State off;
			public State growing;
			public State growingDone;
			public State growingDonePst;
			public State waitingForDelivery;

			public Signal MossGrown;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off
					.PlayAnim("empty")
					.EventTransition(GameHashes.OperationalChanged, waitingForDelivery, smi => smi.GetComponent<Operational>().IsOperational);

				waitingForDelivery
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
					.EnterTransition(growing, HasEnoughWater)
					.EventTransition(GameHashes.OnStorageChange, growing, HasEnoughWater);

				growing
					.Enter("SetActive", smi =>
					{
						smi.GetComponent<Operational>().SetActive(true);
						smi.GetComponent<ManualDeliveryKG>().Pause(true, "my purpose has been fulfilled");
					})
					.OnSignal(MossGrown, growingDone)
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.GetComponent<Operational>().IsOperational)
					.ToggleStatusItem("Growing", "");

				growingDone
					.GoTo(growingDonePst)
					.QueueAnim("moss_grown")
					.OnAnimQueueComplete(growingDonePst);

				growingDonePst
					.Enter(smi => smi.master.ConvertSelfToMoss());
			}

			private bool HasEnoughWater(SMInstance smi)
			{
				return smi.GetComponent<Storage>().IsFull();
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, MossBed, object>.GameInstance
		{
			public SMInstance(MossBed master) : base(master) { }
		}
	}
}
