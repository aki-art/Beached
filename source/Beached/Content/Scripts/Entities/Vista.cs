using Beached.Integration;

namespace Beached.Content.Scripts.Entities
{
	public class Vista : KMonoBehaviour
	{
		[MyCmpGet] private KPrefabID kPrefabID;

		private CavityInfo currentCavity;

		public override void OnPrefabInit()
		{
			gameObject.AddTag(BTags.FastTrack_registerRoom);
			gameObject.AddTag(BTags.vista);

			if (Mod.integrations.IsModPresent(Integrations.FAST_TRACK))
				Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
		}

		private void OnUpdateRoom(object obj)
		{
			if (obj is Room room)
				UpdateRoom(room?.cavity);
		}

		public override void OnCleanUp() => RemoveVista();

		public void UpdateRoom(CavityInfo cavity)
		{
			if (Game.IsQuitting() || cavity == currentCavity)
				return;

			RemoveVista();

			if (cavity != null)
				cavity.AddNaturePOI(kPrefabID);

			currentCavity = cavity;
		}

		private void RemoveVista()
		{
			if (currentCavity != null)
				currentCavity.RemoveNaturePOI(kPrefabID);
		}
	}
}
