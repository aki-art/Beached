using Beached.Content;
using Beached.Content.Defs;
using Beached.Content.Defs.Entities;
using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Defs.Entities.SetPieces;
using Beached.Content.Defs.Equipment;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using Beached.Content.Defs.Items;
using Beached.Content.ModDb.Germs;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using static SandboxToolParameterMenu.SelectorValue;

namespace Beached.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		private static readonly HashSet<Tag> TAGS = new()
		{
			CrystalConfig.ID,

			// Geysers
			"GeyserGeneric_" + GeyserConfigs.AMMONIA_VENT,
			"GeyserGeneric_" + GeyserConfigs.MURKY_BRINE_GEYSER,
			"GeyserGeneric_" + GeyserConfigs.BISMUTH_VOLCANO,
			"GeyserGeneric_" + GeyserConfigs.CORAL_REEF,

			// Gems
			RareGemsConfig.FLAWLESS_DIAMOND,
			RareGemsConfig.HADEAN_ZIRCON,
			RareGemsConfig.MAXIXE,
			RareGemsConfig.STRANGE_MATTER,

			// Equipment
			MaxixePendantConfig.ID,
			RubberBootsConfig.ID,
			HematiteNecklaceConfig.ID,
			HadeanZirconAmuletConfig.ID,
			PearlNecklaceConfig.ID,
			ZeolitePendantConfig.ID,
			StrangeMatterAmuletConfig.ID,

			// Plants & Creatures
			CellAlgaeConfig.ID,
			GlowCapConfig.ID,
			LeafletCoralConfig.ID,
			MusselSproutConfig.ID,
			PoffShroomConfig.ID,
			WaterCupsConfig.ID,
			DewPalmConfig.ID,
			SlickShellConfig.ID,
			BabySlickShellConfig.ID,
			BambooConfig.ID,
			PurpleHangerConfig.ID,

			// setpieces
			SetPiecesConfig.TEST,
			SetPiecesConfig.BEACH,
			SetPiecesConfig.ZEOLITE,

			// genetic samples
			GeneticSamplesConfig.EVERLASTING,
			GeneticSamplesConfig.MEATY,

			// misc
			ForceFieldConfig.ID,
			SmokerConfig.ID,
			SandySeashellsConfig.SEASHELL,
			SandySeashellsConfig.SLICKSHELL,
			SeaShellConfig.ID,
			JellyfishStrobilaConfig.ID,
			PlanktonGerms.ID, /// <see cref="UIOnlyPlankton"/>
			BrinePoolConfig.ID,
		};

		private static readonly HashSet<Tag> FOOD = new()
		{
			AstrobarConfig.ID,
			SmokedFishConfig.ID,
			SmokedLiceConfig.ID,
			SmokedMeatConfig.ID,
			SmokedTofuConfig.ID,
			HighQualityMeatConfig.ID,
			LegendarySteakConfig.ID,
			AspicLiceConfig.ID,
			JellyConfig.ID,
			BerryJellyConfig.ID,
			MusselTongueConfig.ID,
			SpongeCakeConfig.ID,
			PipShootConfig.ID
		};

		private static readonly HashSet<Tag> FAUNA = new()
		{
			SlickShellConfig.ID,
			BabySlickShellConfig.ID,
			JellyfishStrobilaConfig.ID,
			JellyfishConfig.ID,
			BabyJellyfishConfig.ID
		};

		private static readonly HashSet<Tag> FLORA = new()
		{
			CellAlgaeConfig.ID,
			GlowCapConfig.ID,
			LeafletCoralConfig.ID,
			MusselSproutConfig.ID,
			PoffShroomConfig.ID,
			WaterCupsConfig.ID,
			DewPalmConfig.ID,
			BambooConfig.ID,
			PurpleHangerConfig.ID,
		};

		private static readonly HashSet<Tag> GEYSERS = new()
		{
			"GeyserGeneric_" + GeyserConfigs.AMMONIA_VENT,
			"GeyserGeneric_" + GeyserConfigs.MURKY_BRINE_GEYSER,
			"GeyserGeneric_" + GeyserConfigs.BISMUTH_VOLCANO,
			"GeyserGeneric_" + GeyserConfigs.CORAL_REEF,
		};

		private static readonly HashSet<Tag> GEMS = new()
		{
			RareGemsConfig.FLAWLESS_DIAMOND,
			RareGemsConfig.HADEAN_ZIRCON,
			RareGemsConfig.MAXIXE,
			RareGemsConfig.STRANGE_MATTER,
		};

		private static readonly HashSet<Tag> EQUIPMENTS = new()
		{

			MaxixePendantConfig.ID,
			RubberBootsConfig.ID,
			HematiteNecklaceConfig.ID,
			HadeanZirconAmuletConfig.ID,
			PearlNecklaceConfig.ID,
			ZeolitePendantConfig.ID,
			StrangeMatterAmuletConfig.ID,
		};

		[HarmonyPatch(typeof(SandboxToolParameterMenu), nameof(SandboxToolParameterMenu.ConfigureEntitySelector))]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				foreach (var poff in PoffConfig.configs)
				{
					FOOD.Add(poff.rawID);
					FOOD.Add(poff.cookedID);
				}

				var sprite = Def.GetUISprite(Assets.GetPrefab(SlickShellConfig.ID));
				var mods = SandboxUtil.AddModMenu(__instance, "Beached", sprite, _ => false);

				AddSubMenu(__instance, mods, "All", TAGS, JellyfishConfig.ID);
				AddSubMenu(__instance, mods, "Flora", FLORA, WaterCupsConfig.ID);
				AddSubMenu(__instance, mods, "Fauna", FAUNA, JellyfishConfig.ID);
				AddSubMenu(__instance, mods, "Geysers", GEYSERS, "GeyserGeneric_" + GeyserConfigs.BISMUTH_VOLCANO);
				AddSubMenu(__instance, mods, "Gems", GEMS, RareGemsConfig.HADEAN_ZIRCON);
				AddSubMenu(__instance, mods, "Equipment", EQUIPMENTS, RubberBootsConfig.ID);
				AddSubMenu(__instance, mods, "Set Pieces", BTags.setPiece, SetPiecesConfig.BEACH);
				AddSubMenu(__instance, mods, "Food", FOOD, AstrobarConfig.ID);
				AddSubMenu(__instance, mods, "Genetic Samples", BTags.geneticSample, GeneticSamplesConfig.FABULOUS);
				AddSubMenu(__instance, mods, "Amber Inclusions", BTags.amberInclusion, AmberInclusionsConfig.FLYING_CENTIPEDE);

				SandboxUtil.UpdateOptions(__instance);
			}

			private static void AddSubMenu(SandboxToolParameterMenu menu, SearchFilter mods, string name, Tag tag, string sprite)
			{
				var filter = new SearchFilter(
					name,
					prefab => prefab is KPrefabID id && id.HasTag(tag),
					mods,
					Def.GetUISprite(Assets.GetPrefab(sprite)));

				menu.entitySelector.filters = menu.entitySelector.filters.AddToArray(filter);
			}

			private static void AddSubMenu(SandboxToolParameterMenu menu, SearchFilter mods, string name, HashSet<Tag> set, string sprite)
			{
				var filter = new SearchFilter(
					name,
					prefab => prefab is KPrefabID id && set.Contains(id.PrefabTag),
					mods,
					Def.GetUISprite(Assets.GetPrefab(sprite)));

				menu.entitySelector.filters = menu.entitySelector.filters.AddToArray(filter);
			}

			public static void AddFilters(SandboxToolParameterMenu menu, params SearchFilter[] newFilters)
			{
				var filters = menu.entitySelector.filters;

				if (filters == null)
				{
					Log.Warning("Filters are null");
					return;
				}

				var f = new List<SearchFilter>(filters);
				f.AddRange(newFilters);
				menu.entitySelector.filters = f.ToArray();

				UpdateOptions(menu);
			}

			public static void UpdateOptions(SandboxToolParameterMenu menu)
			{
				var filters = menu.entitySelector.filters;

				if (filters == null)
				{
					return;
				}

				var options = ListPool<object, SandboxToolParameterMenu>.Allocate();

				foreach (var prefab in Assets.Prefabs)
				{
					foreach (var filter in filters)
					{
						if (filter.condition(prefab))
						{
							options.Add(prefab);
							break;
						}
					}
				}

				menu.entitySelector.options = options.ToArray();
				options.Recycle();
			}

			private static SearchFilter FindParent(SandboxToolParameterMenu menu, string parentFilterID)
			{
				return parentFilterID != null ? menu.entitySelector.filters.First(x => x.Name == parentFilterID) : null;
			}
		}
	}
}