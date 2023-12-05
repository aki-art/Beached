using Beached.Content.ModDb;
using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class CollarWearer : KMonoBehaviour, ISim1000ms
	{
		[MyCmpReq] private KSelectable kSelectable;

		[SerializeField] public Storage storage;
		private Guid statusItem;
		private bool wasInRoom;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.ChangeRoom, OnChangeRoom);
			statusItem = kSelectable.AddStatusItem(BStatusItems.controllerByCollarDispenser, null);

			kSelectable.ToggleStatusItem(BStatusItems.controllerByCollarDispenser, statusItem, false);

			UpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));
		}

		private void OnChangeRoom(object obj)
		{
			if (obj is Room room)
				UpdateRoom(room);
		}

		private void UpdateRoom(Room room)
		{
			if (room?.cavity == null)
			{
				Log.Debug("null room");
				return;
			}

			if (room.cavity.TryGetCollarDispenser(out var collarDispenser) && collarDispenser.HasConfiguration)
			{
				Log.Debug("with a dispenser");
				kSelectable.ToggleStatusItem(BStatusItems.controllerByCollarDispenser, statusItem, true, collarDispenser);

				wasInRoom = true;
			}
			else
			{
				if (wasInRoom)
				{
					kSelectable.ToggleStatusItem(BStatusItems.controllerByCollarDispenser, statusItem, false);

					wasInRoom = false;
				}
			}
		}

		public bool IsAllowedToKill(KPrefabID creature)
		{
			var room = Game.Instance.roomProber.GetRoomOfGameObject(creature.gameObject);

			return room == null
				|| !room.cavity.TryGetCollarDispenser(out var collarDispenser)
				|| collarDispenser.CanCull(creature.PrefabTag);
		}

		// this just updates the status item, so doesn't need to be frequent
		public void Sim1000ms(float dt)
		{
			UpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));
		}
	}
}
