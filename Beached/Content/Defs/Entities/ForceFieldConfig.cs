using Beached.Content.Scripts.Buildings;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
    internal class ForceFieldConfig : IEntityConfig
    {
        public const string ID = "Beached_ForceField";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                "Force Field",
                "",
                1f,
                Assets.GetAnim("barbeque_kanim"),
                "object",
                Grid.SceneLayer.Front,
                1,
                1,
                TUNING.DECOR.NONE);


            prefab.AddComponent<ForceFieldVisualizer>();

            return prefab;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
    }
}
