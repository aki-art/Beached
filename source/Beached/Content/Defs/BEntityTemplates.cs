using Beached.Content.Scripts.Entities.Comets;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs
{
	internal class BEntityTemplates
	{
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
			kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim(animName) };
			kBatchedAnimController.isMovable = true;
			kBatchedAnimController.initialAnim = "fall_loop";
			kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kBatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

			gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
			gameObject.transform.localScale = new Vector3(size, size, 1f);
			gameObject.AddTag(GameTags.Comet);

			return gameObject;
		}
	}
}
