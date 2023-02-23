using Beached.Content.Defs.Entities;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class ForceField : StateMachineComponent<ForceField.StatesInstance>, IImguiDebug
    {
        [SerializeField] public Vector3 offset;

        public ForceFieldVisualizer visualizer;

        public override void OnSpawn()
        {
            visualizer = MiscUtil.Spawn(ForceFieldConfig.ID, transform.position + offset).AddOrGet<ForceFieldVisualizer>();
            visualizer.transform.SetParent(transform);
            visualizer.CreateMesh();

            smi.StartSM();
        }

        public void OnImguiDraw()
        {
            visualizer.OnDebugSelected();
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, ForceField, object>.GameInstance
        {
            public StatesInstance(ForceField master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, ForceField>
        {
            public State off;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                off
                    .PlayAnim("off", KAnim.PlayMode.Loop);
            }
        }
    }
}
