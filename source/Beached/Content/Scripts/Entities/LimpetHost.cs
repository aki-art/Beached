namespace Beached.Content.Scripts.Entities
{
	internal class LimpetHost : BMonoBehavior
	{
		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private SymbolOverrideController soc;

		public override void OnSpawn()
		{
			base.OnSpawn();
			var anim = Assets.GetAnim("beached_pincher_limpetgrowth_kanim");
			var source_symbol = anim.GetData().build.GetSymbol("beached_limpetgrowth");
			soc.AddSymbolOverride("beached_limpetgrowth", source_symbol, 100);
		}
	}
}
