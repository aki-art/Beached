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
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_HADEANZIRCONAMULET.NAME),
				new(Db.Get().Attributes.Strength.Id, 2, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_HADEANZIRCONAMULET.NAME),
			};

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_zircon_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.ZIRCON,
				Elements.zirconiumOre,
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
