using UnityEngine;

namespace Beached.Content.Scripts
{
    public class SetPiece : KMonoBehaviour
    {
        public GameObject visualizer;

        [SerializeField]
        public string setPiecePrefabID;

        public override void OnSpawn()
        {
            base.OnSpawn();

            if(ModAssets.Prefabs.setpieces.TryGetValue(setPiecePrefabID, out var prefab))
            {
                visualizer = Instantiate(prefab);

                var position = new Vector3(transform.position.x, transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Backwall) - 0.1f);
                visualizer.transform.position = position;
                visualizer.transform.SetParent(transform);
                visualizer.SetActive(true);
            }
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();

            if(visualizer != null)
            {
                Object.Destroy(visualizer);
            }
        }
    }
}
