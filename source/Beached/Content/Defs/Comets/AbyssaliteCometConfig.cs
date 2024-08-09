using UnityEngine;

namespace Beached.Content.Defs.Comets
{
	public class AbyssaliteCometConfig : IEntityConfig
	{
		public const string ID = "Beached_AbyssaliteComet";
		public GameObject CreatePrefab()
		{
			var go = BaseCometConfig.BaseComet(
				ID,
				STRINGS.COMETS.BEACHED_DIAMONDCOMET.NAME,
				"beached_meteor_diamond_kanim",
				SimHashes.Katairite,
				new Vector2(3f, 20f),
				new Vector2(323.15f, 423.15f),
				"Meteor_Medium_Impact",
				1,
				SimHashes.CarbonDioxide,
				SpawnFXHashes.MeteorImpactMetal,
				0.8f);

			var comet = go.GetComponent<Comet>();
			comet.explosionOreCount = new Vector2I(2, 4);
			comet.entityDamage = 30;
			comet.totalTileDamage = 1f;
			comet.affectedByDifficulty = false;

			return go;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
