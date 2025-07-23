using Klei.AI;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class SlagWoolConfig : IEntityConfig
	{
		public const string ID = "Beached_SlagWool";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.BEACHED_SLAGWOOL.NAME,
				STRINGS.ITEMS.INDUSTRIAL_INGREDIENTS.BEACHED_SLAGWOOL.DESC,
				1f,
				true,
				Assets.GetAnim("beached_slagwool_kanim"),
				"object",
				Grid.SceneLayer.BuildingBack,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.45f,
				true,
				0,
				Elements.slag,
				[
					GameTags.IndustrialIngredient,
					BTags.BuildingMaterials.slag
				]);

			prefab.AddOrGet<EntitySplitter>();

			var modifiers = prefab.AddOrGet<PrefabAttributeModifiers>();
			modifiers.AddAttributeDescriptor(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, 120));
			modifiers.AddAttributeDescriptor(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -0.25f, is_multiplier: true));

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
