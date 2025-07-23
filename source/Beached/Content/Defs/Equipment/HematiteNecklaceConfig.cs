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
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20),
				new(Db.Get().Attributes.CarryAmount.Id, 100f),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.zeolite,
				30f,
				"beached_hematite_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.HEMATITE,
				"beached_hematite_necklace_kanim",
				99,
				attributeModifiers,
				additional_tags:
				[
					GameTags.PedestalDisplayable
				]);

			return equipmentDef;
		}

		public void DoPostConfigure(GameObject go) { }

		public string[] GetDlcIds() => null;
	}
}
