using Beached.Content.ModDb;

namespace Beached.Content.Defs.Duplicants
{
	public class MinnowConfig : IDuplicantConfig
	{
		public const string ID = "BEACHED_MINNOW";
		public const int HASH = 770288065;

		public string GetID() => ID;

		public Personality CreatePersonality()
		{
			return new Personality(
					GetID(),
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MINNOW.NAME,
					CONSTS.DUPLICANTS.GENDER.FEMALE,
					null,
					BTraits.SIREN,
					BTraits.PLUSHIE_MAKER,
					"",
					BTraits.GILLS,
					HASH,
					HASH,
					1,
					2,
					HASH,
					2,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_MINNOW.DESC,
					true,
					"beached_minnow", //TODO?: custom gravestone
					GameTags.Minions.Models.Standard,
					0 //TODO? custom speech mouth);
					);
		}

		public void OnSpawn(MinionIdentity identity) { }

		public void OnTraitRoll(MinionStartingStats stats)
		{
			//var trait = Db.Get().traits.Get(BTraits.GILLS);
			//stats.Traits.Add(trait);
		}

		public void ModifyBodyData(ref KCompBuilder.BodyData bodyData)
		{
		}

		public string GetHeadAnim() => "minnow_head_kanim";
	}
}
