namespace Beached.Content.Scripts.Entities.Plant
{
	public class LeafletCoral : KMonoBehaviour
	{
		[MyCmpReq] private ReceptacleMonitor receptacleMonitor;
		public override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.PlanterStorage, OnReplanted);
			base.OnPrefabInit();
		}

		private void OnReplanted(object data = null)
		{
			if (!receptacleMonitor.Replanted)
				return;

			Tutorial.Instance.oxygenGenerators.Add(gameObject);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			if (!Tutorial.Instance.oxygenGenerators.Contains(gameObject))
				return;

			Tutorial.Instance.oxygenGenerators.Remove(gameObject);
		}
	}
}
