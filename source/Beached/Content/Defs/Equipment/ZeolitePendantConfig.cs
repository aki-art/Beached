using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class ZeolitePendantConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_ZeolitePendant";

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20),
				new(Db.Get().Amounts.Stress.deltaAttribute.Id, -2f / CONSTS.CYCLE_LENGTH),
			};

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_zeolite_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.ZEOLITE,
				Elements.zeolite,
				attributeModifiers);

			return equipmentDef;
		}

		public void DoPostConfigure(GameObject go)
		{
			BEntityTemplates.SetupJewelleryPost(go);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;
	}
}
