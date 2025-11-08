using Beached.Integration;

namespace Beached.Content.Scripts
{
	// companion to Diggable, assisting Mineralogy leveled dupes to gain extra treasures when digging
	public class TreasureHolder : KMonoBehaviour
	{
		[MyCmpGet] public Diggable diggable;
		private float mass;
		private float temperature;

		private static SimHashes neutroniumDust = (SimHashes)Hash.SDBMLower("UnobtaniumDust");
		private const float dustMultiplier = 0.4f / 10_000f;
		private int cachedCell;

		public override void OnSpawn()
		{
			cachedCell = Grid.PosToCell(this);
			diggable.OnWorkableEventCB += OnDiggableEvent;
			mass = Grid.Mass[cachedCell];


			// needs to check once because the resumed work from a previous game
			// session wont fire the workable event after this components OnSpawn
			if (diggable.worker != null)
				OnStartWork();
		}

		private void OnDiggableEvent(Workable workable, Workable.WorkableEvent evt)
		{
			switch (evt)
			{
				case Workable.WorkableEvent.WorkStarted:
					OnStartWork();
					break;
				case Workable.WorkableEvent.WorkCompleted:
					break;
				case Workable.WorkableEvent.WorkStopped:
					OnStopWork();
					break;
			}
		}

		private void OnStartWork()
		{
			if (diggable.worker != null)
			{
				Treasury.diggers[cachedCell] = diggable.worker;
				Log.Debug($"cached digger on cell {cachedCell} {diggable.worker.GetProperName()}");
			}

			temperature = Grid.Temperature[cachedCell];
		}

		private void OnStopWork()
		{
			if (diggable.isDigComplete)
			{
				Beached_Mod.Instance.treasury.TrySpawnTreasure(diggable, diggable.originalDigElement, diggable.worker);
				SpawnNeutroniumDust();
			}

			Treasury.diggers.Remove(cachedCell);
		}

		private void SpawnNeutroniumDust()
		{
			if (!Mod.integrations.IsModPresent(Integrations.ROCKETRY_EXPANDED))
				return;

			if (diggable.originalDigElement?.id == neutroniumDust)
			{
				var dust = ElementLoader.FindElementByHash(neutroniumDust);

				if (!isLoadingScene && dust != null && dust.substance != null)
				{
					dust.substance.SpawnResource(transform.position, mass * dustMultiplier, temperature, byte.MaxValue, 0);
				}
			}
		}

		public override void OnCleanUp()
		{
			Treasury.diggers.Remove(cachedCell);
		}
	}
}
