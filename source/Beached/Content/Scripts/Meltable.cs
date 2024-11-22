using Beached.Content.ModDb;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class Meltable : Workable
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private PrimaryElement primaryElement;
		[MyCmpGet] private KBatchedAnimHeatPostProcessingEffect heatEffect;
		[MyCmpGet] private SimTemperatureTransfer temperatureTransfer;
		private WorkChore<Meltable> chore;
		private HandleVector<int>.Handle structureTemperature;
		[Serialize] protected bool isMarkedForThawing;
		[Serialize] protected bool harvestWhenReady;
		[Serialize] protected float startTemperature;

		[SerializeField] public float selfHeatKW;

		private static float MELT = 273.2f;

		protected Meltable()
		{
			faceTargetWhenWorking = true;
			SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}

		public override float GetPercentComplete() => Mathf.Clamp01((MELT - primaryElement.Temperature) / (MELT - startTemperature));

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = BStatusItems.thawing;

			attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
			skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

			multitoolContext = ModAssets.CONTEXTS.THAWING;
			multitoolHitEffectTag = EffectConfigs.BuildSplashId;

			faceTargetWhenWorking = true;

			SetWorkTime(80f);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			structureTemperature = GameComps.StructureTemperatures.GetHandle(gameObject);

			if (isMarkedForThawing)
				CreateThawChore();

			SetWorkTime(float.PositiveInfinity);
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
		}

		private void CreateThawChore()
		{
			chore = new WorkChore<Meltable>(Db.Get().ChoreTypes.Demolish, this);
		}

		public virtual void OnRefreshUserMenu(object data)
		{
			if (harvestWhenReady)
			{
				MiscUtil.AddSimpleButton(gameObject, "status_item_plant_temperature", "Cancel Thawing", "TODO", OnClickCancelHarvestWhenReady);
			}
			else
			{
				MiscUtil.AddSimpleButton(gameObject, "status_item_plant_temperature", "Thaw Out", "TODO", OnClickHarvestWhenReady);
			}
		}

		private void OnClickHarvestWhenReady()
		{
			isMarkedForThawing = true;
			if (chore == null)
			{
				CreateThawChore();
				// todo: add status item
			}
		}

		private void OnClickCancelHarvestWhenReady()
		{
			isMarkedForThawing = false;
			if (chore != null)
			{
				chore.Cancel("Cancel thaw");
				chore = null;
			}
		}

		private float AddSelfHeat(float dt)
		{
			//temperatureTransfer.ModifyEnergy(selfHeatKW * dt);

			primaryElement.SetTemperature((selfHeatKW * dt) + primaryElement.Temperature);

			if (heatEffect != null)
				heatEffect.SetHeatBeingProducedValue(selfHeatKW);

			return selfHeatKW;
		}

		public override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			startTemperature = primaryElement.Temperature;
		}

		public override bool OnWorkTick(WorkerBase worker, float dt)
		{
			AddSelfHeat(dt);
			return base.OnWorkTick(worker, dt);
		}

		public override void OnStopWork(WorkerBase worker)
		{
			base.OnStopWork(worker);

			if (heatEffect != null)
				heatEffect.SetHeatBeingProducedValue(0);
		}
	}
}
