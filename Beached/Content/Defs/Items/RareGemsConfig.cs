using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
    public class RareGemsConfig : IMultiEntityConfig
    {
        public static List<GameObject> items = new();

        public const string AMBER_INCLUSION_BUG = "Beached_Item_AmberInclusionBug";
        public const string AMBER_INCLUSION_HATCH = "Beached_Item_AmberInclusionHatch";
        public const string AMBER_INCLUSION_MICRORAPTOR = "Beached_Item_AmberInclusionMicroraptor";
        public const string AMBER_INCLUSION_SCORPION = "Beached_Item_AmberInclusionScorpion";
        public const string FLAWLESS_DIAMOND = "Beached_Item_Flawless_Diamond";
        public const string HADEAN_ZIRCON = "Beached_Item_HadeanZircon";
        public const string MAXIXE = "Beached_Item_Maxixe";
        public const string MOTHER_PEARL = "Beached_Item_MotherPearl";

        public List<GameObject> CreatePrefabs()
        {
            items = new()
            {
                CreateGem(FLAWLESS_DIAMOND, STRINGS.ITEMS.GEMS.FLAWLESS_DIAMOND.NAME, STRINGS.ITEMS.GEMS.FLAWLESS_DIAMOND.DESCRIPTION, "beached_flawless_diamond_kanim", DECOR.BONUS.TIER2, SimHashes.Diamond),
                CreateGem(HADEAN_ZIRCON, STRINGS.ITEMS.GEMS.HADEAN_ZIRCON.NAME, STRINGS.ITEMS.GEMS.HADEAN_ZIRCON.DESCRIPTION, "beached_hadean_zircon_kanim", DECOR.BONUS.TIER2, Elements.zirconiumOre),
                CreateGem(MAXIXE, STRINGS.ITEMS.GEMS.MAXIXE.NAME, STRINGS.ITEMS.GEMS.MAXIXE.DESCRIPTION, "beached_maxixe_kanim", DECOR.BONUS.TIER2, Elements.aquamarine)
            };

            return items;
        }

        public static GameObject CreateGem(string ID, string name, string description, string anim, EffectorValues decor, SimHashes element)
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                description,
                1f,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.Creatures,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.66f,
                0.75f,
                true,
                0,
                element,
                additionalTags: new List<Tag>
                {
                    BTags.MaterialCategories.crystal,
                    GameTags.PedestalDisplayable
                });

            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<SimpleMassStatusItem>();
            prefab.AddOrGet<OccupyArea>().OccupiedCellsOffsets = EntityTemplates.GenerateOffsets(1, 1);
            prefab.AddOrGet<DecorProvider>().SetValues(decor);

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
