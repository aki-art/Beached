namespace Beached.Content.Scripts.Buildings
{
	public class FiltrationTileWorkable : Workable
	{
		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
			workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
			attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
			attributeExperienceMultiplier = TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

			workAnims = [
				"unclog_pre",
				"unclog_loop"
			];

			workingPstComplete =
			[
				"unclog_pst"
			];

			workingPstFailed =
			[
				"unclog_pst"
			];
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
		}
	}
}
