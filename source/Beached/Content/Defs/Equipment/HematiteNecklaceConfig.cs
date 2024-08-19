using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class HematiteNecklaceConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_HematiteNecklace";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20),
				new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, 100f),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.heulandite,
				30f,
				TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0,
				CONSTS.SNAPONS.JEWELLERIES.HEMATITE,
				"peached_hematite_necklace_kanim",
				99,
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
