using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using static ClothingWearer;

namespace Beached.Content.Defs.Equipment
{
    internal class RubberBootsConfig : IEquipmentConfig
    {
        public const string ID = "Beached_RubberBoots";

        public EquipmentDef CreateEquipmentDef()
        {
            var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
                ID,
                BAssignableSlots.SHOES_ID,
                Elements.Rubber,
                30f,
                TUNING.EQUIPMENT.VESTS.COOL_VEST_ICON0,
                CONSTS.SNAPONS.RUBBER_BOOTS,
                "beached_rubberboots_kanim",
                4,
                new List<AttributeModifier>(),
                additional_tags: new Tag[]
                {
                    GameTags.PedestalDisplayable
                });

            var clothingInfo = ClothingInfo.BASIC_CLOTHING;

            equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
            equipmentDef.OnEquipCallBack = eq => CoolVestConfig.OnEquipVest(eq, clothingInfo);
            equipmentDef.OnUnequipCallBack = CoolVestConfig.OnUnequipVest;

            return equipmentDef;
        }

        public void DoPostConfigure(GameObject go)
        {
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
    }
}
