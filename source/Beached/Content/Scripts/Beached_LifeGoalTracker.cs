using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_LifeGoalTracker : KMonoBehaviour
	{
		[MyCmpReq] private ChoreProvider choreProvider;

		[Serialize] private Dictionary<string, int> attributesSerialized;
		[Serialize] public Tag wantTag;
		[Serialize] public Func<AssignableSlotInstance, bool> hasWantedAssignableFn;

		public bool isGoalFulfilled;

		public List<AttributeModifier> fulfilledLifegoalModifiers;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (attributesSerialized != null)
			{
				InitAttributes(attributesSerialized);
			}

			Subscribe(ModHashes.lifeGoalFulfilled, OnLifegoalFulfilled);
			Subscribe(ModHashes.lifeGoalLost, OnLifegoalLost);
		}

		private void OnLifegoalFulfilled(object obj)
		{
			if (isGoalFulfilled || fulfilledLifegoalModifiers == null)
			{
				return;
			}

			foreach (var attribute in fulfilledLifegoalModifiers)
			{
				this.GetAttributes().Add(attribute);
			}

			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Goal Fulfilled", transform);
			new EmoteChore(choreProvider, Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.Cheer);

			isGoalFulfilled = true;
		}

		private void OnLifegoalLost(object _)
		{
			if (!isGoalFulfilled || fulfilledLifegoalModifiers == null)
			{
				return;
			}

			foreach (var attribute in fulfilledLifegoalModifiers)
			{
				this.GetAttributes().Remove(attribute);
			}

			isGoalFulfilled = false;

			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, "Goal Lost", transform);
			new EmoteChore(choreProvider, Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.Disappointed);
		}

		public void AddAttributes(Dictionary<string, int> attributes)
		{
			attributesSerialized = attributes;
			InitAttributes(attributes);
		}

		private void InitAttributes(Dictionary<string, int> attributes)
		{
			fulfilledLifegoalModifiers = [];
			foreach (var attribute in attributes)
			{
				fulfilledLifegoalModifiers.Add(new AttributeModifier(attribute.Key, attribute.Value, "Life Goal"));
			}
		}

		public void AddSimpleAssignable(Tag tag)
		{
			hasWantedAssignableFn = slot => slot.assignable != null && slot.assignable.PrefabID() == tag;
		}

		public bool HasWantedAssignable(AssignableSlotInstance assignableSlotInstance)
		{
			return assignableSlotInstance != null 
				&& hasWantedAssignableFn != null 
				&& hasWantedAssignableFn(assignableSlotInstance);
		}
	}
}
