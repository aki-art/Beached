using Beached.Content.Scripts;
using HarmonyLib;

namespace Beached.Patches
{
	public class IrrigationMonitorPatch
	{
		[HarmonyPatch(typeof(IrrigationMonitor), "InitializeStates")]
		public class IrrigationMonitor_InitializeStates_Patch
		{
			public static void Postfix(IrrigationMonitor __instance)
			{
				__instance.replanted
					// the underlying collection of actions is a list. Enter always adds
					// to the end of this list, so this will run after the original Enter
					.Enter(smi =>
					{
						if (smi.gameObject.TryGetComponent(out SelfIrrigator _))
						{
							foreach (var manualDelivery in smi.gameObject.GetComponents<ManualDeliveryKG>())
							{
								manualDelivery.Pause(true, "self irrigator");
								manualDelivery.enabled = false;
							}
						}
					});
			}
		}


		[HarmonyPatch(typeof(IrrigationMonitor.Instance), "SetStorage")]
		public class IrrigationMonitor_Instance_SetStorage_Patch
		{
			public static void Postfix(IrrigationMonitor.Instance __instance, object obj)
			{
				if (__instance.master.gameObject.TryGetComponent(out SelfIrrigator selfIrrigator))
					selfIrrigator.SetStorage(obj as Storage);
			}
		}
	}
}
