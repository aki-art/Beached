using Beached.Content.ModDb;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Beached.Patches
{
    public class AssetsPatch
    {
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Prefix(Assets __instance)
            {
                LoadSprites(__instance, Path.Combine(Mod.folder, "assets", "sprites"));

                var driedOutSprite = LoadSprite("status_item_dryedout", ModAssets.Sprites.STATUSITEM_DRIEDOUT);
                __instance.SpriteAssets.Add(driedOutSprite);

                Assets.RegisterOnAddPrefab(AcidVulnerableCreature.OnAddPrefab);
            }

            [HarmonyPostfix]
            [HarmonyPriority(Priority.Last)]
            public static void LatePostfix()
            {
                BDb.AddRecipes();
            }

            private static void LoadSprites(Assets __instance, string path)
            {
                if(!Directory.Exists(path))
                {
                    Log.Warning("Sprites directory not found.");
                    return;
                }

                Log.Info("Loading Sprite Assets...");

                foreach(var file in Directory.GetFiles(path, "*.png"))
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    var sprite = LoadSprite(file, name);
                    __instance.SpriteAssets.Add(sprite);

                    var metaPath = Path.Combine(Path.GetDirectoryName(path), name + ".meta.json");
                    if(File.Exists(metaPath))
                    {
                        var json = File.ReadAllText(metaPath);
                        if (json != null)
                        {
                            var data = JsonConvert.DeserializeObject<AssetMetaData>(json);
                            if(data.TintedSprite)
                            {
                                __instance.TintedSpriteAssets.Add(new TintedSprite()
                                {
                                    sprite = sprite,
                                    name = name,
                                    color = Util.ColorFromHex(data.ColorHex)
                                });
                            }
                        }
                    }
                }
            }

            public class AssetMetaData
            {
                public bool TintedSprite { get; set; }

                public string ColorHex { get; set; } = "FFFFFF";
            }

            private static Sprite LoadSprite(string path, string spriteName)
            {
                var texture = ModAssets.LoadTexture(path, true);

                if(texture == null)
                {
                    return null;
                }

                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
                sprite.name = spriteName;

                return sprite;
            }
        }
    }
}
