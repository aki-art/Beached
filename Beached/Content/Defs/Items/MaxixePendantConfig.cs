using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
    public class MaxixePendantConfig : IEquipmentConfig
    {
        public const string ID = "Beached_Equipment_MaxixePendant";

        public EquipmentDef CreateEquipmentDef()
        {
            var attributeModifiers = new List<AttributeModifier>
            {
                new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20, "Maxixe Pendant")
            };

            var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
                ID,
                BAssignableSlots.JEWELLERY_ID,
                Elements.Aquamarine,
                30f,
                TUNING.EQUIPMENT.VESTS.COOL_VEST_ICON0,
                TUNING.EQUIPMENT.VESTS.SNAPON0,
                TUNING.EQUIPMENT.VESTS.COOL_VEST_ANIM0,
                4,
                attributeModifiers,
                additional_tags: new Tag[]
                {
                    GameTags.PedestalDisplayable
                });

            return equipmentDef;
        }

        public void DoPostConfigure(GameObject go)
        {
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
    }
}
