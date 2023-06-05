namespace Beached.Content.Scripts
{
	// companion to Diggable, assisting Mineralogy leveled dupes to gain extra treasures when digging
	public class TreasureHolder : KMonoBehaviour
	{
		[MyCmpGet] public Diggable diggable;
		private float mass;

		public override void OnSpawn()
		{
			diggable.OnWorkableEventCB += OnDiggableEvent;
			mass = Grid.Mass[Grid.PosToCell(this)];

			// needs to check once because the resumed work from a previous game
			// session wont fire the workable event after this components OnSpawn
			if (diggable.worker != null)
			{
				OnStartWork();
			}
		}

		private void OnDiggableEvent(Workable workable, Workable.WorkableEvent evt)
		{
			switch (evt)
			{
				case Workable.WorkableEvent.WorkStarted:
					OnStartWork();
					break;
				case Workable.WorkableEvent.WorkCompleted:
					Log.Debug("work complete");
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
				Treasury.diggers[diggable.GetCell()] = diggable.worker;
			}
		}

		private void OnStopWork()
		{
			if (diggable.isDigComplete)
			{
				Beached_Mod.Instance.treasury.TrySpawnTreasure(diggable, diggable.originalDigElement, diggable.worker);
			}

			Treasury.diggers.Remove(diggable.GetCell());
		}

		public override void OnCleanUp()
		{
			Treasury.diggers.Remove(Grid.PosToCell(this));
		}
	}
}
