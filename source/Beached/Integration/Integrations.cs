using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace Beached.Integration
{
	public class Integrations
	{
		public const string
			CHEMICAL_PROCESSING = "Ronivan.ChemProcessing",
			CRITTER_TRAITS_REBORN = "CritterTraitsReborn",
			DECOR_PACK_I = "DecorPackA",
			FAST_TRACK = "PeterHan.FastTrack",
			ROCKETRY_EXPANDED = "Rocketry Expanded";

		public class IDS
		{
			public class RollerSnakes
			{
				public const string
					ROLLERSNAKESTEEL = "RollerSnakeSteel",
					ROLLERSNAKESTEELBABY = "RollerSnakeSteelBaby",
					ROLLERSNAKE = "RollerSnake",
					ROLLERSNAKEBABY = "RollerSnakeBaby";
			}
		}
		private HashSet<string> modsPresent;

		public bool IsModPresent(string modId)
		{
			if (modsPresent == null)
			{
				Log.Warning("Cannot check if mod is present, mods not loaded yet");
				return false;
			}

			return modsPresent.Contains(modId);
		}

		public void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
		{
			modsPresent = mods
				.Where(mod => mod.IsEnabledForActiveDlc())
				.Select(mod => mod.staticID)
				.ToHashSet();

			if (IsModPresent(CRITTER_TRAITS_REBORN))
			{
				CritterTraitsReborn.Initialize();
				Patches.SimpleInfoScreenPatch.Patch(harmony);
			}

			CritterShedding.Initialize();
		}
	}
}
