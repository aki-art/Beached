namespace Beached.Content.Scripts.Entities
{
	public class HarvestableSandySeashell : Harvestable
	{
		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			multitoolContext = SandySeaShell.context;
			multitoolHitEffectTag = SandySeaShell.splashFxID;
		}
	}
}
