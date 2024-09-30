using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities.Critters.Jellies;
using Beached.Content.Defs.Entities.Critters.Muffins;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Database;
using ProcGenGame;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
    public class Beached_WorldLoader : KMonoBehaviour
	{
		public static Beached_WorldLoader Instance;

		public override void OnPrefabInit()
		{
			Instance = this;
			SaveLoader.Instance.OnWorldGenComplete += OnWorldgenComplete;
		}

		private void OnWorldgenComplete(Cluster cluster)
		{

		}

		public override void OnCleanUp() => Instance = null;

		public bool IsBeachedContentActive { get; private set; } = true;

		private static readonly List<string> extraMobsToFind = [
			SlickShellConfig.ID,
			MuffinConfig.ID,
			JellyfishConfig.ID
		];

		private static readonly List<string> mobsToNotFind = [
			DreckoConfig.ID,
			OilFloaterConfig.ID,
			MoleConfig.ID
		];

		public void WorldLoaded(string clusterId)
		{
			IsBeachedContentActive = clusterId == CONSTS.WORLDGEN.CLUSTERS.BEACHED;

			if (IsBeachedContentActive)
				Log.Info("Loaded Astropelagos world, initializing Beached settings.");

			Elements.OnWorldReload(IsBeachedContentActive);
			ZoneTypes.OnWorldLoad();

			var tameCrittersAchievement = Db.Get().ColonyAchievements.TameAllBasicCritters;
			foreach (var item in tameCrittersAchievement.requirementChecklist)
			{
				if (item is CritterTypesWithTraits critterTypesWithTraits)
				{
					if (IsBeachedContentActive)
					{
						foreach (var mob in extraMobsToFind)
							critterTypesWithTraits.critterTypesToCheck.GetOrAdd(mob, () => false);
						foreach (var mob in mobsToNotFind)
							critterTypesWithTraits.critterTypesToCheck.Remove(mob);
					}
				}

			}
		}
	}
}
