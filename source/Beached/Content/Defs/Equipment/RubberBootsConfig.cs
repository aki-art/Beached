using Beached.Content.ModDb;
using UnityEngine;

namespace Beached.Content.Defs.Equipment
{
	public class RubberBootsConfig : IEquipmentConfig
	{
		public const string ID = "Beached_Equipment_RubberBoots";

		public EquipmentDef CreateEquipmentDef()
		{
			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.SHOES_ID,
				Elements.rubber,
				30f,
				"beached_rubberboots_item_kanim",
				CONSTS.SNAPONS.RUBBER_BOOTS,
				"beached_rubberboots_kanim",
				4,
				[],
				additional_tags:
				[
					GameTags.PedestalDisplayable
				]);

			var clothingInfo = ClothingWearer.ClothingInfo.BASIC_CLOTHING;

			equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
			equipmentDef.OnEquipCallBack = eq => ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
			equipmentDef.OnUnequipCallBack = ClothingWearer.ClothingInfo.OnUnequipVest;

			return equipmentDef;
		}

		public void DoPostConfigure(GameObject go)
		{
		}

		public string[] GetDlcIds() => null;
	}
}
