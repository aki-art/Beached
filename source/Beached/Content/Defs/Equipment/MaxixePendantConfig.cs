﻿using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class MaxixePendantConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_MaxixePendant";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20),
				new(Db.Get().Attributes.AirConsumptionRate.Id, -0.05f),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				Elements.pearl,
				30f,
				"beached_maxixe_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.MAXIXE,
				"beached_maxixe_necklace_kanim",
				4,
				attributeModifiers,
				height: 0.25f,
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
