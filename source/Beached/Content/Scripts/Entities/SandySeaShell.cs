namespace Beached.Content.Scripts.Entities
{
	public class SandySeaShell : Uprootable
	{
		public static string splashFxID = EffectConfigs.AttackSplashId;
		public static string context = "dig";

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			multitoolHitEffectTag = splashFxID;
			multitoolContext = context;
		}
	}
}
