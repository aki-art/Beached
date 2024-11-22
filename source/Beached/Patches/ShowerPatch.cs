using Beached.Content;
using Beached.Content.Scripts.Buildings;
using HarmonyLib;

namespace Beached.Patches
{
	public class ShowerPatch
	{
		[HarmonyPatch(typeof(Shower), "OnCompleteWork")]
		public class Shower_OnCompleteWork_Patch
		{
			public static void Postfix(Shower __instance, WorkerBase worker)
			{
				if (__instance.HasTag(BTags.soaped) && __instance.TryGetComponent(out Beached_SoapHolder soapy))
					soapy.OnCompleteShower(worker);
			}
		}
	}
}
