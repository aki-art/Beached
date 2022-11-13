using Beached.Content.ModDb;
using Klei.AI;

namespace Beached.Content.Scripts
{
    public class FreshAirMonitor : GameStateMachine<FreshAirMonitor, FreshAirMonitor.Instance, IStateMachineTarget, FreshAirMonitor.Def>
    {
        public override void InitializeStates(out BaseState default_state)
        {
            default_state = root;

            root
                .EventHandler(ModHashes.GreatAirQuality, (smi, data) => OnBreatheFreshAir(smi));
        }

        private void OnBreatheFreshAir(Instance smi)
        {
            smi.effects.Add(BEffects.OCEAN_BREEZE, true);
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Effects effects;

            public Instance(IStateMachineTarget master) : base(master)
            {
                effects = GetComponent<Effects>();
            }
        }
    }
}
