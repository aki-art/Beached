using Beached.Content.Scripts.Entities;
using HarmonyLib;
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
                __instance.SpriteAssets.Add(LoadSprite("mod_mineralogy", ModAssets.Sprites.MOD_MINERALOGIST));
                __instance.SpriteAssets.Add(LoadSprite("icon_errand_mineralogy", ModAssets.Sprites.ERRAND_MINERALOGY));
                __instance.SpriteAssets.Add(LoadSprite("icon_archetype_mineralogy", ModAssets.Sprites.ARCHETYPE_MINERALOGY));
                __instance.SpriteAssets.Add(LoadSprite("icon_category_beachedpois", ModAssets.Sprites.BUILDCATEGORY_POIS));

                Assets.RegisterOnAddPrefab(AcidVulnerableCreature.OnAddPrefab);
            }

            private static Sprite LoadSprite(string fileName, string spriteName)
            {
                var path = Path.Combine(Mod.folder, "assets", "sprites", fileName + ".png");
                var texture = ModAssets.LoadTexture(path, true);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
                sprite.name = spriteName;

                return sprite;
            }
        }
    }
}
