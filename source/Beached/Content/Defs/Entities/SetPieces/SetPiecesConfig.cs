using Beached.Content.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.SetPieces
{
	public class SetPiecesConfig : IMultiEntityConfig
	{
		public const string TEST = "Beached_TestSetPiece";
		public const string BEACH = "Beached_BeachSetPiece";
		public const string ZEOLITE = "Beached_ZeoliteSetPiece";

		public List<GameObject> CreatePrefabs()
		{
			return new List<GameObject>()
			{
				CreateSetPiece(TEST, 11, 8, "test"),
				CreateSetPiece(BEACH, 17, 10, "beach"),
				CreateTestPiece(ZEOLITE, 8, 8, ModAssets.Textures.Placeholders.zeoliteBg)
			};
		}

		private GameObject CreateTestPiece(string ID, int width, int height, Texture2D texture)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				ID,
				"",
				100f,
				Assets.GetAnim("farmtile_kanim"),
				"",
				Grid.SceneLayer.Backwall,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2,
				additionalTags:
				[
					BTags.setPiece
				]);

			var setPiece = prefab.AddComponent<SetPiece>();
			setPiece.setPiecePrefabID = "test";
			setPiece.placeholderTexture = texture;
			setPiece.width = width;
			setPiece.height = height;

			return prefab;
		}

		private GameObject CreateSetPiece(string ID, int width, int height, string bgPrefabID)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Set Piece",
				"",
				100f,
				Assets.GetAnim("farmtile_kanim"),
				"",
				Grid.SceneLayer.Backwall,
				width,
				height,
				TUNING.DECOR.BONUS.TIER2);

			var setPiece = prefab.AddComponent<SetPiece>();
			setPiece.setPiecePrefabID = bgPrefabID;
			setPiece.width = width;
			setPiece.height = height;

			prefab.AddComponent<Vista>();

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
