using UnityEngine;

namespace Beached.Content.Defs
{
    // an entity used to store data in templates
    internal class TemplateProcessorConfig : IEntityConfig
    {
        public const string ID = "Beached_TemplateProcessor";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateBasicEntity(
                ID,
                "Template Processor",
                "You should never see mee.",
                1f,
                false,
                Assets.GetAnim("barbeque_kanim"),
                "object",
                Grid.SceneLayer.Front);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) {}
    }
}
