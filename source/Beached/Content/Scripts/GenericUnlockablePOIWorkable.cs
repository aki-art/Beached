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
			overrideAnims =
			[
				Assets.GetAnim( "anim_interacts_research_unlock_kanim")
			];

			synchronizeAnims = true;
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
