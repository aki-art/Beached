using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	internal class LimpetHost : BMonoBehavior
	{
		public static readonly HashedString targetSymbol = "beached_limpetgrowth";

		[MyCmpReq] private KBatchedAnimController kbac;
		[MyCmpReq] private SymbolOverrideController soc;

		[Serialize] public int growth;

		private KAnim.Build.Symbol[] symbols;

		public override void OnSpawn()
		{
			var anim = Assets.GetAnim("beached_pincher_limpetgrowth_kanim");
			symbols = new KAnim.Build.Symbol[4];
			for (int i = 0; i < symbols.Length; i++)
			{
				symbols[i] = anim.GetData().build.GetSymbol($"beached_limpetgrowth_{growth}");
			}

			base.OnSpawn();
			growth = Random.Range(0, 4);

			//kbac.SetSymbolVisiblity(targetSymbol, false);
			soc.AddSymbolOverride(targetSymbol, symbols[growth], 100);
		}
	}
}
