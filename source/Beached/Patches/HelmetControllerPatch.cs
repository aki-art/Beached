using Beached.Content.ModDb.Sicknesses;
using HarmonyLib;
using Klei.AI;

namespace Beached.Patches
{
	public class HelmetControllerPatch
	{
		[HarmonyPatch(typeof(HelmetController), nameof(HelmetController.OnPrefabInit))]
		public class HelmetController_OnPrefabInit_Patch
		{
			public static void Postfix(HelmetController __instance)
			{
				__instance.GetComponent<KPrefabID>().prefabInitFn += go =>
				{
					if (go.TryGetComponent(out Assignable assignable))
					{
						assignable.AddAssignPrecondition(IsNotCapped);
						assignable.AddAutoassignPrecondition(IsNotCapped);
					}
				};
			}

			private static bool IsNotCapped(MinionAssignablesProxy proxy)
			{
				var targetGo = proxy.GetTargetGameObject();
				return targetGo != null && !targetGo.GetSicknesses().Has(BSicknesses.capped);
			}
		}
	}
}
