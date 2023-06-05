using Beached.Content;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Beached.Integration
{
    public class DecorPackI
    {
        public delegate void AddTileDelegate(
            string elementID,
            Color specularColor,
            bool isSolid,
            string[] dlcIDs,
            Texture2D main,
            Texture2D tops,
            Texture2D place,
            Texture2D spec);

        public static void RegisterTiles()
        {
#if ELEMENTS
            var decorPackAPIType = Type.GetType("DecorPackA.ModAPI, DecorPackA");

            if (decorPackAPIType == null) return;

            var addTileMethod = decorPackAPIType.GetMethod("AddTile", BindingFlags.Static | BindingFlags.Public);
            var addTile = (AddTileDelegate)Delegate.CreateDelegate(typeof(AddTileDelegate), addTileMethod);

            AddTile(addTile, Elements.aquamarine.ToString(), new Color(0, 1, 2));
            AddTile(addTile, Elements.mucus.ToString(), Color.white, false);
            AddTile(addTile, Elements.bismuth.ToString(), Color.red);
            AddTile(addTile, Elements.siltStone.ToString(), Color.white);
#endif
        }

        private static void AddTile(AddTileDelegate addTile, string id, Color specColor, bool isSolid = true)
        {
            var path = Path.Combine(Mod.folder, "assets", "decorpacki", "tiles");
            var lowerId = id.ToLowerInvariant();
            var main = ModAssets.LoadTexture(Path.Combine(path, $"{lowerId}_glass_tiles.png"));
            var tops = ModAssets.LoadTexture(Path.Combine(path, $"{lowerId}_glass_tiles_tops.png"));
            var spec = ModAssets.LoadTexture(Path.Combine(path, $"{lowerId}_glass_tiles_spec.png"));
            var place = ModAssets.LoadTexture(Path.Combine(path, $"{lowerId}_glass_tiles_place.png"));

            addTile(id, specColor, isSolid, null, main, tops, place, spec);
        }
    }
}
