namespace Beached.Content.Scripts.LifeGoals
{
    public class BedroomBuildingGoal : StateMachineComponent<BedroomBuildingGoal.StatesInstance>
    {
        protected override void OnSpawn()
        {
            smi.StartSM();
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, BedroomBuildingGoal, object>.GameInstance
        {
            [MyCmpGet]
            public LifeGoalTracker lifeGoals;

            [MyCmpGet]
            public MinionIdentity minionIdentity;

            public Assignable bed;

            public StatesInstance(BedroomBuildingGoal master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, BedroomBuildingGoal>
        {
            public State bedless;
            public State hasBed;
            public State satisfied;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = bedless;

                bedless
                    .EventHandlerTransition(GameHashes.AssignablesChanged, hasBed, HasBed);

                hasBed
                    .UpdateTransition(satisfied, HasTargetBuilding, UpdateRate.SIM_4000ms);

                satisfied
                    .TriggerOnEnter(ModHashes.LifeGoalFulfilled)
                    .UpdateTransition(bedless, (smi, dt) => !HasBed(smi, null), UpdateRate.SIM_4000ms)
                    .UpdateTransition(hasBed, (smi, dt) => !HasTargetBuilding(smi, dt), UpdateRate.SIM_4000ms)
                    .TriggerOnExit(ModHashes.LifeGoalLost);
            }

            private bool HasTargetBuilding(StatesInstance smi, float _)
            {
                if(smi.bed == null)
                {
                    Log.Debug("has no bed");
                    return false;
                }

                var room = Game.Instance.roomProber.GetRoomOfGameObject(smi.bed.gameObject);

                if(room == null)
                {
                    Log.Debug("has no room");
                    return false;
                }

                var roomTypes = Db.Get().RoomTypes;

                if(!(room.roomType == roomTypes.Bedroom || room.roomType == roomTypes.Barracks))
                {
                    Log.Debug("not in bedroom");
                    return false;
                }

                foreach(var building in room.buildings)
                {
                    return building.IsPrefabID(smi.lifeGoals.wantTag);
                }

                Log.Debug("no surf board");
                return false;
            }

            private bool HasBed(StatesInstance smi, object _)
            {
                var soleOwner = smi.minionIdentity.GetSoleOwner();
                var assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);

                if(assignable != null)
                {
                    smi.bed = assignable;
                    return true;
                }

                return false;
            }

            private bool IsWantedEquipment(StatesInstance smi, object data)
            {
                return data is KPrefabID kPrefabID && kPrefabID.IsPrefabID(smi.lifeGoals.wantTag);
            }
        }
    }
}
