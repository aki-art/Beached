using System;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class SulfurGlandConfig : IEntityConfig
	{
		public const string ID = "Beached_SulfurGland";

		public GameObject CreatePrefab()
		{
			return BEntityTemplates.CreateSimpleItem(
				ID,
				"beached_sulfurgland_kanim",
				TUNING.DECOR.NONE,
				SimHashes.Sulfur,
				0.66f,
				0.66f);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
