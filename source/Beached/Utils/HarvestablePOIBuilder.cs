using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Utils
{
    public class HarvestablePOIBuilder
    {
        private readonly HarvestablePOIConfigurator.HarvestablePOIType poiType;
        private readonly string ID;
        private readonly string anim;

        private HashedString artifact = ArtifactPOIConfigurator.defaultArtifactPoiType.idHash;

        public HarvestablePOIBuilder(string ID, string anim)
        {
            this.ID = "Beached_HarvestableSpacePOI_" + ID;
            this.anim = anim;

            poiType = new HarvestablePOIConfigurator.HarvestablePOIType(
                ID,
                new(),
                54_000f,
                81_000f,
                30_000f,
                60_000f,
                true,
                HarvestablePOIConfig.AsteroidFieldOrbit,
                20,
                DlcManager.EXPANSION1_ID);
        }

        public HarvestablePOIBuilder Element(SimHashes element, float weight)
        {
            poiType.harvestableElements[element] = weight; 
            return this;
        }

        public HarvestablePOIBuilder SetArtifacts(string preset)
        {
            artifact = preset;
            return this;
        }

        public HarvestablePOIBuilder Capacity(float min, float max)
        {
            poiType.poiCapacityMin = min;
            poiType.poiCapacityMax = max;
            return this;
        }

        public HarvestablePOIBuilder Recharge(float min, float max)
        {
            poiType.poiRechargeMin = min;
            poiType.poiRechargeMax = max;
            return this;
        }

        public GameObject Build()
        {
            string nameKey = $"STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.{ID.ToUpper()}.NAME";
            string descKey = $"STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.{ID.ToUpper()}.DESC";

            var displayName = Strings.TryGet(nameKey, out var name) 
                ? name.String 
                : "MISSING";

            var prefab = EntityTemplates.CreateEntity(ID, displayName);

            prefab.AddOrGet<SaveLoadRoot>();

            var harvestablePoiConfigurator = prefab.AddOrGet<HarvestablePOIConfigurator>();
            harvestablePoiConfigurator.presetType = poiType.id;

            var harvestablePoiClusterGridEntity = prefab.AddOrGet<Beached_HarvestablePOIClusterGridEntity>();
            harvestablePoiClusterGridEntity.m_name = displayName;
            harvestablePoiClusterGridEntity.m_Anim = "asteroid_field";
            harvestablePoiClusterGridEntity.animFile = anim;

            prefab.AddOrGetDef<HarvestablePOIStates.Def>();

            if (poiType.canProvideArtifacts)
            {
                prefab.AddOrGetDef<ArtifactPOIStates.Def>();
                ArtifactPOIConfigurator artifactPoiConfigurator = prefab.AddOrGet<ArtifactPOIConfigurator>();
                artifactPoiConfigurator.presetType = artifact;
            }

            var info = prefab.AddOrGet<InfoDescription>();
            info.description = Strings.TryGet(descKey, out var desc)
                ? desc.String
                : "MISSING";

            return prefab;
        }
    }
}
