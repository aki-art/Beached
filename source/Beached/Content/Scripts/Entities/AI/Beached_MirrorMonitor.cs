using Beached.Content.ModDb;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{
	public class Beached_MirrorMonitor : KMonoBehaviour, ISim1000ms
	{
		private bool hasMirrorBuff;
		[MyCmpReq] private KSelectable kSelectable;
		private OvercrowdingMonitor.Def overCrowdingDef;
		private Guid statusItem;

		[SerializeField] public int originalSpaceRequirement;

		public override void OnSpawn()
		{
			overCrowdingDef = gameObject.GetDef<OvercrowdingMonitor.Def>();

			RecheckMirrors(Game.Instance.roomProber.GetRoomOfGameObject(gameObject), true);

			//statusItem = kSelectable.AddStatusItem(BStatusItems.mirror);
			Sim1000ms(0);
		}

		private void RecheckMirrors(Room room, bool forceUpdate)
		{
			var shouldHaveMirrorBuff = room?.cavity != null && room.cavity.GetMirrorCount() > 0;

			if (hasMirrorBuff != shouldHaveMirrorBuff || forceUpdate)
			{
				var mult = shouldHaveMirrorBuff ? 0.5f : 1f;
				overCrowdingDef.spaceRequiredPerCreature = Mathf.FloorToInt(originalSpaceRequirement * mult);
				statusItem = kSelectable.ToggleStatusItem(BStatusItems.mirror, statusItem, shouldHaveMirrorBuff);
			}
		}

		public void Sim1000ms(float dt)
		{
			var room = Game.Instance.roomProber.GetRoomOfGameObject(gameObject);
			if (room == null)
				return;

			RecheckMirrors(room, false);
		}
	}
}
