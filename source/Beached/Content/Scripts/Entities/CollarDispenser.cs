using HarmonyLib;
using ImGuiNET;
using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts.Entities
{
	public class CollarDispenser : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private SimpleFlatFilterable filterable;

		private Dictionary<Tag, int> critterCounts = new();
		private List<CollarWearer> wearers = new();
		private int critterCount;
		private CavityInfo currentCavity;

		[Serialize] public HashSet<Tag> cullableCritters;
		[Serialize] public int limit;
		[Serialize] public bool perCritter;

		public bool HasConfiguration => cullableCritters != null;

		public int GetCritterCount(Tag critter) => critterCounts.GetOrDefault(critter, 0);

		public CollarDispenser()
		{
			limit = 10;
		}

		public override void OnPrefabInit()
		{
			gameObject.AddTag(BTags.FastTrack.registerRoom);

			base.OnPrefabInit();

			Subscribe((int)GameHashes.UpdateRoom, OnUpdateRoom);

		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			critterCount = -1;
			OnUpdateRoom(Game.Instance.roomProber.GetRoomOfGameObject(gameObject));

			if (cullableCritters == null)
			{
				cullableCritters = new();

				foreach (var critter in Assets.GetPrefabsWithComponent<Butcherable>())
				{
					if (critter.HasTag(BTags.Creatures.doNotTargetMeByCarnivores))
						continue;

					cullableCritters.Add(critter.PrefabID());
				}
			}

			filterable.tagOptions = BTags.TagCollections.cullableCreatures;
			filterable.Refresh();
		}

		public bool CanCull(Tag prefabTag)
		{
			Log.Debug("CollarDispenser: checking to cull");

			if (cullableCritters == null)
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
				.Replace("{PermittedCritters}", cullableCritters == null
					? "All"
					: cullableCritters.Join(tag => tag.ProperNameStripLink()))
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
					cullableCritters.Add(HatchConfig.ID);
				else
					cullableCritters.Remove(HatchConfig.ID);
			}

			if (ImGui.Checkbox("Allow Pip", ref allowHatch))
			{
				if (allowPip)
					cullableCritters.Add(SquirrelConfig.ID);
				else
					cullableCritters.Remove(SquirrelConfig.ID);
			}
		}
	}
}
