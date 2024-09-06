using Beached.Content.Defs.Foods;

namespace Beached.Content.Scripts
{
	public class MiniFridge : KMonoBehaviour
	{
		public override void OnPrefabInit()
		{
			DiscoveredResources.Instance.Discover(FieldRationConfig.ID, GameTags.Edible);
			DiscoveredResources.Instance.Discover(AstrobarConfig.ID, GameTags.Edible);
		}
	}
}
