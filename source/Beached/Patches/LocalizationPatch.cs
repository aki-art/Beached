using Beached.Content.Defs.Buildings;
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
				//Translations.RegisterForTranslation(typeof(STRINGS));

				Log.Debug("CLASS NAME: " + typeof(STRINGS).AssemblyQualifiedName);
				Strings.Add("STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES", STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES);
				Strings.Add("STRINGS.DUPLICANTS.TRAITS.GILLS.SHORT_DESC", STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.SHORT_DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MiniFridgeConfig.ID.ToUpperInvariant()}.EFFECT", global::STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.EFFECT);


				Strings.Add($"STRINGS.MISC.TAGS.BEACHED_MOSS", Strings.Get("STRINGS.ELEMENTS.BEACHED_MOSS.NAME"));
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.BEACHED_DECONSTRUCTABLEROCKETTILE.DESC", global::STRINGS.BUILDINGS.PREFABS.ROCKETWALLTILE.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.BEACHED_DECONSTRUCTABLEROCKETTILE.EFFECT", global::STRINGS.BUILDINGS.PREFABS.ROCKETWALLTILE.EFFECT);

				Log.Debug($"Mini fridge string: {Strings.Get("STRINGS.BUILDINGS.PREFABS.BEACHED_MINIFRIDGE.NAME")}");
			}
		}
	}
}
