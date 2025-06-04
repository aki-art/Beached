namespace Beached.Content.Scripts
{
	public class GenericUnlockablePOIWorkable : Workable
	{
		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
			alwaysShowProgressBar = true;
			resetProgressOnStop = false;
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);

			var smi = this.GetSMI<GenericUnlockablePOI.Instance>();
			smi.UnlockTechItems();
			smi.sm.pendingChore.Set(false, smi);

			gameObject.Trigger((int)GameHashes.UIRefresh);
			Prioritizable.RemoveRef(gameObject);
		}
	}
}
