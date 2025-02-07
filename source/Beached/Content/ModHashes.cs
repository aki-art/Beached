namespace Beached.Content
{
	// Wrapper for GameHashes enum for convenience
	public class ModHashes
	{
		public static readonly ModHashes
			onBeachedWorldLoaded = new("OnBeachedWorldLoaded"),
			onBeachedWorldUnLoaded = new("OnBeachedWorldUnLoaded"),
			usedBuilding = new("UsedBuilding"),
			greatAirQuality = new("GreatAirQuality"),
			producedLubricant = new("ProducedLubricant"),
			desiccated = new("Desiccated"),
			enteredDarkness = new("EnteredDarkness"),
			stackableChanged = new("StackableChanged"),
			stackableSegmentDestroyed = new("StackableSegmentDestroyed"),
			lifeGoalFulfilled = new("LifeGoalFulfilled"),
			updateNeighbors = new("UpdateNeighbors"),
			blockUpdate = new("BlockUpdate"),
			lifeGoalLost = new("LifeGoalLost"),
			lifeGoalTrackerUpdate = new("LifeGoalTrackerUpdate"),
			wishingStarEvent = new("WishingStarEvent"),
			debugDataChange = new("DebugDataChange"),
			prePoop = new("PrePoop"),
			critterMined = new("CritterMined"),
			sidesSreenRefresh = new("SideScreenRefresh"),
			builtNest = new("BuiltNest"),
			medusaSignal = new("MedusaSignal"),
			depleted = new("Depleted"),
			crystalRotated = new("CrystalRotated"),
			crystalHarvested = new("CrystalHarvested"),

			segmentedEntityUpdate = new("SegmentedEntityUpdate"),
			critterTraitAdded = new("CritterTraitAdded");

		private readonly int value;
		private readonly string name;
		private readonly GameHashes hash;

		public ModHashes(string name)
		{
			name = "Beached_" + name;
			this.name = name;
			value = Hash.SDBMLower(name);
			hash = (GameHashes)value;
		}

		public static implicit operator GameHashes(ModHashes modHashes) => modHashes.hash;

		public static implicit operator int(ModHashes modHashes) => modHashes.value;

		public static implicit operator string(ModHashes modHashes) => modHashes.name;

		public override string ToString() => name;
	}
}
