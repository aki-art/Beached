using UnityEngine;

namespace Beached.Content.Defs.Items
{
    public class SeaShellConfig : IEntityConfig
    {
        public const string ID = "Beached_Seashell";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.MISC.BEACHED_SEASHELL.NAME,
                STRINGS.ITEMS.MISC.BEACHED_SEASHELL.DESC,
                1f,
                true,
                Assets.GetAnim("beached_seashell_kanim"),
                "object",
                Grid.SceneLayer.Creatures,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.6f,
                0.4f,
                true,
                0,
                SimHashes.Lime,
                new ()
                {
                    GameTags.Organics
                });

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
