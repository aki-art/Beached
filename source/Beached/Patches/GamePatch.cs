using Beached.Utils;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "InitializeFXSpawners")]
        public class Game_InitializeFXSpawners_Patch
        {
            public static void Prefix(ref Game.SpawnPoolData[] ___fxSpawnData)
            {
                if (___fxSpawnData == null)
                {
                    Log.Warning("No spawn data");
                    return;
                }

                var spawnData = new List<Game.SpawnPoolData>(___fxSpawnData);

                var prefab = spawnData.Find(d => d.id == SpawnFXHashes.OxygenEmissionBubbles).fxPrefab;

                if (prefab == null)
                {
                    Log.Warning("FX prefab not found.");
                    return;
                }

                spawnData.Add(new Game.SpawnPoolData()
                {
                    id = ModAssets.Fx.saltOff,
                    initialCount = 4,
                    spawnOffset = Vector3.zero,
                    spawnRandomOffset = new Vector2(0.5f, 0.2f),
                    colour = Color.white,
                    fxPrefab = GetNewPrefab(prefab, "beached_offsalt_kanim", 2f),
                    initialAnim = "bubble"
                });

                spawnData.Add(new Game.SpawnPoolData()
                {
                    id = ModAssets.Fx.grimcapPoff,
                    initialCount = 4,
                    spawnOffset = Vector3.zero,
                    spawnRandomOffset = new Vector2(0.5f, 0.2f),
                    colour = Color.white,
                    fxPrefab = GetNewPrefab(prefab, "beached_grimcap_poff_kanim", 1f),
                    initialAnim = "poff"
                });

                spawnData.Add(new Game.SpawnPoolData()
                {
                    id = ModAssets.Fx.mossplosion,
                    initialCount = 4,
                    spawnOffset = Vector3.zero,
                    spawnRandomOffset = new Vector2(0f, 0f),
                    colour = Color.white,
                    fxPrefab = GetNewPrefab(prefab, "beached_mossplosion_kanim", 0.45f),
                    initialAnim = "splode"
                });

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
