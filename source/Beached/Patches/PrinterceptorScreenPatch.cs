using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine.UI;

namespace Beached.Patches
{
	internal class PrinterceptorScreenPatch
	{

		[HarmonyPatch(typeof(PrinterceptorScreen), "SpawnOptionButton")]
		public class PrinterceptorScreen_SpawnOptionButton_Patch
		{
			public static void Postfix(PrinterceptorScreen __instance, Tag id)
			{
				if (Beached_WorldLoader.Instance.PrinterceptorDisabled(id))
				{
					if (__instance.optionButtons.TryGetValue(id, out var button))
					{
						if (button.TryGetComponent(out HierarchyReferences refs))
						{
							if (refs.TryGetReference<Image>("FGIcon", out var icon))
								icon.sprite = Assets.GetSprite("beached_glitched_egg_1");
						}
					}
				}
			}
		}

		[HarmonyPatch(typeof(PrinterceptorScreen), "SelectEntity")]
		public class PrinterceptorScreen_SelectEntity_Patch
		{
			public static void Postfix(PrinterceptorScreen __instance, Tag id)
			{
				if (Beached_WorldLoader.Instance.PrinterceptorDisabled(id))
				{
					// TODO: STRING
					__instance.selectedEffectsText.text = "<smallcaps><color=#b83846>C<b>ORRUPTED</color></b> D<b><i>A</i></b>TA▯▯▯. Cann▯t <size=140%>pri</size>nt this this this item▯▯▯▯▯.</smallcaps>\n\n" +
						"<i>This data has been compromised. Finding a Hatch by other means first should allow my Duplicants to restore the missing information.</i>";

				}
			}
		}
	}
}
