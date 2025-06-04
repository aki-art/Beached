using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts
{
	// very similar to PoiUnlockable but a bit more configurable
	public class GenericUnlockablePOI : GameStateMachine<GenericUnlockablePOI, GenericUnlockablePOI.Instance, IStateMachineTarget, GenericUnlockablePOI.Def>
	{
		public State locked;
		public UnlockedStates unlocked;
		public BoolParameter isUnlocked;
		public BoolParameter pendingChore;
		public BoolParameter seenNotification;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = locked;
			serializable = SerializeType.ParamsOnly;

			locked
					.ParamTransition(isUnlocked, unlocked, IsTrue);

			unlocked
					.ParamTransition(seenNotification, unlocked.notify, IsFalse)
					.ParamTransition(seenNotification, unlocked.done, IsTrue);

			unlocked.notify
					.ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired)
					.ToggleNotification(smi =>
					{
						smi.notificationReference = EventInfoScreen.CreateNotification(GenerateEventPopupData(smi));
						smi.notificationReference.Type = NotificationType.MessageImportant;
						return smi.notificationReference;
					});

			unlocked.done
					.Enter(smi =>
					{
						smi.SpawnPrefabs();
						Util.KDestroyGameObject(smi.gameObject);
					});
		}

		private static string GetMessageBody(Instance smi)
		{
			var buildings = "";
			foreach (TechItem unlockTechItem in smi.unlockTechItems)
				buildings = $"{buildings}\n    • {unlockTechItem.Name}";

			return string.Format(smi.def.messageBody, buildings);
		}

		private static EventInfoData GenerateEventPopupData(Instance smi)
		{
			var eventPopupData = new EventInfoData(
				smi.def.popUpName,
				GetMessageBody(smi),
				smi.def.animName);

			var length = Mathf.Max(2, Components.LiveMinionIdentities.Count);

			var minions = new GameObject[length];

			if (length > 0)
				minions = Components.LiveMinionIdentities
					.Shuffle()
					.Take(length)
					.Select(identity => identity.gameObject)
					.ToArray();

			eventPopupData.minions = minions;

			eventPopupData.AddDefaultOption(() =>
			{
				smi.sm.seenNotification.Set(true, smi);
				smi.notificationReference = null;
			});

			eventPopupData.clickFocus = smi.gameObject.transform;

			return eventPopupData;
		}

		public class Def : BaseDef
		{
			public List<string> techUnlockIDs;
			public List<string> spawnPrefabs;
			public LocString popUpName;
			public LocString messageBody;
			public string animName;
			public string loreUnlockId;
			public Action<GameObject> onSpawnFn;
		}

		public new class Instance : GameInstance, ISidescreenButtonControl
		{
			public List<TechItem> unlockTechItems;
			public Notification notificationReference;
			private Chore unlockChore;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				unlockTechItems = new List<TechItem>(def.techUnlockIDs.Count);
				foreach (string poiTechUnlockId in def.techUnlockIDs)
				{
					var techItem = Db.Get().TechItems.TryGet(poiTechUnlockId);
					if (techItem != null)
						unlockTechItems.Add(techItem);
					else
						Beached.Log.Warning("Invalid tech item " + poiTechUnlockId + " for POI Tech Unlock");
				}
			}


			public override void StartSM()
			{
				Subscribe((int)GameHashes.SelectObject, OnBuildingSelect);
				UpdateUnlocked();
				base.StartSM();

				if (!sm.pendingChore.Get(this) || unlockChore != null)
					return;

				CreateChore();
			}

			public override void StopSM(string reason)
			{
				Unsubscribe((int)GameHashes.SelectObject, OnBuildingSelect);
				base.StopSM(reason);
			}

			public void OnBuildingSelect(object obj)
			{
				if (!(bool)obj || sm.seenNotification.Get(this) || notificationReference == null)
					return;

				notificationReference.customClickCallback(notificationReference.customClickData);
			}

			public void UnlockTechItems()
			{
				foreach (var unlockTechItem in unlockTechItems)
					unlockTechItem?.POIUnlocked();

				MusicManager.instance.PlaySong("Stinger_ResearchComplete");
				UpdateUnlocked();
			}

			public void SpawnPrefabs()
			{
				if (def.spawnPrefabs == null)
					return;

				foreach (var prefab in def.spawnPrefabs)
				{
					var go = FUtility.Utils.Spawn(prefab, gameObject.transform.position + Vector3.up);
					def.onSpawnFn?.Invoke(go);
				}
			}

			private void UpdateUnlocked()
			{
				bool stillNeedsunlocking = true;
				foreach (var unlockTechItem in unlockTechItems)
				{
					if (!unlockTechItem.IsComplete())
					{
						stillNeedsunlocking = false;
						break;
					}
				}

				sm.isUnlocked.Set(stillNeedsunlocking, smi);
			}

			public string SidescreenButtonText
			{
				get
				{
					if (sm.isUnlocked.Get(smi))
						return global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;

					return unlockChore != null
						? global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF :
						global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
				}
			}

			public string SidescreenButtonTooltip
			{
				get
				{
					if (sm.isUnlocked.Get(smi))
						return global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;

					return unlockChore != null ? global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF : global::STRINGS.UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
				}
			}

			public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
			{
			}

			public bool SidescreenEnabled() => smi.IsInsideState(sm.locked);

			public bool SidescreenButtonInteractable() => smi.IsInsideState(sm.locked);

			public void OnSidescreenButtonPressed()
			{
				if (unlockChore == null)
				{
					smi.sm.pendingChore.Set(true, smi);
					smi.CreateChore();
				}
				else
				{
					smi.sm.pendingChore.Set(false, smi);
					smi.CancelChore();
				}
			}

			private void CreateChore()
			{
				var workable = smi.master.GetComponent<GenericUnlockablePOIWorkable>();
				Prioritizable.AddRef(gameObject);
				Trigger((int)GameHashes.UIRefresh);

				unlockChore = new WorkChore<GenericUnlockablePOIWorkable>(Db.Get().ChoreTypes.Research, workable);
			}

			private void CancelChore()
			{
				unlockChore.Cancel("UserCancel");
				unlockChore = null;
				Prioritizable.RemoveRef(gameObject);
				Trigger((int)GameHashes.UIRefresh);
			}

			public int HorizontalGroupID() => -1;

			public int ButtonSideScreenSortOrder() => 20;
		}

		public class UnlockedStates : State
		{
			public State notify;
			public State done;
		}
	}
}
