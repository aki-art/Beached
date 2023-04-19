using Beached.Content.Scripts.LifeGoals;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
    // goes on an Sleepable, so it can keep track of a dupe's bedroom
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TargetOfGoalTracker : KMonoBehaviour
    {
        [SerializeField]
        public Tag targetTag;

        [MyCmpReq]
        private Assignable assignable;
        private GameObject target;

        [Serialize]
        private bool isAssignedToTrackedDupe;

        [Serialize]
        private Ref<KPrefabID> targetRef;

        public override void OnSpawn()
        {
            Subscribe((int)GameHashes.AssigneeChanged, OnAssigneeChanged);

            if (targetRef != null)
            {
                OnUpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));
            }
        }

        private void OnAssigneeChanged(object obj)
        {
            if (obj is IAssignableIdentity assignee)
            {
                var identity = assignee.GetSoleOwner();

                if (identity != null && identity.TryGetComponent(out BedroomBuildingGoal goal))
                {
                    Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
                    isAssignedToTrackedDupe = true;

                    return;
                }
            }

            if (isAssignedToTrackedDupe)
            {
                Unsubscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
            }

            isAssignedToTrackedDupe = false;
        }

        private Room GetRoom(GameObject target)
        {
            return Game.Instance.roomProber.GetRoomOfGameObject(target);
        }

        private void OnUpdateRoom(object data)
        {
            if (assignable.assignee == null)
            {
                // no dupe is assigned
                return;
            }

            GameObject newTarget = null;
            var isTargetValid = target == null;

            if (data is Room room)
            {
                isTargetValid &= GetRoom(target) == room;

                if (isTargetValid)
                {
                    // all is in order
                    return;
                }

                // target was lost or changed, look for it again
                foreach (var building in room.buildings)
                {
                    if (building.HasTag(targetTag))
                    {
                        newTarget = building.gameObject;
                        SetTarget(building.gameObject);

                        return;
                    }
                }
            }

            SetTarget(null);
        }

        private void SetTarget(GameObject newTarget)
        {
            if (target != newTarget)
            {
                TriggerOwner(newTarget.gameObject);
            }

            target = newTarget;
            targetRef = newTarget == null ? null : new Ref<KPrefabID>(newTarget.GetComponent<KPrefabID>());
        }

        private void TriggerOwner(GameObject target)
        {
            if (assignable.assignee != null)
            {
                assignable.assignee.GetSoleOwner()?.Trigger(ModHashes.lifeGoalTrackerUpdate, (target, this));
            }
        }
    }
}
