namespace Beached.Content
{
    // Wrapper for GameHashes enum for convenience
    public class ModHashes
    {
        public static readonly ModHashes
            OnBeachedWorldLoaded = new("OnBeachedWorldLoaded"),
            OnBeachedWorldUnLoaded = new("OnBeachedWorldUnLoaded"),
            GreatAirQuality = new("GreatAirQuality"),
            ProducedLubricant = new("ProducedLubricant"),
            Desiccated = new("Desiccated"),
            EnteredDarkness = new("EnteredDarkness"),
            StackableChanged = new("StackableChanged"),
            LifeGoalFulfilled = new("LifeGoalFulfilled"),
            LifeGoalLost = new("LifeGoalLost"),
            LifeGoalTrackerUpdate = new("LifeGoalTrackerUpdate"),
            DebugDataChange = new("DebugDataChange");

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
