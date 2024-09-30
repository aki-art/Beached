using Beached.Content.Scripts.Entities.AI;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters.Jellies
{
	public static class BaseJellyfishConfig
	{
		public const int SORTING_ORDER = 53;
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
				DECOR.NONE);

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

			ConfigureAI(prefab, symbolOverridePrefix, BTags.Species.snail);

			return prefab;
		}

		public static void ConfigureAI(GameObject gameObject, string symbolOverridePrefix, Tag species)
		{
			var germSuckDef = new GermSuckStates.Def()
			{
			};

			var choreTable = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				//.Add(new GrowUpStates.Def())
				.Add(new TrappedStates.Def())
				//.Add(new IncubatingStates.Def())
				.Add(new BaggedStates.Def())
				//.Add(new FallStates.Def())
				//.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				//.Add(new DefendStates.Def())
				//.Add(new AttackStates.Def("eat_pre", "eat_pst", null))
				.PushInterruptGroup()
				//.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				//.Add(new RanchedStates.Def())
				//.Add(new LayEggStates.Def())
				.Add(germSuckDef)
				//.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CallAdultStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(gameObject, choreTable, species, symbolOverridePrefix);
		}
	}
}

