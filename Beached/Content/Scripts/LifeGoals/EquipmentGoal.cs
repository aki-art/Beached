namespace Beached.Content.Scripts.LifeGoals
{
    public class EquipmentGoal : StateMachineComponent<EquipmentGoal.StatesInstance>
    {
        public override void OnSpawn()
        {
            smi.StartSM();
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, EquipmentGoal, object>.GameInstance
        {
            [MyCmpGet]
            public LifeGoalTracker lifeGoals;

            [MyCmpGet]
            public Equipment equipment;

            public StatesInstance(EquipmentGoal master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, EquipmentGoal>
        {
            public State idle;
            public State celebrate;
            public State satisfied;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;

                idle
                    .EventHandlerTransition(GameHashes.EquippedItemEquipper, satisfied, IsWantedEquipment);

                satisfied
                    .TriggerOnEnter(ModHashes.lifeGoalFulfilled)
                    .EventHandlerTransition(GameHashes.UnequippedItemEquipper, idle, IsWantedEquipment)
                    .TriggerOnExit(ModHashes.lifeGoalLost);
            }

            private bool IsWantedEquipment(StatesInstance smi, object data)
            {
                return data is KPrefabID kPrefabID && kPrefabID.IsPrefabID(smi.lifeGoals.wantTag);
            }
        }
    }
}
