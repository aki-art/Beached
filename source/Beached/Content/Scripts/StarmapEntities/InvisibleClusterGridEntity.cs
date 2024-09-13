using System.Collections.Generic;

namespace Beached.Content.Scripts.StarmapEntities
{
	/// <summary>
	/// used by the meteor swarm poi entities
	/// </summary>
	public class InvisibleClusterGridEntity : ClusterGridEntity
	{
		public string _name;

		public override string Name => _name;

		public override EntityLayer Layer => EntityLayer.FX;

		public override List<AnimConfig> AnimConfigs =>
		[
			new() //filler
			{
				animFile = Assets.GetAnim( "temporal_tear_kanim"),
				initialAnim = "open_loop"
			}
		];

		public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
		{
			base.OnClusterMapIconShown(levelUsed);
		}

		public override bool IsVisible => true; // testing, set to false for finished product

		public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Hidden;
	}
}
