using Beached.Content;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    public class TESTPATCHES
    {
        [HarmonyPatch(typeof(GroundRenderer), "ConfigureMaterialShine")]
        public class GroundRenderer_ConfigureMaterialShine_Patch
        {
            public static bool Prefix(Material material)
            {
                if(!material.HasProperty("_ShineMask"))
                {
                    return false;
                }

                return true;
            }
        }


        [HarmonyPatch(typeof(GeneratedOre), "LoadGeneratedOre")]
        public class GeneratedOre_LoadGeneratedOre_Patch
        {
            public static void Prefix()
            {
                var zeolite = Elements.Heulandite.Get();

                if (zeolite == null)
                {
                    Log.Warning("NULL ZEOLITE");
                    return;
                }

                if (!DlcManager.IsContentActive(zeolite.dlcId))
                {
                    Log.Warning("NO DLC");
                }

                if (zeolite.substance == null)
                {
                    Log.Warning("NULL ZEOLITE SUBSTANCE");
                    return;
                }


                if (zeolite.substance.anim == null)
                {
                    Log.Warning("NULL ZEOLITE ANIM");
                    return;
                }

            }

            public static void Postfix()
            {
                Log.Warning("heulandite item");
                var prefab = Assets.GetPrefab(Elements.Heulandite.Tag);

                if(prefab == null)
                {
                    Log.Warning("No heulandite item");
                }
            }
        }


        [HarmonyPatch(typeof(Substance), "SpawnResource")]
        public class Substance_SpawnResource_Patch
        {
            public static void Prefix(float mass, float temperature, Substance __instance, Tag ___nameTag)
            {
                Log.Debug($"spawning {___nameTag}, {mass}, {temperature}");
            }
        }
    }
}
