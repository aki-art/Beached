using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	[EntityConfigOrder(0)]
	public static class BaseJellyfishConfig
	{
		public static GameObject CreatePrefab(string id, string name, string desc, string anim_file, string traitId, string symbolOverridePrefix)
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				name,
				desc,
				JellyfishTuning.MASS,
				Assets.GetAnim(anim_file),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				2,
				DECOR.NONE); // there will be many, in water, so better to now waste perf.

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				FactionManager.FactionID.Friendly,
				traitId,
				CONSTS.NAV_GRID.SWIMMER,
				NavType.Swim,
				16,
				0.25f,
				JellyfishTuning.ON_DEATH_DROP,
				1,
				false,
				false,
				288.15f,
				343.15f,
				243.15f,
				373.15f);

			prefab.AddTag(GameTags.Creatures.Swimmer);
			prefab.AddTag(GameTags.Creatures.CrabFriend);
			prefab.AddTag(GameTags.Amphibious);

			JellyfishBrain.ConfigureAI(prefab, symbolOverridePrefix, BTags.Species.snail);

			return prefab;
		}
	}
}

