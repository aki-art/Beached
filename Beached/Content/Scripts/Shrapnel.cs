using HarmonyLib;

namespace Beached.Content.Scripts
{
    public class Shrapnel : Comet
    {
        [MyCmpReq]
        private LoopingSounds loopingSounds;

        private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

        private static AccessTools.FieldRef<Comet, float> explosionMass;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            if (explosionMass == null)
            {
                var f_explosionMass = AccessTools.Field(typeof(Comet), "explosionMass");
                explosionMass = AccessTools.FieldRefAccess<Comet, float>(f_explosionMass);
            }
        }

        public void SetExplosionMass(float value)
        {
            explosionMass(this) = value;
        }

        protected override void OnSpawn()
        {
            StartLoopingSound();
        }

        private void StartLoopingSound()
        {
            loopingSounds.StartSound(flyingSound);
            loopingSounds.UpdateFirstParameter(flyingSound, FLYING_SOUND_ID_PARAMETER, flyingSoundID);
        }
    }
}
