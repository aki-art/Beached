using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs
{
	// mirrors EffectConfigs
	public class BEffectConfigs : IMultiEntityConfig
	{
		public const string
			MUSSEL_GIBLETS = "Beached_MusselGiblets";

		public List<GameObject> CreatePrefabs()
		{
			var result = new List<GameObject>
			{
				CreatePrefab(MUSSEL_GIBLETS, "beached_mussel_giblets_impact_kanim", "loop", KAnim.PlayMode.Loop, false, true)
			};

			return result;
		}

		private static GameObject CreatePrefab(string id, string animFile, string initialAnim, KAnim.PlayMode initialMode, bool destroyOnAnimComplete, bool addLoopingSounds = false)
		{
			var entity = EntityTemplates.CreateEntity(id, id, false);

			var kbac = entity.AddOrGet<KBatchedAnimController>();
			kbac.materialType = KAnimBatchGroup.MaterialType.Simple;
			kbac.initialAnim = initialAnim;
			kbac.initialMode = initialMode;
			kbac.isMovable = true;
			kbac.destroyOnAnimComplete = destroyOnAnimComplete;
			kbac.AnimFiles = [Assets.GetAnim(animFile)];

			if (addLoopingSounds)
				entity.AddOrGet<LoopingSounds>();

			return entity;
		}

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
