using HarmonyLib;
using System;
using System.IO;

namespace Beached.Integration
{
	public class TrueTiles
	{
		public static void OnAllModsLoaded()
		{
			var t_Mod = Type.GetType("TrueTiles.Mod, TrueTiles");
			if (t_Mod != null)
			{
				var path = Path.Combine(Mod.folder, "assets", "truetiles");

				AccessTools
					.DeclaredMethod(t_Mod, "AddPack")
					.Invoke(null, new[] { path });
			}
		}
	}
}
