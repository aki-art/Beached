using KSerialization;
using TUNING;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class Meltable : Workable
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpGet] private KBatchedAnimHeatPostProcessingEffect heatEffect;
		private WorkChore<Meltable> chore;
		private HandleVector<int>.Handle structureTemperature;
		[Serialize] protected bool isMarkedForThawing;
		[Serialize] protected bool harvestWhenReady;

		[SerializeField] public float selfHeatKW;

		protected Meltable()
		{
			faceTargetWhenWorking = true;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
			attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
			skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

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
			GameComps.StructureTemperatures.ProduceEnergy(structureTemperature, selfHeatKW * dt, global::STRINGS.BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);

			if (heatEffect != null)
				heatEffect.SetHeatBeingProducedValue(selfHeatKW);

			return selfHeatKW;
		}

		public override bool OnWorkTick(Worker worker, float dt)
		{
			AddSelfHeat(dt);
			return base.OnWorkTick(worker, dt);
		}

		public override void OnStopWork(Worker worker)
		{
			base.OnStopWork(worker);

			if (heatEffect != null)
				heatEffect.SetHeatBeingProducedValue(0);
		}
	}
}
