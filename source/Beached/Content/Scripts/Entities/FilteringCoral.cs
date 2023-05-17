using System;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
    public class FilteringCoral : StateMachineComponent<FilteringCoral.StatesInstance>
    {
        [MyCmpReq]
        private ElementConsumer elementConsumer;

        [SerializeField]
        public Tag gunkTag;

        [SerializeField]
        public float gunkLimit;

        [SerializeField]
        public float gunkClearTimeSeconds;

        [SerializeField]
        public float emitMass;

        [SerializeField]
        public Vector3 emitOffset = Vector3.zero;

        [SerializeField]
        public Vector2 initialVelocity;

        public FilteringCoral()
        {
            gunkTag = GameTags.Solid;
            gunkLimit = 10f;
            gunkClearTimeSeconds = 30f;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class States : GameStateMachine<States, StatesInstance, FilteringCoral>
        {
            public State alive;
            public State gunked;
            public State dead;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = alive;

                alive
                    .EventHandlerTransition(GameHashes.OnStorageChange, gunked, IsGunked)
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(true));

                gunked
                    .EventHandlerTransition(GameHashes.OnStorageChange, alive, (smi, data) => !IsGunked(smi, data))
                    .ScheduleAction("clear gunk", GetGunkClearTime, ClearGunk)
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(false));

            }

            private float GetGunkClearTime(StatesInstance smi) => smi.master.gunkClearTimeSeconds;

            private void ClearGunk(StatesInstance smi) => smi.storage.Drop(smi.master.gunkTag);

            private bool IsGunked(StatesInstance smi, object data)
            {
                return smi.storage.GetMassAvailable(smi.master.gunkTag) > smi.master.gunkLimit;
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, FilteringCoral, object>.GameInstance
        {
            public Storage storage;

            public StatesInstance(FilteringCoral master) : base(master)
            {
                storage = master.GetComponent<Storage>();
            }
        }
    }
}
