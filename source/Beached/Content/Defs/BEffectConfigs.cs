using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs
{
	// mirrors EffectConfigs
	public class BEffectConfigs : IMultiEntityConfig
	{
		public const string
			MUSSEL_GIBLETS = "Beached_Fx_MusselGiblets",
			SAND = "Beached_Fx_Sand";

		public List<GameObject> CreatePrefabs()
		{
			var result = new List<GameObject>
			{
				CreateGunImpactFx(MUSSEL_GIBLETS, "beached_mussel_giblets_impact_kanim"),
				CreateGunImpactFx(SAND, "beached_sand_impact_kanim"),
			};

			return result;
		}

		private static GameObject CreateGunImpactFx(string id, string animFile, string initialAnim = "loop")
		{
			return CreatePrefab(id, animFile, initialAnim, KAnim.PlayMode.Loop, false, true);
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
