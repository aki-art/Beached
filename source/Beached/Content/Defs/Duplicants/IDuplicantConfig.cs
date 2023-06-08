namespace Beached.Content.Defs.Duplicants
{
	public interface IDuplicantConfig
	{
		public string GetID();

		public Personality CreatePersonality();

		public void OnTraitRoll(MinionStartingStats stats);

		public void OnSpawn(MinionIdentity identity);

		public void ModifyBodyData(ref KCompBuilder.BodyData bodyData);

		public string GetHeadAnim();
	}
}
