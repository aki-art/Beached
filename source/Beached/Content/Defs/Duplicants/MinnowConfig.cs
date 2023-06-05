using Beached.Content.ModDb;

namespace Beached.Content.Defs.Duplicants
{
	public class MinnowConfig : IDuplicantConfig
	{
		public const string ID = "MINNOW";

		public string GetID() => ID;

		public Personality CreatePersonality()
		{
			return new Personality(
					GetID(),
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MINNOW.NAME,
					CONSTS.DUPLICANTS.GENDER.FEMALE,
					null,
					"StressVomiter",
					"HappySinger",
					"",
					null,
					1,
					1,
					1,
					2,
					4,
					2,
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MINNOW.DESC,
					true);
		}

		public void OnSpawn(MinionIdentity identity) { }

		public void OnTraitRoll(MinionStartingStats stats)
		{
			var trait = Db.Get().traits.Get(BTraits.GILLS);
			stats.Traits.Add(trait);
		}

		public void ModifyBodyData(ref KCompBuilder.BodyData bodyData)
		{
			bodyData.hair = HashCache.Get().Add("hair_minnow");
			bodyData.headShape = HashCache.Get().Add("headshape_minnow");
			bodyData.mouth = HashCache.Get().Add("mouth_minnow");
		}
	}
}
