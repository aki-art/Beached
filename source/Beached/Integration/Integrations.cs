using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace Beached.Integration
{
	public class Integrations
	{
		public const string
			MOONLET = "Moonlet",
			CHEMICAL_PROCESSING = "Ronivan.ChemProcessing",
			CRITTER_TRAITS_REBORN = "CritterTraitsReborn",
			DECOR_PACK_I = "DecorPackA",
			FAST_TRACK = "PeterHan.FastTrack",
			ROCKETRY_EXPANDED = "Rocketry Expanded",
			PIP_MORPHS = "ONIPipMorphsELU";

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

			public class PipMorphs
			{
				public const string
					SQUIRREL_AUTUMN = "SquirrelAutumn",
					SQUIRREL_AUTUMN_BABY = "SquirrelAutumnBaby",
					SQUIRREL_AUTUMN_EGG = "SquirrelAutumnEgg",
					SQUIRREL_SPRING = "SquirrelSpring",
					SQUIRREL_SPRING_BABY = "SquirrelSpringBaby",
					SQUIRREL_SPRING_EGG = "SquirrelSpringEgg",
					SQUIRREL_WINTER = "SquirrelWinter",
					SQUIRREL_WINTER_BABY = "SquirrelWinterBaby",
					SQUIRREL_WINTER_EGG = "SquirrelWinterEgg";
			}

			public class VeryRealONIContentTheWikiSaysSo
			{
				public const string
					HATCHGOLD = "HatchGold",
					HATCHGOLDBABY = "HatchGoldBaby";
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
