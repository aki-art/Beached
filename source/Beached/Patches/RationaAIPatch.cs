using Beached.Content.Scripts.Entities;
using HarmonyLib;

namespace Beached.Patches
{
	public class RationaAIPatch
	{
		// Extend duplicant AI
		[HarmonyPatch(typeof(RationalAi), nameof(RationalAi.InitializeStates))]
		public class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive
					.ToggleStateMachine(smi => new FreshAirMonitor.Instance(smi.master));
				//.ToggleStateMachine(telepadInstance => new ScaredMonitor.Instance(telepadInstance.master));
			}
		}
	}
}
