using Klei.AI;
using System;
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
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20, STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME),
			};

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_strange_matter_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.STRANGE_MATTER,
				SimHashes.Unobtanium,
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
