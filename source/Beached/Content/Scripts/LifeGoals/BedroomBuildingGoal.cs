namespace Beached.Content.Scripts.LifeGoals
{
    public class BedroomBuildingGoal : StateMachineComponent<BedroomBuildingGoal.StatesInstance>
    {
        public override void OnSpawn()
        {
            smi.StartSM();
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, BedroomBuildingGoal, object>.GameInstance
        {
            [MyCmpGet]
            public Beached_LifeGoalTracker lifeGoals;

            [MyCmpGet]
            public MinionIdentity minionIdentity;

            public Assignable bed;
            public Operational wantTarget;
            public Room bedroom;

            public StatesInstance(BedroomBuildingGoal master) : base(master)
            {
            }
        }

        public class States : GameStateMachine<States, StatesInstance, BedroomBuildingGoal>
        {
            public State idle;
            public HasWantedObjectStates hasWanted;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;

                root
                    .EventHandler(GameHashes.AssignablesChanged, Recheck);

                idle
                    .Enter(smi => Recheck(smi, null))
                    .ToggleStatusItem("bedless", "");

                hasWanted
                    .ToggleStatusItem("hasWanted", "")
                    .DefaultState(hasWanted.unoperational)
                    .UpdateTransition(idle, RecheckBedAndTarget, UpdateRate.SIM_4000ms);

                hasWanted.unoperational
                    .ToggleStatusItem("hasWanted.unoperational", "")
                    .EventHandlerTransition(GameHashes.OperationalChanged, smi => smi.wantTarget, hasWanted.satisfied, IsTargetOperational);

                hasWanted.satisfied
                    .ToggleStatusItem("hasWanted.satisfied", "")
                    .TriggerOnEnter(ModHashes.lifeGoalFulfilled)
                    .EventHandlerTransition(GameHashes.OperationalChanged, smi => smi.wantTarget, hasWanted.unoperational, (smi, data) => !IsTargetOperational(smi, data))
                    .TriggerOnExit(ModHashes.lifeGoalLost);
            }

            private bool RecheckBedAndTarget(StatesInstance smi, float _)
            {
                if (smi.bed == null || smi.wantTarget == null)
                {
                    Log.Debug("bed or target is null");
                    return true;
                }

                var bedsRoom = Game.Instance.roomProber.GetRoomOfGameObject(smi.bed.gameObject);
                var targetssRoom = Game.Instance.roomProber.GetRoomOfGameObject(smi.wantTarget.gameObject);

                if (!IsBedroom(bedsRoom) || bedsRoom != targetssRoom)
                {
                    Log.Debug("not in bedroom");
                    return true;
                }

                return false;
            }

            private void Recheck(StatesInstance smi, object data)
            {
                Log.Debug("ASSIGNBALE CHANGED");
                if (data is not AssignableSlotInstance assignableSlotInstance)
                {
                    return;
                }

                if (assignableSlotInstance.slot != Db.Get().AssignableSlots.Bed)
                {
                    Log.Debug("not a bed assignment");
                    return;
                }

                Log.Debug("ASSIGNBALE ok");

                var soleOwner = smi.minionIdentity.GetSoleOwner();
                var bed = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);

                if (bed != null)
                {
                    var room = Game.Instance.roomProber.GetRoomOfGameObject(bed.gameObject);

                    if (IsBedroom(room))
                    {
                        Log.Debug("Got bed in bedroom");
                        smi.bed = bed;

                        if (smi.wantTarget == null)
                        {
                            foreach (var building in room.buildings)
                            {
                                if (building.IsPrefabID(smi.lifeGoals.wantTag))
                                {
                                    smi.wantTarget = building.GetComponent<Operational>();
                                    smi.GoTo(smi.wantTarget.IsOperational ? smi.sm.hasWanted.satisfied : smi.sm.hasWanted.unoperational);
                                    return;
                                }
                            }
                        }

                        Log.Debug("has bed but no surf board");
                        smi.GoTo(smi.sm.idle);
                    }
                    else
                    {
                        Log.Debug("bed not in bedroom");
                    }
                }

                smi.bed = null;
                smi.wantTarget = null;
                smi.GoTo(smi.sm.idle);
            }

            private bool IsBedroom(Room room)
            {
                if (room == null)
                {
                    return false;
                }

                var roomTypes = Db.Get().RoomTypes;
                return room.roomType == roomTypes.Bedroom || room.roomType == roomTypes.Barracks;
            }

            public class HasWantedObjectStates : State
            {
                public State unoperational;
                public State satisfied;
            }


            private bool DoesntHaveBed(StatesInstance smi, object data)
            {
                if (smi.bed == null)
                {
                    return true;
                }

                var soleOwner = smi.minionIdentity.GetSoleOwner();
                return soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed) != smi.bed;
            }

            private bool WantedBuildingNoLongerExists(StatesInstance smi, float arg2)
            {
                if (smi.wantTarget == null)
                {
                    return true;
                }

                var room = Game.Instance.roomProber.GetRoomOfGameObject(smi.bed.gameObject);
                var roomTypes = Db.Get().RoomTypes;

                return !(room.roomType == roomTypes.Bedroom || room.roomType == roomTypes.Barracks);
            }

            private bool BedNoLongerExists(StatesInstance smi, float arg2)
            {
                if (smi.bed == null)
                {
                    return true;
                }

                var room = Game.Instance.roomProber.GetRoomOfGameObject(smi.bed.gameObject);
                var roomTypes = Db.Get().RoomTypes;

                return !(room.roomType == roomTypes.Bedroom || room.roomType == roomTypes.Barracks);
            }

            private bool IsTargetOperational(StatesInstance smi, object _)
            {
                return smi.wantTarget != null && smi.wantTarget.GetComponent<Operational>().IsOperational;
            }

            private bool HasTargetBuilding(StatesInstance smi, float _)
            {
                if (smi.bed == null)
                {
                    Log.Debug("has no bed");
                    return false;
                }

                var room = Game.Instance.roomProber.GetRoomOfGameObject(smi.bed.gameObject);

                if (room == null)
                {
                    Log.Debug("has no room");
                    return false;
                }

                var roomTypes = Db.Get().RoomTypes;

                if (!(room.roomType == roomTypes.Bedroom || room.roomType == roomTypes.Barracks))
                {
                    Log.Debug("not in bedroom");
                    return false;
                }

                foreach (var building in room.buildings)
                {
                    if (building.IsPrefabID(smi.lifeGoals.wantTag))
                    {
                        smi.wantTarget = building.GetComponent<Operational>();
                        return true;
                    }
                }

                Log.Debug("no surf board");
                return false;
            }

            private bool HasBed(StatesInstance smi, object _)
            {
                var soleOwner = smi.minionIdentity.GetSoleOwner();
                var assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);

                if (assignable != null)
                {
                    smi.bed = assignable;
                    return true;
                }

                return false;
            }
        }
    }
}
