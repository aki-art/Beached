using Beached.Content.Defs.Entities;

namespace Beached.Content.Scripts
{
    internal class ForceField : KMonoBehaviour, IImguiDebug
    {
        public ForceFieldVisualizer visualizer;

        public override void OnSpawn()
        {
            visualizer = MiscUtil.Spawn(ForceFieldConfig.ID, gameObject).AddOrGet<ForceFieldVisualizer>();
            visualizer.transform.SetParent(transform);
            visualizer.CreateMesh();
        }

        public void Initialize()
        {
        }

        public void OnImguiDraw()
        {
            visualizer.OnDebugSelected();
        }
    }
}
