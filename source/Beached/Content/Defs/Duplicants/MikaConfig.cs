using Beached.Content.ModDb;

namespace Beached.Content.Defs.Duplicants
{
	public class MikaConfig : IDuplicantConfig
	{
		public const string ID = "BEACHEDMIKA";

		public string GetID() => ID;

		public Personality CreatePersonality()
		{
			return new Personality(
					GetID(),
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MIKA.NAME,
					CONSTS.DUPLICANTS.GENDER.MALE,
					null,
					"StressVomiter",
					"HappySinger",
					"",
					null,
					4,
					1,
					1,
					3,
					25,
					1,
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MIKA.DESC,
					true);
		}

		public void OnSpawn(MinionIdentity identity) { }

		public void OnTraitRoll(MinionStartingStats stats)
		{
			var trait = Db.Get().traits.Get(BTraits.COMFORT_SEEKER);
			stats.Traits.Add(trait);
		}

		public void ModifyBodyData(ref KCompBuilder.BodyData bodyData)
		{
		}
	}
}
