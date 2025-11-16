using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class HijackedHeadquartersConfigPatch
	{
		[HarmonyPatch(typeof(HijackedHeadquartersConfig), "GetDataBankCost")]
		public class HijackedHeadquartersConfig_GetDataBankCost_Patch
		{
			public static void Postfix(Tag printableTag, ref int __result)
			{
				if (Beached_WorldLoader.Instance.PrinterceptorDisabled(printableTag))
				{
					__result = int.MaxValue;
				}
			}
		}
	}
}
