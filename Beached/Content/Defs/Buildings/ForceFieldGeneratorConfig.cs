using Beached.Content.Scripts;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Buildings
{
    public class ForceFieldGeneratorConfig : IBuildingConfig
    {
        public const string ID = "Beached_AtmosphericForceFieldGenerator";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                5,
                3,
                "beached_forcefieldgenerator_kanim",
                BUILDINGS.HITPOINTS.TIER3,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER5,
                MATERIALS.ALL_METALS,
                BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                BuildLocationRule.OnFloor,
                DECOR.NONE,
                NOISE_POLLUTION.NOISY.TIER3);

            def.OnePerWorld = true;
            def.DebugOnly = true;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<ForceField>().offset = new Vector3(2.5f, 2.75f);
        }
    }
}
