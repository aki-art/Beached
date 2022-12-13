using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Corals
{
    public class CoralTemplate
    {
        public static readonly SimHashes[] ALL_WATERS = new[]
        {
            SimHashes.Water,
            SimHashes.SaltWater,
            SimHashes.Brine,
            SimHashes.DirtyWater,
            Elements.MurkyBrine,
            Elements.SulfurousWater
        };

        public static GameObject Create(string id, float mass, string anim, int width, int height, EffectorValues decor, float defaultTemperature = 293f, string initialAnim = "idle_loop", List<Tag> additionalTags = null, SimHashes[] safeElements = null)
        {
            var name = Strings.TryGet("STRINGS.CORALS." + id.ToUpperInvariant() + ".NAME", out var n) ? n.String : "no name";
            var desc = Strings.TryGet("STRINGS.CORALS." + id.ToUpperInvariant() + ".DESCRIPTION", out var d) ? d.String : "no desc";

            var prefab = EntityTemplates.CreatePlacedEntity(
                id,
                name,
                desc,
                mass,
                Assets.GetAnim(anim),
                initialAnim,
                Grid.SceneLayer.BuildingBack,
                width,
                height,
                decor,
                NOISE_POLLUTION.NONE,
                SimHashes.Creature,
                additionalTags,
                defaultTemperature);

            prefab.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;

            prefab.AddOrGet<SimTemperatureTransfer>();
            prefab.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
            {
                ObjectLayer.Building
            };

            prefab.AddOrGet<EntombVulnerable>();
            //prefab.AddOrGet<DrowningMonitor>().livesUnderWater = true;

            if (safeElements != null)
            {
                prefab.AddOrGet<PressureVulnerable>().Configure(ALL_WATERS);
            }

            prefab.AddOrGet<Prioritizable>();
            prefab.AddOrGet<Uprootable>();

            prefab.AddOrGet<Harvestable>();
            prefab.AddOrGet<HarvestDesignatable>();

            prefab.AddTag(BTags.Coral);

            return prefab;
        }

        public static void AddSimpleConverter(GameObject prefab, SimHashes input, float inKgPerSecond, SimHashes output, float outKgPerSecond = -1, float offsetX = 0, float offsetY = 0, float outputMultiplier = 1f, byte disase = byte.MaxValue, int diseaseCount = 0, bool storeOutput = true)
        {
            if (outKgPerSecond == -1)
            {
                outKgPerSecond = inKgPerSecond;
            }

            var elementConverter = prefab.AddComponent<ElementConverter>();
            elementConverter.OutputMultiplier = outputMultiplier;

            elementConverter.consumedElements = new[]
            {
                new ElementConverter.ConsumedElement(input.ToString(), inKgPerSecond)
            };

            elementConverter.outputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(outKgPerSecond,
                    output,
                    0f,
                    true,
                    storeOutput,
                    offsetX,
                    offsetY,
                    0.75f,
                    disase,
                    diseaseCount)
            };
        }
    }
}
