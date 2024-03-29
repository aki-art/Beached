﻿using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class GasBreatherFromWorldProviderPatch
	{
#if ELEMENTS
		[HarmonyPatch(typeof(GasBreatherFromWorldProvider), nameof(GasBreatherFromWorldProvider.OnSimConsume))]
		public class GasBreatherFromWorldProvider_OnSimConsume_Patch
		{
			// Trigger events for atmospheres
			// liquids are included for Minnow's waterbreathing ability
			public static void Postfix(Sim.MassConsumedCallback mass_cb_info, OxygenBreather ___oxygenBreather)
			{
				var id = ElementLoader.elements[mass_cb_info.elemIdx].id;

				if (id == Elements.saltyOxygen || id == SimHashes.SaltWater)
					___oxygenBreather.Trigger((int)ModHashes.greatAirQuality, mass_cb_info);
				else if (id == SimHashes.DirtyWater || id == Elements.murkyBrine)
					___oxygenBreather.Trigger((int)GameHashes.PoorAirQuality, mass_cb_info);
			}
		}
#endif
	}
}
