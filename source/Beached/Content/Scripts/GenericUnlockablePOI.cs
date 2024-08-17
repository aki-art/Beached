using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	internal class GenericUnlockablePOI : GameStateMachine<GenericUnlockablePOI, GenericUnlockablePOI.Instance, IStateMachineTarget, GenericUnlockablePOI.Def>
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
					.PlayAnim("on", KAnim.PlayMode.Loop)
					.ParamTransition(isUnlocked, unlocked, IsTrue);

			unlocked
					.ParamTransition(seenNotification, unlocked.notify, IsFalse)
					.ParamTransition(seenNotification, unlocked.done, IsTrue);

			unlocked.notify
					.PlayAnim("notify", KAnim.PlayMode.Loop)
					.ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired)
					.ToggleNotification(smi =>
					{
						smi.notificationReference = EventInfoScreen.CreateNotification(GenerateEventPopupData(smi));
						smi.notificationReference.Type = NotificationType.MessageImportant;
						return smi.notificationReference;
					});

			unlocked.done
					.PlayAnim("off");
		}

		private static string GetMessageBody(Instance smi)
		{
			string str = "";
			foreach (TechItem unlockTechItem in smi.unlockTechItems)
				str = $"{str}\n    • {unlockTechItem.Name}";

			return string.Format(global::STRINGS.MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, str);
		}

		private static EventInfoData GenerateEventPopupData(Instance smi)
		{
			var eventPopupData = new EventInfoData(
				global::STRINGS.MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME,
				GetMessageBody(smi),
				smi.def.animName);

			var length = Mathf.Max(2, Components.LiveMinionIdentities.Count);
			GameObject[] gameObjectArray = new GameObject[length];

			// todo: actual dupe who went to unlock
			using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle().GetEnumerator())
			{
				for (int index = 0; index < length; ++index)
				{
					if (!enumerator.MoveNext())
					{
						gameObjectArray = [];
						break;
					}
					gameObjectArray[index] = enumerator.Current.gameObject;
				}
			}

			eventPopupData.minions = gameObjectArray;
			if (smi.def.loreUnlockId != null)
				eventPopupData.AddOption(global::STRINGS.MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE).callback = () =>
				{
					smi.sm.seenNotification.Set(true, smi);
					smi.notificationReference = null;
					Game.Instance.unlocks.Unlock(smi.def.loreUnlockId);
					ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId);
				};

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
			public string animName;
			public string loreUnlockId;
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
						DebugUtil.DevAssert(false, "Invalid tech item " + poiTechUnlockId + " for POI Tech Unlock");
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
				if (def.spawnPrefabs != null)
				{
					foreach (var prefab in def.spawnPrefabs)
						FUtility.Utils.Spawn(prefab, gameObject);
				}
			}

			private void UpdateUnlocked()
			{
				bool flag = true;
				foreach (TechItem unlockTechItem in unlockTechItems)
				{
					if (!unlockTechItem.IsComplete())
					{
						flag = false;
						break;
					}
				}
				sm.isUnlocked.Set(flag, smi);
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
				Workable component = smi.master.GetComponent<GenericUnlockablePOIWorkable>();
				Prioritizable.AddRef(gameObject);
				Trigger((int)GameHashes.UIRefresh);

				unlockChore = new WorkChore<GenericUnlockablePOIWorkable>(Db.Get().ChoreTypes.Research, component);
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
