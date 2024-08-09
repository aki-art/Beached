using Beached.Content.Defs.Buildings;
using FUtility.FLocalization;
using HarmonyLib;

namespace Beached.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				Translations.RegisterForTranslation();

				Strings.Add("STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES", STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES);
				Strings.Add("STRINGS.DUPLICANTS.TRAITS.GILLS.SHORT_DESC", STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.SHORT_DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MiniFridgeConfig.ID.ToUpperInvariant()}.EFFECT", global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.EFFECT);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AmmoniaGeneratorConfig.ID.ToUpperInvariant()}.EFFECT", global::STRINGS.BUILDINGS.PREFABS.HYDROGENGENERATOR.EFFECT);
			}
		}
	}
}
