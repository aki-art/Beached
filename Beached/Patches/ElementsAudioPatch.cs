using Beached.Content;
using HarmonyLib;

namespace Beached.Patches
{
    public class ElementsAudioPatch
    {
        [HarmonyPatch(typeof(ElementsAudio), "LoadData")]
        public class ElementsAudio_LoadData_Patch
        {
            public static void Postfix(ElementsAudio __instance, ref ElementsAudio.ElementAudioConfig[] ___elementAudioConfigs)
            {
                ___elementAudioConfigs = ___elementAudioConfigs.AddRangeToArray(Elements.CreateAudioConfigs(__instance));

                foreach (var item in ___elementAudioConfigs)
                {
                    Log.Debug("ELEMENT AUDIO CONFIGS");
                    Log.Debug("elementID " + item.elementID);
                    Log.Debug("ambienceType " + item.ambienceType);
                    Log.Debug("solidAmbienceType " + item.solidAmbienceType);
                    Log.Debug("miningSound " + item.miningSound);
                    Log.Debug("miningBreakSound " + item.miningBreakSound);
                    Log.Debug("oreBumpSound " + item.oreBumpSound);
                    Log.Debug("floorEventAudioCategory " + item.floorEventAudioCategory);
                    Log.Debug("creatureChewSound " + item.creatureChewSound);
                }
            }
        }
    }
}
