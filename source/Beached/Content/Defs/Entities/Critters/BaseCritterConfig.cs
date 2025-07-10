using Beached.Content.DefBuilders;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	public abstract class BaseCritterConfig
	{
		public virtual GameObject CreatePrefab(BaseCritterConfig config)
		{
			var builder = new CritterBuilder(Id, AnimFile);
			return ConfigureCritter(builder).Build();
		}

		protected abstract CritterBuilder ConfigureCritter(CritterBuilder builder);

		// Conditions just just a basic strings set where each string acts like a flag.
		protected abstract void ConfigureAI(CritterBuilder.BrainBuilder builder, HashSet<string> conditions);

		protected abstract string AnimFile { get; }

		protected virtual string Id => GetType().GetField("ID").GetValue(this) as string;

		public virtual string EggId => $"{Id}Egg";

		public virtual string BaseTraitId => $"{Id}BaseTrait";

		public GameObject SetupBasicSolidDiet(GameObject prefab, Diet.Info[] dietInfos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			var diet = new Diet(dietInfos);
			var def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			def.diet = diet;
			def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}
	}
}
