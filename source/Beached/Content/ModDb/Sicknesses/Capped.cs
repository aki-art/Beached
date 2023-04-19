using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb.Sicknesses
{
    /// Disallowing helmet gear: 
    /// <see cref="Beached.Patches.HelmetControllerPatch.HelmetController_OnPrefabInit_Patch"/>
    public class Capped : Sickness.SicknessComponent
    {
        public static Dictionary<int, bool> dupesWithCaps = new Dictionary<int, bool>();
        private static List<string> caps = new List<string>()
        {
            "beached_greencap_kanim",
            "beached_purplecap_kanim",
            "beached_bluecap_kanim"
        };

        public override void OnCure(GameObject go, object instance_data)
        {
            // TODO: restore hat
            ToggleSymbolOverride(go, false, (string)instance_data, "snapTo_hat");
            go.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
        }

        public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
        {
            // TODO: put to MinionStorage
            var cap = caps.GetRandom();
            ToggleSymbolOverride(go, true, cap, "snapTo_hat");
            go.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);

            return cap;
        }

        public override List<Descriptor> GetSymptoms()
        {
            return new List<Descriptor>()
            {
                new Descriptor("Occupied Head", "Cannot wear Helmets (such as Atmo Suits).")
            };
        }

        private void ToggleSymbolOverride(GameObject target, bool on, string anim_file, KAnimHashedString targetSymbolName)
        {
            var kbac = target.GetComponent<KBatchedAnimController>();
            if (kbac == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(anim_file))
            {
                if (kbac.TryGetComponent(out SymbolOverrideController soc))
                {
                    if (on)
                    {
                        var anim = Assets.GetAnim(anim_file);
                        var source_symbol = anim.GetData().build.GetSymbol(targetSymbolName);
                        soc.AddSymbolOverride(targetSymbolName, source_symbol, 10);
                        kbac.SetSymbolVisiblity(targetSymbolName, true);
                    }
                    else
                    {
                        soc.RemoveSymbolOverride(targetSymbolName, 10);
                        kbac.SetSymbolVisiblity(targetSymbolName, false);
                    }
                }
            }
        }
    }
}
