using Beached.Content.Scripts;
using UnityEngine;

namespace Beached.Content.Defs.Entities.SetPieces
{
    public class TestSetPieceConfig : IEntityConfig
    {
        public const string ID = "Beached_TestSetPiece";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                "Set Piece",
                "",
                100f,
                Assets.GetAnim("farmtile_kanim"),
                "",
                Grid.SceneLayer.Backwall,
                11,
                8,
                TUNING.DECOR.BONUS.TIER2);

            prefab.AddComponent<SetPiece>().setPiecePrefabID = "test";

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
