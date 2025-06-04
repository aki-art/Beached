using HarmonyLib;
using ImGuiNET;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class CollarDispenser : KMonoBehaviour, IImguiDebug
	{
		public static List<Tag> cullables;

		private Dictionary<Tag, int> critterCounts = [];
		private List<CollarWearer> wearers = [];
		private int critterCount;
		private CavityInfo currentCavity;

		[Serialize] public int limit;
		[Serialize] public bool perCritter;
		[Range(0, 100)][Serialize] public float minAge;
		[Serialize] public bool ignoreEggs;
		[Serialize] public bool ignoreNamed;

		[Serialize] private List<Tag> filteredCrittersSerialized;
		[Serialize] private List<int> filteredCritterCountsSerialized;

		public Dictionary<Tag, int> filteredCritters;

		[Serialize] public int defaultCritterCount;
		[Serialize] public DefaultBeheavior defaultBehavior;

		public enum DefaultBeheavior
		{
			Count,
			Hunt,
			Ignore
		}

		public bool HasConfiguration => cullables != null;

		public int GetCritterCount(Tag critter) => critterCounts.GetOrDefault(critter, 0);

		public CollarDispenser()
		{
			limit = 10;
			minAge = 0.50f;
			ignoreNamed = true;
		}

		[OnDeserialized]
		private void OnDeserialized()
		{
			filteredCritters = [];

			if (filteredCrittersSerialized == null)
				return;

			for (int i = 0; i < filteredCrittersSerialized.Count; i++)
			{
				var tag = filteredCrittersSerialized[i];
				var count = filteredCritterCountsSerialized[i];

				if (filteredCritters.ContainsKey(tag))
				{
					Log.Warning($"Duplicate entry: {tag}");
					continue;
				}

				filteredCritters[tag] = count;
			}
		}

		[OnSerializing]
		private void OnSerializing()
		{
			filteredCrittersSerialized = [];
			filteredCritterCountsSerialized = [];

			foreach (var item in filteredCritters)
			{
				filteredCrittersSerialized.Add(item.Key);
				filteredCritterCountsSerialized.Add(item.Value);
			}
		}

		public void SetFilter(Tag tag, int count)
		{
			Log.Debug($"SetFilter : {tag} {count}");
			if (!tag.IsValid)
			{
				Log.Warning("invalid filter");
				return;
			}

			count = Mathf.Clamp(count, 0, 999);

			if (!filteredCritters.ContainsKey(tag))
				filteredCritters.Add(tag, count);
			else
				filteredCritters[tag] = count;
		}

		public void RemoveFilter(Tag tag)
		{
			Log.Debug($"RemoveFilter {tag}");
			filteredCritters.Remove(tag);
			RefreshTargets();
		}

		public bool IsAcceptedTag(Tag tag)
		{
			Log.Debug($"is accepted tag? : {tag} {(!filteredCritters.ContainsKey(tag))}");
			return !filteredCritters.ContainsKey(tag);
		}

		public override void OnPrefabInit()
		{
			gameObject.AddTag(BTags.FastTrack_registerRoom);

			base.OnPrefabInit();

			Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			critterCount = -1;
			OnUpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));

			if (cullables == null)
			{
				cullables = [];

				foreach (var critter in Assets.GetPrefabsWithComponent<Butcherable>())
				{
					// disregard specifically disabled critters, such as Makis or other Muffins
					if (critter.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
						continue;

					// disregard unnaturally long lifespan critters, such as GlitterPufts or Suspicious Pips
					var age = Db.Get().Amounts.Age.Lookup(critter);
					if (age != null && age.GetMax() > 1200)
						continue;

					cullables.Add(critter.PrefabID());
				}

				foreach (var egg in Assets.GetPrefabsWithTag(GameTags.IncubatableEgg))
				{
					if (egg.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
						continue;

					cullables.Add(egg.PrefabID());
				}

				cullables = [.. cullables.OrderBy(c => c.ProperNameStripLink())];
			}

			filteredCritterCountsSerialized ??= [];
			filteredCrittersSerialized ??= [];
			filteredCritters ??= [];
		}

		public bool CanCull(Tag prefabTag)
		{
			Log.Debug("CollarDispenser: checking to cull");

			if (cullables == null)
				return true;

			Log.Debug("CollarDispenser: has rules " + limit);

			var result = !perCritter
				? critterCount > limit
				: critterCounts.GetOrDefault(prefabTag, 0) > limit;

			Log.Debug($"CollarDispenser: Permission {(result ? "granted" : "denied")}: total: {critterCount} {prefabTag}: {critterCounts.GetOrDefault(prefabTag, 0)} limit: {limit}");

			return result;
		}

		private void OnUpdateRoom(object obj)
		{
			Log.Debug("CollarDispenser: On Update room");

			if (obj is Room room)
			{
				UpdateRoom(room?.cavity);

				if (room.cavity.creatures.Count == critterCount)
					return;

				critterCount = 0;
				critterCounts.Clear();

				foreach (var critter in room.cavity.creatures)
				{
					if (critter.TryGetComponent(out CollarWearer collarWearer))
						wearers.Add(collarWearer);

					if (!critter.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
						critterCount++;

					if (!critterCounts.ContainsKey(critter.PrefabTag))
						critterCounts[critter.PrefabTag] = 1;
					else
						critterCounts[critter.PrefabTag]++;
				}
			}
		}

		public void UpdateRoom(CavityInfo cavity)
		{
			if (Game.IsQuitting())
				return;

			if (cavity == currentCavity)
				return;

			RemoveDispenser();

			cavity?.AddCollarDispenser(this);

			currentCavity = cavity;
		}

		private void RemoveDispenser()
		{
			currentCavity?.RemoveCollarDispenser(this);
		}

		public string FormatStatusItemString(string str) => str
				.Replace("{PermittedCritters}", cullables == null
					? "All"
					: cullables.Join(tag => tag.ProperNameStripLink()))
				.Replace("{PerCritter}", perCritter
					? STRINGS.CREATURES.STATUSITEMS.BEACHED_CONTROLLERBYCOLLARDISPENSER.PER_CRITTER
					: STRINGS.CREATURES.STATUSITEMS.BEACHED_CONTROLLERBYCOLLARDISPENSER.GLOBAL)
				.Replace("{MinCount}", limit.ToString());

		private static bool allowHatch;
		public static bool allowPip;

		public void OnImguiDraw()
		{
			if (ImGui.Checkbox("Allow Hatch", ref allowHatch))
			{
				if (allowHatch)
					cullables.Add(HatchConfig.ID);
				else
					cullables.Remove(HatchConfig.ID);
			}

			if (ImGui.Checkbox("Allow Pip", ref allowHatch))
			{
				if (allowPip)
					cullables.Add(SquirrelConfig.ID);
				else
					cullables.Remove(SquirrelConfig.ID);
			}
		}

		public void RefreshTargets()
		{
		}

		internal void SetFilters(Dictionary<Tag, int> filteredCritters)
		{
			this.filteredCritters = filteredCritters;
			RefreshTargets();
		}

		internal void ClearFilters()
		{
			filteredCritters.Clear();
		}

		public Tag GetNewFilterableTag()
		{
			return cullables.Find(f => !filteredCritters.ContainsKey(f));
		}
	}
}
