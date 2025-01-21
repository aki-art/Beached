using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
	public class GamePatch
	{
		[HarmonyPatch(typeof(Game), nameof(Game.InitializeFXSpawners))]
		public class Game_InitializeFXSpawners_Patch
		{
			public static void Prefix(Game __instance, ref Game.SpawnPoolData[] ___fxSpawnData)
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
					spawnOffset = new Vector2(0f, 0.5f),
					spawnRandomOffset = new Vector2(0f, 0f),
					colour = Util.ColorFromHex("314f22"),
					fxPrefab = GetNewPrefab(prefab, "beached_mossplosion_kanim", 0.45f),
					initialAnim = "splode"
				});

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.mossplosionRed,
					initialCount = 4,
					spawnOffset = new Vector2(0f, 0.5f),
					spawnRandomOffset = new Vector2(0f, 0f),
					colour = Util.ColorFromHex("ba452b"),
					fxPrefab = GetNewPrefab(prefab, "beached_mossplosion_kanim", 0.45f),
					initialAnim = "splode"
				});

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.ammoniaBubbles,
					initialCount = 4,
					spawnOffset = Vector3.zero,
					spawnRandomOffset = new Vector2(0f, 0f),
					colour = Color.white,
					fxPrefab = GetNewPrefab(prefab, "beached_ammonia_bubbles_kanim", 1f),
					initialAnim = "bubble"
				});

				___fxSpawnData = [.. spawnData];

			}
			/*
						public static void ___Postfix(Game __instance)
						{
							Log.Debug("spawning");
							var game = __instance;
							var fx_idx = game.fxSpawner.Count;
							var fx_mask = (ushort)(1 << fx_idx);

							void destroyer(SpawnFXHashes fxid, GameObject go)
							{
								Log.Debug("destroyer");
								if (Game.IsQuitting())
									return;

								var mask = (int)game.activeFX[Grid.PosToCell(go)];
								mask = (ushort)(mask &= ~fx_mask);
								game.activeFX[Grid.PosToCell(go)] = (ushort)mask;

								if (go.TryGetComponent(out ParticleSystemPlayer player))
									player.Hide();

								game.fxPools[(int)fxid].ReleaseInstance(go);
							}

							GameObject instantiator()
							{
								Log.Debug("instantiator");
								GameObject gameObject = GameUtil.KInstantiate(ModAssets.Fx.ParticleFxSet.ammoniaBubblesUp, Grid.SceneLayer.Front);
								Log.Debug("instantiated");
								var player = gameObject.GetComponent<ParticleSystemPlayer>();
								player.enabled = false;
								Log.Debug("ParticleSystemPlayer disabled");
								gameObject.SetActive(true);
								Log.Debug("ParticleSystemPlayer SetActive");
								player.OnDie += go => destroyer(ModAssets.Fx.ammoniaBubbles, go);
								Log.Debug("ParticleSystemPlayer OnDie");

								return gameObject;
							}

							var pool = new GameObjectPool(instantiator, 4);
							game.fxPools[(int)ModAssets.Fx.ammoniaBubbles] = pool;

							Log.Debug("created pool");
							__instance.fxSpawner[(int)ModAssets.Fx.ammoniaBubbles] = (pos, rotation) =>
							{
								var cell = Grid.PosToCell(pos);
								if ((__instance.activeFX[cell] & fx_mask) != 0)
									return;

								game.activeFX[cell] |= fx_mask;
								var instance = pool.GetInstance();
								*//*
													int index3 = (int)((double)rotation / 90.0);
													if (index3 < 0)
														index3 += spawnPoolData.rotationData.Length;*//*

								instance.transform.SetPosition(pos);

								if (instance.TryGetComponent(out ParticleSystemPlayer player))
								{
									player.enabled = true;
									player.Play();
								}
							};

							Log.Debug("created prefab");
						}*/
		}

		private static GameObject GetNewPrefab(GameObject original, string newAnim = null, float scale = 1f)
		{
			var prefab = UnityEngine.Object.Instantiate(original);
			var component = prefab.GetComponent<KBatchedAnimController>();

			if (!newAnim.IsNullOrWhiteSpace())
				component.AnimFiles[0] = Assets.GetAnim(newAnim);

			component.animScale *= scale;

			return prefab;
		}
	}
}

