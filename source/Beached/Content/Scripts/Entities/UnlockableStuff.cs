using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class UnlockableStuff : GameStateMachine<UnlockableStuff, UnlockableStuff.Instance, IStateMachineTarget, UnlockableStuff.Def>
	{
		public UnlockedStates unlocked;
		public BoolParameter pendingChore;
		public BoolParameter seenNotification;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = unlocked;
			serializable = SerializeType.ParamsOnly;

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

		public class UnlockedStates : State
		{
			public State notify;
			public State done;
		}

		private static string GetMessageBody(Instance smi)
		{
			var buildings = "";
			foreach (var unlockTechItem in smi.GetTechItems())
				buildings = $"{buildings}\n    • {unlockTechItem.Name}";

			return string.Format(STRINGS.MISC.NOTIFICATIONS.BEACHED_SLEEPINGMUFFINS.MESSAGEBODY, buildings);
		}

		private static EventInfoData GenerateEventPopupData(Instance smi)
		{
			var eventPopupData = new EventInfoData(
				STRINGS.MISC.NOTIFICATIONS.BEACHED_SLEEPINGMUFFINS.NAME,
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
			public string animName;
			public string loreUnlockId;
		}

		public new class Instance : GameInstance
		{
			private List<TechItem> unlockTechItems;
			public Notification notificationReference;

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

			public List<TechItem> GetTechItems()
			{
				if (unlockTechItems == null)
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

				return unlockTechItems;
			}
			public override void StartSM()
			{
				Subscribe((int)GameHashes.SelectObject, OnBuildingSelect);
				base.StartSM();
			}

			public override void StopSM(string reason)
			{
				Unsubscribe((int)GameHashes.SelectObject, OnBuildingSelect);
				base.StopSM(reason);
			}

			public void OnBuildingSelect(object obj)
			{
				if (!((Boxed<bool>)obj).value || sm.seenNotification.Get(this) || notificationReference == null)
					return;

				notificationReference.customClickCallback(notificationReference.customClickData);
			}

			public void UnlockTechItems()
			{
				foreach (var unlockTechItem in GetTechItems())
					unlockTechItem?.POIUnlocked();

				MusicManager.instance.PlaySong("Stinger_ResearchComplete");
			}

			public void SpawnPrefabs()
			{
				if (def.spawnPrefabs == null)
					return;

				foreach (var prefab in def.spawnPrefabs)
				{
					var entity = FUtility.Utils.Spawn(prefab, gameObject);
					if (entity.TryGetComponent(out Effects effects))
						effects.Add(BEffects.DAZED, true);
				}
			}
		}
	}
}
