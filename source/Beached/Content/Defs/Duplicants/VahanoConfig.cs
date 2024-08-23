using Beached.Content.ModDb;

namespace Beached.Content.Defs.Duplicants
{
	public class VahanoConfig : IDuplicantConfig
	{
		public const string ID = "BEACHED_VAHANO";
		public const int HASH = -1199214942;

		public Personality CreatePersonality()
		{
			return new Personality(
					GetID(),
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_VAHANO.NAME,
					CONSTS.DUPLICANTS.GENDER.FEMALE,
					null,
					BTraits.SIREN,
					BTraits.PLUSHIE_MAKER,
					"",
					null,
					HASH,
					HASH,
					1,
					HASH,
					HASH,
					2,
					0,
					0,
					0,
					0,
					0,
					0,
					STRINGS.DUPLICANTS.PERSONALITIES.BEACHED_VAHANO.DESC,
					false);
		}

		public string GetHeadAnim() => "beached_vahano_head_kanim";

		public string GetID() => ID;

		public void ModifyBodyData(ref KCompBuilder.BodyData bodyData)
		{
		}

		public void OnSpawn(MinionIdentity identity)
		{
			identity.GetComponent<KBatchedAnimController>().animScale *= 0.9f;
		}

		public void OnTraitRoll(MinionStartingStats stats)
		{
		}
	}
}
