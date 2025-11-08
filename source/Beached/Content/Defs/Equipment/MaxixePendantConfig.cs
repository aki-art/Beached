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

			var equipmentDef = BEntityTemplates.Necklace(
				ID,
				"beached_maxixe_necklace_kanim",
				CONSTS.SNAPONS.JEWELLERIES.MAXIXE,
				Elements.aquamarine,
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
