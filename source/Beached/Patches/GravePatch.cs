using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	internal class GravePatch
	{

		[HarmonyPatch(typeof(Grave), "OnStorageChanged")]
		public class Grave_OnStorageChanged_Patch
		{
			public static void Prefix(Grave __instance, object data)
			{
				if (data is GameObject go
					&& go.TryGetComponent(out MinionIdentity identity)
					&& __instance.TryGetComponent(out Beached_Grave bGrave))
				{
					bGrave.SetPersonality(identity.personalityResourceId);

					Log.Debug("buried + " + HashCache.Get().Get(identity.personalityResourceId));
				}
			}
		}
	}
}
