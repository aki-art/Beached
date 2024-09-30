using Beached.Content.Defs;
using Beached.Content.ModDb;
using Database;
using Klei.AI;
using KSerialization;
using System;
using System.Linq;
using System.Runtime.Serialization;
using TUNING;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class SandBox : Workable, ISim4000ms, IWorkerPrioritizable
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField] public float degradationTime;
		[SerializeField] public string trackingEffect;
		[SerializeField] public string specificEffect;


		[Serialize] public float creationTime;
		[Serialize] public string currentStage;
		[Serialize] public string userChosenTargetStage;

		public const string DEFAULT = "Default";
		public string defaultAnimName = "idle";
		public AttributeModifier artQualityDecorModifier;

		public WorkChore<SandBox> chore;

		private Guid statusItem;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
			attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
			skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			multitoolContext = ModAssets.CONTEXTS.SAND;
			multitoolHitEffectTag = BEffectConfigs.SAND;
			requiredSkillPerk = null;
			faceTargetWhenWorking = true;
			SetOffsetTable(OffsetGroups.InvertedStandardTable);
			SetWorkTime(40f);
			SetReportType(ReportManager.ReportType.PersonalTime);
		}

		private void Poof()
		{
			var effect = FXHelpers.CreateEffect("sculpture_fx_kanim", transform.GetPosition(), transform);
			effect.destroyOnAnimComplete = true;
			effect.transform.SetLocalPosition(Vector3.zero);
			effect.Play("poof");
		}

		public override void OnStartWork(Worker worker)
		{
			base.OnStartWork(worker);
			worker.GetComponent<FaceGraph>().AddExpression(Db.Get().Expressions.Happy);
		}

		public override void OnSpawn()
		{
			if (currentStage.IsNullOrWhiteSpace())
				SetDefault();
			else
				SetStage(currentStage, true);

			shouldShowSkillPerkStatusItem = false;
			base.OnSpawn();
		}

		[OnDeserialized]
		public void OnDeserialized()
		{
			if (currentStage != DEFAULT && Db.GetArtableStages().TryGet(currentStage) == null)
				currentStage = DEFAULT;
		}

		public void SetDefault()
		{
			currentStage = DEFAULT;

			var def = GetComponent<Building>().Def;

			kbac.SwapAnims(def.AnimFiles);
			kbac.Play(defaultAnimName);
			kSelectable.SetName(def.Name);
			// todo: status item
			this.GetAttributes().Remove(artQualityDecorModifier);

			UpdateStatusItem();

			if (currentStage == DEFAULT)
			{
				shouldShowSkillPerkStatusItem = true;
				Prioritizable.AddRef(gameObject);
				chore = new WorkChore<SandBox>(
					Db.Get().ChoreTypes.Relax,
					this,
					allow_in_red_alert: false,
					schedule_block: Db.Get().ScheduleBlockTypes.Recreation,
					allow_prioritization: false,
					priority_class: PriorityScreen.PriorityClass.high);

				chore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
			}


			SimAndRenderScheduler.instance.sim4000ms.Remove(this);
			if (statusItem != null)
				kSelectable.RemoveStatusItem(statusItem);
		}


		public override void OnStopWork(Worker worker)
		{
			base.OnStopWork(worker);
			worker.GetComponent<FaceGraph>().RemoveExpression(Db.Get().Expressions.Happy);
		}

		public override void OnCompleteWork(Worker worker)
		{
			if (worker != null)
				AddEffect(worker);

			if (userChosenTargetStage.IsNullOrWhiteSpace())
			{
				var db = Db.Get();

				var id = GetComponent<KPrefabID>().PrefabID();
				var prefabStages = Db.GetArtableStages().GetPrefabStages(id);

				var newStage = prefabStages
					.Where(stage => stage.statusItem.StatusType != ArtableStatuses.ArtableStatusType.AwaitingArting)
					.GetRandom();

				SetStage(newStage.id, false);

				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, newStage.cheerOnComplete ? db.Emotes.Minion.Cheer : db.Emotes.Minion.Disappointed);
			}
			else
			{
				SetStage(userChosenTargetStage, false);
				userChosenTargetStage = null;
			}

			shouldShowSkillPerkStatusItem = false;
			UpdateStatusItem();
			Prioritizable.RemoveRef(gameObject);
		}

		public void SetStage(string stageId, bool skipEffect)
		{
			if (stageId == DEFAULT)
			{
				SetDefault();
				return;
			}

			var artableStage = Db.GetArtableStages().Get(stageId);

			if (artableStage == null)
			{
				Log.Warning($"Missing stage: {stageId}");
				return;
			}

			currentStage = stageId;

			kbac.SwapAnims([Assets.GetAnim(artableStage.animFile)]);
			kbac.Play(artableStage.anim);

			this.GetAttributes().Remove(artQualityDecorModifier);

			if (artableStage.decor != 0)
			{
				artQualityDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, artableStage.decor, "Art Quality");
				this.GetAttributes().Add(artQualityDecorModifier);
			}

			kSelectable.SetName(artableStage.Name);
			kSelectable.SetStatusItem(Db.Get().StatusItemCategories.Main, artableStage.statusItem, this);

			gameObject.GetComponent<BuildingComplete>().SetDescriptionFlavour(artableStage.Description);

			shouldShowSkillPerkStatusItem = false;
			UpdateStatusItem();

			creationTime = GameClock.Instance.GetTime();
			SimAndRenderScheduler.instance.sim4000ms.Add(this);
			statusItem = kSelectable.AddStatusItem(BStatusItems.sandboxCrumble, this);

			if (!skipEffect)
				Poof();
		}

		public void SetUserChosenTargetState(string stageID)
		{
			SetDefault();
			userChosenTargetStage = stageID;
		}

		public void Sim4000ms(float dt)
		{
			if (currentStage == DEFAULT)
				return;

			var remaining = (creationTime + degradationTime) - GameClock.Instance.GetTime();
			if (remaining <= 0)
			{
				SetDefault();
				Poof();
			}
		}

		public static string ResolveStatusItemString(string str, object data)
		{
			if (data is SandBox sandbox)
			{
				var remaining = (sandbox.creationTime + sandbox.degradationTime) - GameClock.Instance.GetTime();
				return string.Format(str, GameUtil.GetFormattedTime(remaining));
			}

			return str;
		}

		private void AddEffect(Worker worker)
		{
			if (worker.TryGetComponent(out Effects effects))
			{
				if (!specificEffect.IsNullOrWhiteSpace())
					effects.Add(specificEffect, true);

				if (!trackingEffect.IsNullOrWhiteSpace())
					effects.Add(trackingEffect, true);
			}
		}

		public bool GetWorkerPriority(Worker worker, out int priority)
		{
			priority = RELAXATION.PRIORITY.TIER3;

			if (worker.TryGetComponent(out Effects effects))
			{
				if (!trackingEffect.IsNullOrWhiteSpace() && effects.HasEffect(trackingEffect))
				{
					priority = 0;
					return false;
				}

				if (!specificEffect.IsNullOrWhiteSpace() && effects.HasEffect(specificEffect))
				{
					priority = RELAXATION.PRIORITY.RECENTLY_USED;
					return false;
				}
			}

			return true;
		}
	}
}
