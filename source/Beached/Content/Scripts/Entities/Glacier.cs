using KSerialization;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Glacier : KMonoBehaviour
	{
		[SerializeField] public Tag[] rewards;
		[SerializeField] public Vector3 offset;
		[SerializeField] public bool showRewardSilhouette;
		[SerializeField] public string initialAnim;

		[Serialize] public Tag selectedReward;

		[MyCmpReq] private PrimaryElement primaryElement;

		private float meltTemperature;

		private const float MARGIN = 1.0f;
		private static readonly HashedString DISPLAY_TARGET = "item";

		private KBatchedAnimTracker tracker;
		private KBatchedAnimController displayKbac;

		public Glacier()
		{
			initialAnim = "object";
			showRewardSilhouette = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			GetComponent<PrimaryElement>().SetTemperature(GameUtil.GetTemperatureConvertedToKelvin(-30, GameUtil.TemperatureUnit.Celsius));
			meltTemperature = primaryElement.Element.highTemp - MARGIN;

			if (selectedReward == Tag.Invalid)
			{
				if (rewards.Contains(BasicSingleHarvestPlantConfig.SEED_ID) && !Beached_Mod.Instance.hasAtLeastOnceMealwoodGlacier)
				{
					selectedReward = BasicSingleHarvestPlantConfig.SEED_ID;
					Beached_Mod.Instance.hasAtLeastOnceMealwoodGlacier = true;
				}
				else
					selectedReward = rewards.GetRandom();
			}

			UpdateRewardDisplay();
		}

		private void UpdateRewardDisplay()
		{
			if (!showRewardSilhouette)
				return;

			var kbac = GetComponent<KBatchedAnimController>();

			var reward = Assets.GetPrefab(selectedReward);
			if (reward == null || !reward.TryGetComponent(out KBatchedAnimController prefabKbac))
				return;

			var gameObject = new GameObject("Beached_GlacierRewardDisplay");
			gameObject.SetActive(false);

			var column = (Vector3)kbac
				.GetSymbolTransform(DISPLAY_TARGET, out var _)
				.GetColumn(3) with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.Building)
			};


			gameObject.transform.SetPosition(column);

			displayKbac = gameObject.AddComponent<KBatchedAnimController>();
			displayKbac.AnimFiles = [.. prefabKbac.animFiles];
			displayKbac.SetSceneLayer(Grid.SceneLayer.Building);

			tracker = gameObject.AddComponent<KBatchedAnimTracker>();
			tracker.symbol = DISPLAY_TARGET;
			tracker.forceAlwaysVisible = true;

			tracker.SetAnimControllers(displayKbac, kbac);

			displayKbac.initialAnim = prefabKbac.initialAnim;
			displayKbac.TintColour = Color.black;

			kbac.SetSymbolVisiblity((KAnimHashedString)DISPLAY_TARGET, false);

			gameObject.SetActive(true);
		}

		public override void OnCleanUp()
		{
			if (primaryElement.Temperature > meltTemperature)
			{
				if (selectedReward != Tag.Invalid)
				{
					var go = FUtility.Utils.Spawn(selectedReward, gameObject.transform.position + offset);
					if (go.TryGetComponent(out PrimaryElement pe))
						pe.Temperature = primaryElement.Temperature;
				}
			}

			if (!displayKbac.IsNullOrDestroyed())
				Util.KDestroyGameObject(displayKbac.gameObject);
		}
	}
}

/*using KSerialization;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class Glacier : Workable, IApproachable
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private KPrefabID kPrefabID;

		[SerializeField] public List<Tag> tags;
		[SerializeField] public float maxShellHealth = 100f;

		[Serialize] private bool markedForMining;
		[Serialize] public float shellIntegrity;

		private Chore chore;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
			readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;

			resetProgressOnStop = true;
			faceTargetWhenWorking = true;

			attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;

			skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
			skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

			multitoolContext = "dig";
			multitoolHitEffectTag = "fx_dig_splash";

			workingPstComplete = null;
			workingPstFailed = null;

			preferUnreservedCell = true;

			SetOffsetTable(OffsetGroups.InvertedStandardTable);

			Prioritizable.AddRef(gameObject);
		}

		public new int GetCell() => Grid.PosToCell(this);

		public override void OnSpawn()
		{
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);

			if (markedForMining)
				Prioritizable.AddRef(gameObject);

			UpdateStatusItem();
			UpdateChore();
			SetWorkTime(10f);
		}

		private void OnRefreshUserMenu(object data)
		{
			if (IsMineable())
			{
				if (!markedForMining)
				{
					var info = new KIconButtonMenu.ButtonInfo(
						"action_uproot",
						global::STRINGS.UI.USERMENUACTIONS.DIG.NAME,
						() => MarkForDig(true),
						tooltipText: global::STRINGS.UI.USERMENUACTIONS.DIG.TOOLTIP);

					Game.Instance.userMenu.AddButton(gameObject, info);
				}
				else
				{
					var info = new KIconButtonMenu.ButtonInfo(
						"icon_cancel",
						global::STRINGS.UI.USERMENUACTIONS.CANCELDIG.NAME,
						() => MarkForDig(false),
						tooltipText: global::STRINGS.UI.USERMENUACTIONS.CANCELDIG.TOOLTIP);

					Game.Instance.userMenu.AddButton(gameObject, info);
				}
			}
		}

		private void MarkForDig(bool dig)
		{
			MarkForDig(dig, new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
		}

		private void MarkForDig(bool mark, PrioritySetting priority)
		{
			mark = mark && IsMineable();

			if (markedForMining && !mark)
			{
				Prioritizable.RemoveRef(gameObject);
			}
			else if (!markedForMining && mark)
			{
				Prioritizable.AddRef(gameObject);
				if (TryGetComponent(out Prioritizable prioritizable))
				{
					prioritizable.SetMasterPriority(priority);
				}
			}

			markedForMining = mark;
			UpdateStatusItem();
			UpdateChore();
		}

		public bool IsMineable() => true;


		private void UpdateStatusItem()
		{
			shouldShowSkillPerkStatusItem = markedForMining;
			base.UpdateStatusItem(null);

			if (markedForMining)
			{
				kSelectable.AddStatusItem(Db.Get().MiscStatusItems.WaitingForDig);
			}
			else
			{
				kSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.WaitingForDig, false);
			}
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			Log.Debug("COMPLETE");

			MarkForDig(false);
			Destroy(this);
		}

		public override void OnCleanUp()
		{
			if (tags != null)
			{
				foreach (var tag in tags)
					FUtility.Utils.Spawn(tag, gameObject);
			}

			base.OnCleanUp();
		}

		public override bool OnWorkTick(Worker worker, float dt)
		{
			var damage = dt / workTime;
			ApplyDamage(damage);

			return base.OnWorkTick(worker, dt);
		}

		public void ApplyDamage(float damage)
		{
			shellIntegrity -= damage * maxShellHealth;
			// RefreshSymbols(shellIntegrity / maxShellHealth);
		}

		private void UpdateChore()
		{
			if (markedForMining && chore == null)
			{
				chore = new WorkChore<Glacier>(Db.Get().ChoreTypes.Dig, this, is_preemptable: true);
			}
			else
			{
				if (!markedForMining && chore != null)
				{
					chore.Cancel("not marked for mining");
					chore = null;
				}
			}
		}

	}
}
*/