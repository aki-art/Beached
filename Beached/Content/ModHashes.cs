namespace Beached.Content
{
    // Wrapper for GameHashes enum for convenience
    public class ModHashes
    {
        public static readonly ModHashes OnBeachedWorldLoaded = new("Beached_OnBeachedWorldLoaded");
        public static readonly ModHashes OnBeachedWorldUnLoaded = new("Beached_OnBeachedWorldUnLoaded");
        public static readonly ModHashes GreatAirQuality = new("Beached_GreatAirQuality");
        public static readonly ModHashes ProducedLubricant = new("Beached_ProducedLubricant");
        public static readonly ModHashes Desiccated = new("Beached_Desiccated");
        public static readonly ModHashes EnteredDarkness = new("Beached_EnteredDarkness");
        public static readonly ModHashes StackableChanged = new("Beached_StackableChanged");
        public static readonly ModHashes LifeGoalFulfilled = new("Beached_LifeGoalFulfilled");
        public static readonly ModHashes LifeGoalLost = new("Beached_LifeGoalLost");

        private readonly int value;
        private readonly string name;
        private readonly GameHashes hash;

        public ModHashes(string name)
        {
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
