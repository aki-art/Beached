using Beached.Content.ModDb;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ClothingWearer;

namespace Beached.Content.Defs.Equipment
{
	public class BeachShirtConfig : IEquipmentConfig
	{
		public const string ID = "Beached_BeachShirt";
		public static ClothingInfo clothingInfo = new(
			STRINGS.EQUIPMENT.PREFABS.BEACHED_BEACHSHIRT.NAME,
			10,
			0f,
			0f);

		public EquipmentDef CreateEquipmentDef()
		{
			var attributeModifiers = new List<AttributeModifier>
			{
				new(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.DECOR, 20),
				new(BAttributes.HEAT_RESISTANCE_ID, 0.9f, is_multiplier: true),
			};

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				TUNING.EQUIPMENT.CLOTHING.SLOT,
				SimHashes.Carbon,
				TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_MASS,
				"beached_beachshirt_green_item_kanim",
				TUNING.EQUIPMENT.VESTS.SNAPON0,
				"beached_beachshirt_green_kanim",
				4,
				attributeModifiers,
				TUNING.EQUIPMENT.VESTS.SNAPON1,
				true,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.75f,
				0.4f,
				additional_tags: [BTags.comfortableClothing]);

			var DESC = "{0}: {1}";

			var thermal = string.Format(DESC, global::STRINGS.DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME,
				GameUtil.GetFormattedDistance(clothingInfo.conductivityMod));

			var decor = string.Format(DESC, global::STRINGS.DUPLICANTS.ATTRIBUTES.DECOR.NAME, clothingInfo.decorMod);

			equipmentDef.additionalDescriptors.Add(new Descriptor(thermal, thermal));
			equipmentDef.additionalDescriptors.Add(new Descriptor(decor, decor));

			equipmentDef.RecipeDescription = global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC;

			equipmentDef.OnEquipCallBack = OnEquip;
			equipmentDef.OnUnequipCallBack = OnUnEquip;

			foreach (var item in Db.GetEquippableFacades().resources)
			{
				if (item.DefID == ID)
					TagManager.Create(item.Id, EquippableFacade.GetNameOverride(ID, item.Id));
			}

			return equipmentDef;
		}

		private void OnEquip(Equippable equippable)
		{
			ClothingInfo.OnEquipVest(equippable, clothingInfo);
		}

		private void OnUnEquip(Equippable equippable)
		{
			ClothingInfo.OnUnequipVest(equippable);
		}

		public void DoPostConfigure(GameObject go)
		{
			CustomClothingConfig.SetupVest(go);
			go.AddTag(GameTags.PedestalDisplayable);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;
	}
}
