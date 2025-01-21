using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class KaracooSkin(string id) : Resource(id)
	{
		private readonly List<Color> colors = [];
		private readonly List<List<KAnimHashedString>> symbols = [];

		public KaracooSkin AddLayer(Color color, List<KAnimHashedString> symbols)
		{
			colors.Add(color);
			this.symbols.Add(symbols);

			return this;
		}

		public static KaracooSkin CreateBasicSkin(string id, string primary, string secondary)
		{
			return CreateBasicSkin(id, Util.ColorFromHex(primary), Util.ColorFromHex(secondary));
		}

		public static KaracooSkin CreateBasicSkin(string id, Color primary, Color secondary)
		{
			return new KaracooSkin(id)
				.AddLayer(primary, ["body", "tails"])
				.AddLayer(secondary, ["tint"]);
		}

		public void Apply(KBatchedAnimController kbac)
		{
			for (int i = 0; i < colors.Count; i++)
			{
				foreach (var symbol in symbols[i])
					kbac.SetSymbolTint(symbol, colors[i]);
			}

			kbac.UpdateAnim(0);
		}
	}
}
