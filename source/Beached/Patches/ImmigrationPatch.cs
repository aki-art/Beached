using Beached.Content;
using Beached.Content.Defs.Entities;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Defs.Entities.Critters.Pufts;
using Beached.Content.Defs.Entities.Critters.Rotmongers;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Flora.Gnawica;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Beached.Patches
{
	public class ImmigrationPatch
	{
		[HarmonyPatch(typeof(Immigration), "ConfigureCarePackages")]
		public class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(Immigration __instance)
			{
				DisallowCoalUntilSteelAge(__instance);
			}

			private static void DisallowCoalUntilSteelAge(Immigration __instance)
			{
				HashSet<string> coalPackages =
					[
						SimHashes.Carbon.ToString(),
						HatchConfig.ID,
						BabyHatchConfig.ID,
						HatchConfig.EGG_ID,
					];

				foreach (var package in __instance.carePackages)
				{
					if (coalPackages.Contains(package.id))
						AddRequirement(package, CoalOnAstropalegosCondition);
				}

				void AddPackage(string tag, float amount = 1f, Func<bool> requirement = null, string facadeId = null)
				{
					requirement = (() => Beached_WorldLoader.Instance.IsBeachedContentActive) + requirement;
					__instance.carePackages.Add(new CarePackageInfo(tag, amount, requirement, facadeId));
				}

				// foods
				AddPackage(MusselTongueConfig.ID, 3);
				AddPackage(AspicLiceConfig.ID, 3);
				AddPackage(AstrobarConfig.ID, 3);
				AddPackage(GlazedDewnutConfig.ID, 3);
				AddPackage(SpaghettiConfig.ID, 3);

				// seeds
				AddPackage(LeafletCoralConfig.SEED_ID, 2);
				AddPackage(AlgaeCellConfig.SEED_ID, 2);
				AddPackage(WashuSpongeConfig.SEED_ID, 2);
				AddPackage(WaterCupsConfig.SEED_ID, 2);
				AddPackage(GnawicaPlantConfig.SEED_ID, 1, Discovered(Elements.bone) + AfterCycle(200));

				// babies
				AddPackage(BabyJellyfishConfig.ID);
				AddPackage(BabyMuffinConfig.ID, 1, Discovered(MuffinConfig.EGG_ID));
				AddPackage(BabyKaracooConfig.ID, 1, Discovered(Elements.moss));
				AddPackage(BabySlagmiteConfig.ID, 1, Discovered(Elements.slag));
				AddPackage(BabyAmmoniaPuftConfig.ID, 1, Discovered(Elements.ammonia));
				AddPackage(BabyRotmongerConfig.ID, 1, Discovered(Elements.rot));
				AddPackage(BabySlickShellConfig.ID, 1, Discovered(SimHashes.Salt));
				AddPackage(BabyMerpipConfig.ID, 1);

				// eggs
				AddPackage(JellyfishConfig.EGG_ID, 2);
				AddPackage(MuffinConfig.EGG_ID, 1, Discovered(MuffinConfig.EGG_ID));
				AddPackage(KaracooConfig.EGG_ID, 2, Discovered(Elements.moss));
				AddPackage(SlagmiteConfig.EGG_ID, 2, Discovered(Elements.slag));
				AddPackage(AmmoniaPuftConfig.EGG_ID, 2, Discovered(Elements.ammonia));
				AddPackage(RotmongerConfig.EGG_ID, 2, Discovered(Elements.rot));
				AddPackage(SlickShellConfig.EGG_ID, 2, Discovered(SimHashes.Salt));

				// equipment
				AddPackage(RubberBootsConfig.ID, 1, BeforeCycle(200));
				AddPackage(BeachShirtConfig.ID, 1, AfterCycle(40), BEquippableFacades.BEACHSHIRTS.BLUE);
				AddPackage(BeachShirtConfig.ID, 1, AfterCycle(40), BEquippableFacades.BEACHSHIRTS.GREEN);
				AddPackage(BeachShirtConfig.ID, 1, AfterCycle(40), BEquippableFacades.BEACHSHIRTS.BLACK);
				AddPackage(BeachShirtConfig.ID, 1, AfterCycle(40), BEquippableFacades.BEACHSHIRTS.RETRO);
				AddPackage(ZeolitePendantConfig.ID, 1, AfterCycle(40));
				AddPackage(HematiteNecklaceConfig.ID, 1, AfterCycle(40));
				AddPackage(PearlNecklaceConfig.ID, 1, AfterCycle(40));

				// gems
				AddPackage(RareGemsConfig.MAXIXE, 1, AfterCycle(300) + Discovered(Elements.aquamarine));
				AddPackage(RareGemsConfig.HADEAN_ZIRCON, 1, AfterCycle(300) + Discovered(Elements.zirconiumOre));
				AddPackage(RareGemsConfig.FLAWLESS_DIAMOND, 1, AfterCycle(300) + Discovered(SimHashes.Diamond));
				AddPackage(RareGemsConfig.STRANGE_MATTER, 1, AfterCycle(300) + Discovered(RareGemsConfig.STRANGE_MATTER));

				// soap
				AddPackage(SoapConfig.ID, 3, HasTech(ShowerConfig.ID));

				// elements
				AddPackage(Elements.zincOre.ToString(), 400f);
				AddPackage(Elements.bismuthOre.ToString(), 400f, AfterCycle(50));
				AddPackage(Elements.zirconiumOre.ToString(), 400f, AfterCycle(100));
				AddPackage(Elements.calcium.ToString(), 300f, AfterCycle(100));
				AddPackage(Elements.brass.ToString(), 300f, Discovered(Elements.brass));
				AddPackage(Elements.ambergris.ToString(), 300f, discoveredSpace);
				AddPackage(Elements.corallium.ToString(), 800f, AfterCycle(200));
				AddPackage(Elements.bone.ToString(), 200f, Discovered(Elements.bone));
				AddPackage(Elements.galena.ToString(), 400f);
				AddPackage(Elements.moss.ToString(), 200f, AfterCycle(50));
				AddPackage(Elements.mucus.ToString(), 1000f);
				AddPackage(Elements.rubber.ToString(), 300f, Discovered(Elements.rubber));
				AddPackage(Elements.slag.ToString(), 300f, Discovered(Elements.slag));
				AddPackage(Elements.slagGlass.ToString(), 600f, Discovered(Elements.slag));
				AddPackage(Elements.zeolite.ToString(), 800f);

				// clusters
				AddPackage(CrystalConfig.GetClusterId(CrystalConfig.ZEOLITE), 1, AfterCycle(200));
			}

			private static readonly Func<bool> discoveredSpace = () => Game.Instance.unlocks.IsUnlocked("surfacebreach");

			private static Func<bool> HasTech(string techItemId) => () => Db.Get().Techs.IsTechItemComplete(techItemId);

			private static Func<bool> Discovered(string id) => () => DiscoveredResources.Instance.IsDiscovered(id);
			private static Func<bool> Discovered(SimHashes id) => () => DiscoveredResources.Instance.IsDiscovered(id.CreateTag().ToString());

			private static Func<bool> AfterCycle(int cycleCount) => () => GameClock.Instance.GetTimeInCycles() >= cycleCount;

			private static Func<bool> BeforeCycle(int cycleCount) => () => GameClock.Instance.GetTimeInCycles() < cycleCount;

			private static void AddRequirement(CarePackageInfo package, Func<bool> condition)
			{
				var fieldInfo = typeof(CarePackageInfo).GetField("requirement");
				var existingRequirement = fieldInfo.GetValue(package) as Func<bool>;

				fieldInfo.SetValue(package, existingRequirement + condition);
			}

			private static bool CoalOnAstropalegosCondition()
			{
				return !Beached_WorldLoader.Instance.IsBeachedContentActive
					|| DiscoveredResources.Instance.IsDiscovered(SimHashes.Steel.CreateTag());
			}
		}
	}
}
