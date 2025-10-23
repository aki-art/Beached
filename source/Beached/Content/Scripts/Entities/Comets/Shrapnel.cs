namespace Beached.Content.Scripts.Entities.Comets
{
	public class Shrapnel : Comet
	{
		[MyCmpReq]
		private LoopingSounds loopingSound;

		private HashedString FLYING_SOUND_PARAMETER = "meteorType";

		public void SetExplosionMass(float value)
		{
			explosionMass = value;
		}

		public override void OnSpawn()
		{
			StartLoopingSound2();
		}

		private void StartLoopingSound2()
		{
			loopingSound.StartSound(flyingSound);
			loopingSound.UpdateFirstParameter(flyingSound, FLYING_SOUND_PARAMETER, flyingSoundID);
		}
	}
}
