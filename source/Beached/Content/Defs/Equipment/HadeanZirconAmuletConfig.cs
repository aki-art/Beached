﻿using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class HadeanZirconAmuletConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_HadeanZirconAmulet";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME),
				new(Db.Get().Attributes.Strength.Id, 2, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.zirconiumOre,
				30f,
				TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0,
				CONSTS.SNAPONS.JEWELLERIES.ZIRCON,
				"beached_zircon_necklace_kanim",
				4,
				attributeModifiers,
				additional_tags:
				[
					GameTags.PedestalDisplayable
				]);

			equipmentDef.OnEquipCallBack += eq =>
			{
				Beached_Mod.Instance.rareJewelleryObjectiveComplete = true;
			};

			return equipmentDef;
		}

		public void DoPostConfigure(GameObject go) { }

		[Obsolete]
		public string[] GetDlcIds() => null;
	}
}
