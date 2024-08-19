using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class StrangeMatterAmuletConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_StrangeMatterAmulet";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.aquamarine,
				30f,
				TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0,
				CONSTS.SNAPONS.JEWELLERIES.STRANGE_MATTER,
				"beached_strange_matter_necklace_kanim",
				4,
				attributeModifiers,
				additional_tags:
				[
					GameTags.PedestalDisplayable
				]);

			return equipmentDef;
		}

		public void DoPostConfigure(GameObject go) { }

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
	}
}
