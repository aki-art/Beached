using Beached.Content.Scripts;
using Beached.Content.Scripts.ClassExtensions;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Utils
{
	public static class ExtensionMethods
	{
		public static Dictionary<MinionStartingStats, MinionStartingStatsExtension> minionStartingStatsExtensions = new();
		public static Dictionary<CavityInfo, CavityInfoExtension> cavityInfoExtensions = new();

		public static MinionStartingStatsExtension GetExtension(this MinionStartingStats stats)
		{
			if (!minionStartingStatsExtensions.ContainsKey(stats))
			{
				minionStartingStatsExtensions.Add(stats, new MinionStartingStatsExtension(stats));
			}

			return minionStartingStatsExtensions[stats];
		}

		public static Trait GetLifeGoalTrait(this MinionStartingStats stats)
		{
			return GetExtension(stats)?.lifeGoalTrait;
		}

		public static Dictionary<string, int> GetLifeGoalAttributes(this MinionStartingStats stats)
		{
			return GetExtension(stats).lifeGoalAttributes;
		}

		public static List<KPrefabID> GetNaturePOIs(this CavityInfo cavity)
		{
			if (cavityInfoExtensions.TryGetValue(cavity, out var extension))
			{
				return extension.pois;
			}

			return null;
		}
		public static void AddNaturePOI(this CavityInfo cavity, KPrefabID kPrefabID)
		{
			Log.Debug("Added nature poi " + kPrefabID.GetProperName());

			if (!cavityInfoExtensions.ContainsKey(cavity))
			{
				cavityInfoExtensions.Add(cavity, new(cavity));
			}

			cavityInfoExtensions[cavity].pois.Add(kPrefabID);
		}

		public static void RemoveNaturePOI(this CavityInfo cavity, KPrefabID kPrefabID)
		{
			if (cavityInfoExtensions.TryGetValue(cavity, out var cavityInfoExtension))
			{
				cavityInfoExtension.pois.Remove(kPrefabID);
			}
		}

		public static void AddBTag(this GameObject gameObject, Tag tag)
		{
			var beachedPrefabId = gameObject.TryGetComponent(out BeachedPrefabID result) ? result : gameObject.AddComponent<BeachedPrefabID>();
			beachedPrefabId.AddTag(tag);
		}

		public static void RemoveBTag(this GameObject gameObject, Tag tag)
		{
			if (gameObject.TryGetComponent(out BeachedPrefabID beachedPrefabId))
			{
				beachedPrefabId.RemoveTag(tag);
			}
		}

		public static bool HasBTag(this GameObject gameObject, Tag tag)
		{
			if (gameObject.TryGetComponent(out BeachedPrefabID beachedPrefabId))
			{
				return beachedPrefabId.HasTag(tag);
			}

			return false;
		}
	}
}
