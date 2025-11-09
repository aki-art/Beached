using Klei.AI;
using System;
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

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_pearl_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.PEARL,
				Elements.pearl,
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
