using Beached.Cmps;
using Beached.Content.Scripts;
using Beached.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS;

namespace Beached.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public class Game_OnSpawn_Patch
        {
            public static void Postfix(Game __instance)
            {
                var gameObject = __instance.gameObject;
                gameObject.AddOrGet<ElementInteractions>();
            }
        }

        [HarmonyPatch(typeof(Game), "InitializeFXSpawners")]
        public class Game_InitializeFXSpawners_Patch
        {
            public static void Prefix(ref MockStructs.SpawnPoolData[] ___fxSpawnData)
            {
                if (___fxSpawnData == null)
                {
                    Log.Warning("No spawn data");
                    return;
                }

                var spawnData = new List<MockStructs.SpawnPoolData>(___fxSpawnData);

                var prefab = spawnData.Find(d => d.id == SpawnFXHashes.OxygenEmissionBubbles).fxPrefab;

                if (prefab == null)
                {
                    Log.Warning("FX prefab not found.");
                    return;
                }

                var saltOffFx = new MockStructs.SpawnPoolData()
                {
                    id = ModAssets.Fx.saltOff,
                    initialCount = 4,
                    spawnOffset = Vector3.zero,
                    spawnRandomOffset = new Vector2(0.5f, 0.2f),
                    colour = Color.white,
                    fxPrefab = GetNewPrefab(prefab, "beached_offsalt_kanim", 2f),
                    initialAnim = "bubble"
                };

                ___fxSpawnData = spawnData.ToArray();
            }

            private static GameObject GetNewPrefab(GameObject original, string newAnim = null, float scale = 1f)
            {
                var prefab = UnityEngine.Object.Instantiate(original);
                var component = prefab.GetComponent<KBatchedAnimController>();

                if (!newAnim.IsNullOrWhiteSpace())
                {
                    component.AnimFiles[0] = Assets.GetAnim(newAnim);
                }

                component.animScale *= scale;

                return prefab;
            }
        }
    }
}
