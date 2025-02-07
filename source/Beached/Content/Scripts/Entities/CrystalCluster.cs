namespace Beached.Content.Scripts.Entities
{
	public class CrystalCluster : KMonoBehaviour, IReceptacleDirection
	{
		public Tag crystalId;
		public Tag previewID;

		public SingleEntityReceptacle.ReceptacleDirection direction;

		public SingleEntityReceptacle.ReceptacleDirection Direction => direction;
	}
}
