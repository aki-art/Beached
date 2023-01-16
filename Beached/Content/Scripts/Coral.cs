using UnityEngine;

namespace Beached.Content.Scripts
{
    public class Coral : StateMachineComponent<Coral.StatesInstance>
    {
        [MyCmpReq]
        private ElementConsumer elementConsumer;

        [SerializeField]
        public Tag emitTag;

        [SerializeField]
        public float emitMass;

        [SerializeField]
        public Vector3 emitOffset = Vector3.zero;

        [SerializeField]
        public Vector2 initialVelocity;

        public override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class States : GameStateMachine<States, StatesInstance, Coral>
        {
            public State alive;
            public State dead;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = alive;

                alive
                    .Update(UpdateEmission, UpdateRate.SIM_1000ms)
                    .EventHandler(GameHashes.OnStorageChange, OnStorageChanged)
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(true));
            }

            private void OnStorageChanged(StatesInstance smi)
            {
                smi.dirty = true;
            }

            private void UpdateEmission(StatesInstance smi, float dt)
            {
                if (smi.dirty)
                {
                    var emitMass = smi.master.emitMass;
                    var emitElement = smi.storage.FindFirst(smi.master.emitTag);

                    if (emitElement == null)
                    {
                        return;
                    }

                    var primaryElement = emitElement.GetComponent<PrimaryElement>();
                    if (primaryElement.Mass >= smi.master.emitMass)
                    {
                        primaryElement.Mass -= emitMass;
                        BubbleManager.instance.SpawnBubble(smi.transform.position, smi.master.initialVelocity, primaryElement.ElementID, emitMass, primaryElement.Temperature);
                        Game.Instance.SpawnFX(SpawnFXHashes.OxygenEmissionBubbles, smi.transform.position, 0);
                    }

                    smi.dirty = false;
                }
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, Coral, object>.GameInstance
        {
            public bool dirty;
            public Storage storage;

            public StatesInstance(Coral master) : base(master)
            {
                dirty = true;
                storage = master.GetComponent<Storage>();
            }
        }
    }
}
