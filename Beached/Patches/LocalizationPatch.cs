using HarmonyLib;
using KMod;
using System;
using System.IO;

namespace Beached.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Translate(typeof(STRINGS));

                Strings.Add("STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES", STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES);
                Strings.Add("STRINGS.DUPLICANTS.TRAITS.GILLS.SHORT_DESC", STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.SHORT_DESC);
            }

            public static void Translate(Type root)
            {
                Localization.RegisterForTranslation(root);
                LoadStrings();
                LocString.CreateLocStringKeys(root, null);
                Localization.GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
            }

            // Load user created translations
            private static void LoadStrings()
            {
                string code = Localization.GetLocale()?.Code;

                if (code.IsNullOrWhiteSpace())
                {
                    return;
                }

                string path = Path.Combine(Mod.folder, "translations", code + ".po");

                if (File.Exists(path))
                {
                    Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
                    Log.Info($"Found translation file for {code}.");
                }
            }
        }
    }
}
