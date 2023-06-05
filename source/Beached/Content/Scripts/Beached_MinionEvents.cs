using Beached.Content.ModDb;
using Klei.AI;
using KSerialization;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_MinionEvents : KMonoBehaviour, ISim1000ms
	{
		public bool isUnderShootingStars;

		[MyCmpReq] private Effects effects;

		public override void OnSpawn()
		{
			base.OnSpawn();
			UpdateWishingStarEvent();
			Beached_Mod.Instance.wishingStarEvent.Subscribe(ModHashes.wishingStarEvent, OnWishingStarEvent);
		}

		private void OnWishingStarEvent(object data)
		{
			if (data is int worldId && worldId == this.GetMyWorldId())
			{
				UpdateWishingStarEvent();
			};
		}

		private void UpdateWishingStarEvent()
		{
			var myWorldId = this.GetMyWorldId();
			isUnderShootingStars = Beached_Mod.Instance.wishingStarEvent.DoesWorldHaveShooties(myWorldId);
		}

		public void Sim1000ms(float dt)
		{
			if (isUnderShootingStars)
			{
				var cell = Grid.PosToCell(this);
				var zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
				if (zoneType == ProcGen.SubWorld.ZoneType.Space)
				{
					effects.Add(BEffects.WISHING_STAR, true); // adds to duration if already existing
				}
			}
		}
	}
}
