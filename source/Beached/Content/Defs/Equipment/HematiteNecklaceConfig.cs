using Klei.AI;
using System;
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

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_hematite_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.HEMATITE,
				SimHashes.IronOre,
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
