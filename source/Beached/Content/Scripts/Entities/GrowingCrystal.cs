using Beached.Content.ModDb;
using ImGuiNET;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class GrowingCrystal : StateMachineComponent<GrowingCrystal.StatesInstance>, IImguiDebug
	{
		[SerializeField] public float maxLength;
		[SerializeField] public float defaultGrowthRatePercentPerCycle;

		[MyCmpReq] public Crystal crystal;
		[MyCmpReq] public KBatchedAnimController kbac;

		private KAnimLink link;
		public KBatchedAnimController shaftKbac;
		private GameObject shaftGo;
		private Vector3 growthVectorNormalized;

		private float overrideLength = -1;

		public override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.NewGameSpawn, OnGameSpawn);
		}

		private void OnGameSpawn(object _)
		{
			var length = Random.Range(0, maxLength) * (100f / maxLength);
			smi.growth.value = length;
		}

		public override void OnSpawn()
		{
			overrideLength = -1;
			CreateCrystalShaft();

			SetAngle(crystal.angleDeg);
			UpdateShaftLength();

			Subscribe(ModHashes.crystalRotated, data => SetAngle((float)data));
			smi.StartSM();
		}

		public void SetAngle(float angle)
		{
			angle -= 180f;
			angle %= 360f;
			if (angle < 0f) angle += 360f;

			var quaternion = Quaternion.Euler(0.0f, 0.0f, angle);
			growthVectorNormalized = (quaternion * Vector3.up).normalized;

			shaftGo.transform.rotation = quaternion;

			UpdateShaftLength();
		}

		private void CreateCrystalShaft()
		{
			var kbac = GetComponent<KBatchedAnimController>();
			string name = kbac.name + ".shaft";

			shaftGo = new GameObject(name);
			shaftGo.SetActive(false);
			shaftGo.transform.parent = kbac.transform;

			shaftGo.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
			shaftKbac = shaftGo.AddComponent<KBatchedAnimController>();
			shaftKbac.AnimFiles = [kbac.AnimFiles[0]];
			shaftKbac.initialAnim = "crystal";
			shaftKbac.isMovable = true;
			shaftKbac.sceneLayer = Grid.SceneLayer.BuildingBack;

			kbac.SetSymbolVisiblity("crystal_main", false);

			shaftGo.transform.SetPosition(kbac.PositionIncludingOffset with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)
			});

			shaftGo.SetActive(true);

			link = new KAnimLink(kbac, shaftKbac);


			kbac.enabled = false;
			kbac.enabled = true;
		}

		public void UpdateShaftLength()
		{
			if (smi == null || smi.growth == null)
				return;

			var length = overrideLength == -1 ? (smi.growth.value / 100f) : overrideLength;
			length = Mathf.Clamp01(length);
			length *= maxLength;

			// set position of tip to where we are growing
			shaftKbac.Offset = growthVectorNormalized * length;

			Log.Debug($"length: {length}");

			var position = shaftKbac.PositionIncludingOffset;
			shaftKbac.GetBatchInstanceData().SetClipRadius(position.x, position.y, length * length, true);
		}

		public void OnImguiDraw()
		{
			if (ImGui.SliderFloat("override length", ref overrideLength, 0f, 1f))
				UpdateShaftLength();
		}

		public bool IsGrown() => smi.sm.IsMaxLength(smi, 0);

		internal float GetCurrentLength() => smi.growth.value * maxLength;

		public class StatesInstance : GameStateMachine<States, StatesInstance, GrowingCrystal, object>.GameInstance
		{
			public AmountInstance growth;
			public AttributeModifier defaultGrowthModifier;

			public StatesInstance(GrowingCrystal master) : base(master)
			{
				InitGrowth();
			}

			public void InitGrowth()
			{
				growth = BAmounts.CrystalGrowth.Lookup(gameObject);

				growth.hide = false;

				defaultGrowthModifier = new AttributeModifier(
					growth.amount.deltaAttribute.Id,
					master.defaultGrowthRatePercentPerCycle,
					"Growth Rate");

				if (DetailsScreen.Instance != null)
					DetailsScreen.Instance.Trigger((int)GameHashes.UIRefreshData);
			}

			public void OnHarvest()
			{
				smi.growth.value = 0;
				smi.master.UpdateShaftLength();
			}
		}

		public class States : GameStateMachine<States, StatesInstance, GrowingCrystal>
		{
			public State growing;
			public State grown;
			public State blocked;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = growing;

				root
					.EventHandler(ModHashes.crystalHarvested, smi => smi.OnHarvest());

				growing
					.ToggleAttributeModifier("Growth", smi => smi.defaultGrowthModifier)
					.Update(UpdateVisual)
					.UpdateTransition(grown, IsMaxLength);

				grown
					.EventTransition(ModHashes.crystalHarvested, growing)
					.ToggleStatusItem("grown", "");
			}

			private void UpdateVisual(StatesInstance instance, float _)
			{
				instance.master.UpdateShaftLength();
			}

			public bool IsMaxLength(StatesInstance instance, float _)
			{
				return instance.growth.value >= instance.growth.GetMax();
			}
		}
	}
}
