﻿using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class PearlNecklaceConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_PearlNecklace";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 30)
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.aquamarine,
				30f,
				TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0,
				CONSTS.SNAPONS.JEWELLERIES.PEARL,
				"beached_pearl_necklace_kanim",
				4,
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
