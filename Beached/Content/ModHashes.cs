namespace Beached.Content
{
    // Wrapper for GameHashes enum for convenience
    public class ModHashes
    {
        public static readonly ModHashes OnBeachedWorldLoaded = new("OnBeachedWorldLoaded");
        public static readonly ModHashes OnBeachedWorldUnLoaded = new("OnBeachedWorldUnLoaded");
        public static readonly ModHashes GreatAirQuality = new("GreatAirQuality");
        public static readonly ModHashes ProducedLubricant = new("ProducedLubricant");
        public static readonly ModHashes Desiccated = new("Desiccated");
        public static readonly ModHashes EnteredDarkness = new("EnteredDarkness");
        public static readonly ModHashes StackableChanged = new("StackableChanged");
        public static readonly ModHashes LifeGoalFulfilled = new("LifeGoalFulfilled");
        public static readonly ModHashes LifeGoalLost = new("LifeGoalLost");
        public static readonly ModHashes LifeGoalTrackerUpdate = new("LifeGoalTrackerUpdate");
        public static readonly ModHashes DebugDataChange = new("DebugDataChange");

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
