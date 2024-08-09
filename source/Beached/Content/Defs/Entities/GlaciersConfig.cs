using Beached.Content.Defs.Entities.Critters;
using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities
{
	public class GlaciersConfig : IMultiEntityConfig
	{
		public const float WEIGHT_PER_TILE = 30;

		public const string
			MUFFINS = "Beached_Glacier_Muffins";

		public List<GameObject> CreatePrefabs() => new()
		{
			CreatePrefab(MUFFINS, 7, 3, "beached_glacier_muffins_kanim",
			[
				MuffinConfig.ID,
				MuffinConfig.ID,
			])
		};

		private static GameObject CreatePrefab(string id, int width, int height, string anim, List<string> tags)
		{
			var name = Strings.Get($"STRINGS.ENTITIES.GLACIERS.{id.ToUpperInvariant()}.NAME");
			var description = $"{Strings.Get($"STRINGS.ENTITIES.GLACIERS.{id.ToUpperInvariant()}.DESCRIPTION")}\n\n{STRINGS.ENTITIES.GLACIERS.GENERIC_THAW}";

			var result = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				description,
				width * height * WEIGHT_PER_TILE,
				Assets.GetAnim(anim),
				"idle",
				Grid.SceneLayer.Ore,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2,
				element: SimHashes.Ice,
				additionalTags:
				[
					BTags.glacier
				],
				defaultTemperature: GameUtil.GetTemperatureConvertedToKelvin(-30, GameUtil.TemperatureUnit.Celsius));

			var locker = result.AddComponent<SetLocker>();
			locker.possible_contents_ids =
			[
				[.. tags]
			];

			result.AddComponent<Glacier>();

			return result;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
