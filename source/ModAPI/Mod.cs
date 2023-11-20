using HarmonyLib;
using KMod;

namespace Beached_ModAPI
{
	public class Mod : UserMod2
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				Debug.Log("Loading API...");

#if DEBUG
				var logging = true;
#else
				var logging = false;
#endif

				if (Beached_API.TryInitialize(logging))
				{
					// use API

					var traits = Beached_API.GetPossibleLifegoalTraits(null, true);

					Debug.Log("Succesfully loaded Beached API. Life goal traits:");

					foreach (var trait in traits)
					{
						Debug.Log($"- {trait}");
					}
				}
			}
		}
	}
}
