using Beached.Content.Defs.Foods;
using System;

namespace Beached.Integration
{
	public class AETE
	{
		public delegate void AddButcherableCookedDropLookupDelegate(Tag drop, Tag cookedMeat, float mass);
		public static AddButcherableCookedDropLookupDelegate AddButcherableCookedDropLookup;

		public static void Initialize()
		{
			var type = Type.GetType("Twitchery.ModAPI, Twitchery");
			if (type == null)
				return;

			var m_addButcherableCookedDropLookup = type.GetMethod("AddButcherableCookedDropLookup", [typeof(Tag), typeof(Tag), typeof(float)]);
			if (m_addButcherableCookedDropLookup != null)
			{
				AddButcherableCookedDropLookup = (AddButcherableCookedDropLookupDelegate)Delegate.CreateDelegate(typeof(AddButcherableCookedDropLookupDelegate), m_addButcherableCookedDropLookup);

				AddFoods();
			}
		}

		private static void AddFoods()
		{
			AddButcherableCookedDropLookup(CracklingsConfig.ID, SpicyCracklingsConfig.ID, 1f);
			AddButcherableCookedDropLookup(RawSnailConfig.ID, StuffedSnailsConfig.ID, 1f);

			foreach (var poff in PoffConfig.poffLookup)
			{
				AddButcherableCookedDropLookup(poff.Value.raw, poff.Value.cooked, 1f);
			}
		}
	}
}
