using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities;
using Beached.Content.Scripts.Entities.Comets;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs
{
	public class BEntityTemplates
	{
		public const string PLACEHOLDER_KANIM = "farmtile_kanim";

		public static GameObject CreateSimpleItem(string ID, string anim, EffectorValues decor, SimHashes element = SimHashes.Creature, float width = 0.66f, float height = 0.75f, bool loop = false)
		{
			var name = Strings.Get($"STRINGS.ITEMS.MISC.{ID.ToUpperInvariant()}.NAME");
			var desc = Strings.Get($"STRINGS.ITEMS.MISC.{ID.ToUpperInvariant()}.DESC");

			return CreateSimpleItem(ID, name, desc, anim, decor, element, width, height, loop);
		}

		public static EquipmentDef Necklace(string ID, string anim, string snapOn, SimHashes element, List<AttributeModifier> attributeModifiers = null)
		{

			var equipmentDef = EquipmentTemplates.CreateEquipmentDef(
				ID,
				BAssignableSlots.JEWELLERY_ID,
				element,
				30f,
				anim,
				snapOn,
				anim,
				4,
				attributeModifiers,
				width: 0.7f,
				height: 0.27f,
				CollisionShape: EntityTemplates.CollisionShape.RECTANGLE,
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

		public static void SetupJewelleryPost(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
			go.AddOrGet<Equippable>().SetQuality(QualityLevel.Good);
			go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
		}

		public static Diet.Info[] SimpleDiet(Tag from, Tag to, float kcalPerKg, float rate = 0.5f)
		{
			return SimpleDiet([from], to, kcalPerKg, rate);
		}

		public static Diet.Info[] SimpleDiet(HashSet<Tag> from, Tag to, float kcalPerKg, float rate = 0.5f)
		{
			return
			[
				new(
					from,
					to,
					kcalPerKg,
					rate,
					null,
					0)
			];
		}

		public static GameObject ExtendPlantToSelfIrrigated(GameObject prefab, PlantElementAbsorber.ConsumeInfo consumeInfo)
		{
			EntityTemplates.ExtendPlantToIrrigated(prefab, [consumeInfo]);

			prefab.AddOrGet<SelfIrrigator>();

			var passiveElementConsumer = prefab.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = ElementLoader.GetElementID(consumeInfo.tag);
			passiveElementConsumer.consumptionRate = consumeInfo.massConsumptionRate;
			passiveElementConsumer.capacityKG = consumeInfo.massConsumptionRate * CONSTS.CYCLE_LENGTH * 3.0f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;

			passiveElementConsumer.EnableConsumption(false);

			return prefab;
		}

		public static GameObject CreateSimpleItem(string ID, string name, string description, string anim, EffectorValues decor, SimHashes element = SimHashes.Creature, float width = 0.66f, float height = 0.75f, bool loop = false)
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				name,
				description,
				1f,
				false,
				Assets.GetAnim(anim),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.66f,
				0.75f,
				true,
				0,
				element,
				additionalTags:
				[
					BTags.MaterialCategories.crystal,
					GameTags.PedestalDisplayable
				]);

			prefab.AddOrGet<EntitySplitter>();
			prefab.AddOrGet<SimpleMassStatusItem>();
			prefab.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
			prefab.AddOrGet<DecorProvider>().SetValues(decor);

			if (loop)
			{
				prefab.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
			}

			return prefab;
		}

		public static GameObject CreateFood(
			string ID,
			string anim,
			float width,
			float height,
			FoodInfo foodInfo)
		{
			var name = Strings.Get($"STRINGS.ITEMS.FOOD.{ID.ToUpperInvariant()}.NAME");
			var desc = Strings.Get($"STRINGS.ITEMS.FOOD.{ID.ToUpperInvariant()}.DESC");

			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				name,
				desc,
				1f,
				false,
				Assets.GetAnim(anim),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				width,
				height,
				true);

			var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

			return gameObject;
		}

		public static GameObject SparkleComet(string id, string name, string animName, SimHashes primaryElement, Vector2 massRange, Vector2 temperatureRange, string impactSound = "Meteor_Large_Impact", int flyingSoundID = 1, SimHashes exhaustElement = SimHashes.CarbonDioxide, SpawnFXHashes explosionEffect = SpawnFXHashes.None, float size = 1f)
		{
			var gameObject = EntityTemplates.CreateEntity(id, name);
			gameObject.AddOrGet<SaveLoadRoot>();
			gameObject.AddOrGet<LoopingSounds>();

			var comet = gameObject.AddOrGet<SparkleComet>();
			comet.massRange = massRange;
			comet.temperatureRange = temperatureRange;
			comet.explosionTemperatureRange = comet.temperatureRange;
			comet.impactSound = impactSound;
			comet.flyingSoundID = flyingSoundID;
			comet.EXHAUST_ELEMENT = exhaustElement;
			comet.explosionEffectHash = explosionEffect;

			var primaryElement2 = gameObject.AddOrGet<PrimaryElement>();
			primaryElement2.SetElement(primaryElement);
			primaryElement2.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;

			var kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kBatchedAnimController.AnimFiles = [Assets.GetAnim(animName)];
			kBatchedAnimController.isMovable = true;
			kBatchedAnimController.initialAnim = "fall_loop";
			kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kBatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

			gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
			gameObject.transform.localScale = new Vector3(size, size, 1f);
			gameObject.AddTag(GameTags.Comet);

			return gameObject;
		}

		public static GameObject CreateAndRegisterClusterForCrystal(
			GameObject crystal,
			string id,
			KAnimFile anim,
			string initialAnim = "object",
			float kgPerHarvest = 30f,
			List<Tag> additionalTags = null,
			Tag replantGroundTag = default,
			int sortOrder = 0,
			EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE,
			float width = 0.25f,
			float height = 0.25f,
			string[] dlcIds = null)
		{
			var name = Strings.Get($"STRINGS.ENTITIES.BEACHED_CRYSTALS.{id.ToUpperInvariant()}.NAME");
			var desc = Strings.Get($"STRINGS.ENTITIES.BEACHED_CRYSTALS.{id.ToUpperInvariant()}.DESCRIPTION");

			var prefab = EntityTemplates.CreateLooseEntity(
				id,
				name,
				desc,
				1f,
				true,
				anim,
				initialAnim,
				Grid.SceneLayer.Front,
				collisionShape,
				width,
				height,
				true,
				1000 + sortOrder);

			prefab.AddOrGet<EntitySplitter>();

			var cluster = prefab.AddOrGet<CrystalCluster>();
			cluster.crystalId = crystal.PrefabID();

			var kprefabId = prefab.GetComponent<KPrefabID>();

			if (additionalTags != null)
			{
				foreach (var additionalTag in additionalTags)
					kprefabId.AddTag(additionalTag);
			}

			kprefabId.requiredDlcIds = dlcIds ?? null;
			kprefabId.AddTag(BTags.crystalCluster);
			kprefabId.AddTag(GameTags.PedestalDisplayable);

			Assets.AddPrefab(kprefabId);

			return prefab;
		}

		public static GameObject CreateAndRegisterPreviewForCluster(
			GameObject cluster,
			string id,
			KAnimFile anim,
			string initialAnim = "crystal_place",
			int width = 1,
			int height = 1)
		{
			var previewGo = EntityTemplates.CreateAndRegisterPreview(id, anim, initialAnim, ObjectLayer.Building, width, height);
			cluster.AddOrGet<CrystalCluster>().previewID = TagManager.Create(id);

			return previewGo;
		}

	}
}
