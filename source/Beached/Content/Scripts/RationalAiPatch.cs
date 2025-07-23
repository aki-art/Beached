using HarmonyLib;

namespace Beached.Content.Scripts
{
	public class RationalAiPatch
	{
		[HarmonyPatch(typeof(RationalAi), "InitializeStates")]
		public class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive
					.ToggleStateMachine(smi => new Beached_ElectricShockable.Instance(smi.master, new Beached_ElectricShockable.Def()))
					.ToggleStateMachine(smi => new Beached_BiomeMonitor.Instance(smi.master))
					.ToggleStateMachine(smi => new Beached_ScaredMonitor.Instance(smi.master, new Beached_ScaredMonitor.Def()
					{
						lightTreshold = 30
					}));
			}
		}
	}
}
