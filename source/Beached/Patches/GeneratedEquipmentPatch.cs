using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
	public class GeneratedEquipmentPatch
	{
		[HarmonyPatch(typeof(GeneratedEquipment), nameof(GeneratedEquipment.LoadGeneratedEquipment))]
		public class GeneratedEquipment_LoadGeneratedEquipment_Patch
		{
			public static void Postfix()
			{
				Assets.GetPrefab(SleepClinicPajamas.ID).AddTag(BTags.comfortableClothing);
			}
		}
	}
}
