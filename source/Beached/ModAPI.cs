using Beached.Content;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached
{
	/* Collection of methods which is promised to not change signature or be deleted, allowing 
     * relatively easy integration.
     * All of these are expected to be called on OnAllModsLoaded, unless the comment says otherwise
     * If you need any assistance, or would like to request more methods, you can open an issue on Github: https://github.com/aki-art/Beached/issues
     * 
     * [Permissions]
     * You have permission to patch any methods and modify behaviour of Beached if you need to;
     * but please print a message to the log, eg. "MySuperCoolModXX has patched Beached method Mod.OnLoadMod, to add more unicorns.".
     * This is to keep things clear and make debugging easier.
     * 
     * [Assets]
     * You are free to reuse and edit assets for add-ons or companion content created for Beached. For any other use and more info, 
     * please refer to my Github page for licensing info.
     */

	public class ModAPI
	{
		/// <summary>
		/// Get all possible life goal traits. Leaving <c>category</c> to be <c>null</c> will return all trait ids.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="logWarning"></param>
		/// <returns></returns>
		public static List<string> GetPossibleLifegoalTraits(string category, bool logWarning)
		{
			if (!category.IsNullOrWhiteSpace())
			{
				if (BTraits.LIFE_GOAL_CATEGORIES.TryGetValue(category, out var list))
					return list;
				else if (logWarning)
				{
					Log.Warning($"No life goal category by ID {category}");
					return null;
				}
			}

			return BTraits.LIFE_GOALS;
		}

		/// <summary>
		/// Get the currently rolled life goal attributes for a duplicant before printing
		/// </summary>
		/// <param name="minionStartingStats"></param>
		/// <returns></returns>
		public static Dictionary<string, int> GetLifeGoalPoints(MinionStartingStats minionStartingStats)
		{
			return minionStartingStats.GetLifeGoalAttributes();
		}

		/// <summary>
		/// Returns if the user has enabled using life goals globally, or a Beached world is being played.
		/// </summary>
		public static bool IsUsingLifeGoals() => Mod.settings.CrossWorld.LifeGoals || Beached_WorldLoader.Instance.IsBeachedContentActive;

		public static Klei.AI.Trait GetCurrentLifeGoal(MinionStartingStats minionStartingStats)
		{
			return minionStartingStats.GetExtension()?.lifeGoalTrait;
		}

		public static void RemoveLifeGoal(MinionStartingStats minionStartingStats)
		{
			var ext = minionStartingStats.GetExtension();

			if (ext == null)
				return;

			ext.lifeGoalAttributes?.Clear();
			ext.SetLifeGoal(null);
		}

		/// <summary>
		/// Applies a life goal based on personality.
		/// </summary>
		public static void ApplyLifeGoalFromPersonality(MinionStartingStats minionStartingStats, bool force)
		{
			if (force || IsUsingLifeGoals())
			{
				var trait = BTraits.GetGoalForPersonality(minionStartingStats.personality);
				SetLifeGoal(minionStartingStats, trait, force);
			}
		}

		/// <summary>
		/// Get the expected life goal trait for a personality
		/// </summary>
		public static Klei.AI.Trait GetLifeGoalFromPersonality(Personality personality)
		{
			return BTraits.GetGoalForPersonality(personality);
		}

		/// <summary>
		/// Set a specific life goal
		/// </summary>
		public static void SetLifeGoal(MinionStartingStats minionStartingStats, string traitId, bool force)
		{
			var trait = Db.Get().traits.TryGet(traitId);
			if (trait == null) return;

			SetLifeGoal(minionStartingStats, trait, force);
		}

		public static void SetLifeGoal(MinionStartingStats minionStartingStats, Klei.AI.Trait trait, bool force)
		{
			if (force || IsUsingLifeGoals())
			{
				var ext = minionStartingStats.GetExtension();

				if (ext == null)
					return;

				ext.SetLifeGoal(trait);

				// Always add 2 morale
				ext.AddLifeGoalReward(Db.Get().Attributes.QualityOfLife.Id, CONSTS.DUPLICANTS.LIFEGOALS.MORALBONUS);

				// Add 3-5 more of their already present aptitudes
				foreach (var aptitude in minionStartingStats.skillAptitudes)
				{
					foreach (var skill in aptitude.Key.relevantAttributes)
					{
						ext.AddLifeGoalReward(skill.Id, UnityEngine.Random.Range(3, 6));
					}
				}
			}
		}

		/// <summary>
		/// Add a new filter rule for genetics samplers and the DNA injector building.
		/// </summary>
		/// <param name="ruleID">An ID, needs to be unique.</param>
		/// <param name="testFn">Applies to the adult critter the egg comes from.</param>
		public static void AddGeneticsEggRule(string ruleID, Func<GameObject, bool> testFn)
		{
			Content.Tuning.Genetics.rules.Add(ruleID, testFn);
		}

		public static void AddGeneticsSamplerConfig(string traitId, Color color, string largeIcon, string ruleID)
		{

		}

		/// <summary>
		/// Make this prefab accept Mucus upgrading. (Doors are automatically recognized, no need to add them separately here,
		/// unless you want to override the values or add custom behavior.)
		/// Modify Beached_Effect_Lubricated effect, <see cref="BEffects.LUBRICATED"> or check for OnStorageChange event with Mucus in storage to handle custom behavior.
		/// Overrides existing configurations, if any existed.
		/// </summary>
		/// <param name="prefab">The building template to apply to.</param>
		/// <param name="mucusStorageCapacityKg"></param>
		/// <param name="kgUsedEachTime"></param>
		/// <param name="isTimedUse">Count down every 200ms while in use. Each time the mass will be removed.</param>
		/// <returns>storage component for mucus</returns>
		public static Storage ExtendPrefabToLubricatable(GameObject prefab, float mucusStorageCapacityKg, float kgUsedEachTime, bool isTimedUse)
		{
			return Lubricatable.ConfigurePrefab(prefab, mucusStorageCapacityKg, kgUsedEachTime, isTimedUse).mucusStorage;
		}

		/// <summary>
		/// Add a new slagmite shell drop possibility when mined. generally common metal ores.
		/// </summary>
		/// <param name="tag">The item to drop</param>
		/// <param name="amount">Mass in kg</param>
		/// <param name="weight">Weight of chance to drop.</param>
		/// <param name="condition">evaluate this before picking a reward. object data is the mite itself in this case</param>
		/// <param name="overrideExistingEntry">if this tag is already dropped, override the values here.</param>
		/// <param name="mergeExistingCondition">only relevant if `overrideExistingEntry` is set to true; in case of overriding, should we keep the original condition too.</param>
		public static void AddSlagmiteShellDrop(string tag, float amount, float weight, Func<object, bool> condition, bool overrideExistingEntry, bool mergeExistingCondition)
		{
			if (BDb.lootTables == null)
			{
				Log.Warning("Trying to add shell drops too early, Db not initialized yet!");
				return;
			}

			var overriddenExisting = false;

			foreach (var existingEntry in BDb.lootTables.slagmiteShellDrops.options)
			{
				if (existingEntry.item.tag == tag)
				{
					if (overrideExistingEntry)
					{
						existingEntry.item.mass = amount;
						existingEntry.weight = weight;

						if (mergeExistingCondition)
							existingEntry.condition += condition;
						else
							existingEntry.condition = condition;
					}

					overriddenExisting = true;
				}
			}

			if (!overriddenExisting)
				BDb.lootTables.slagmiteShellDrops.Add(new LootTable<LootTables.MaterialReward>.WeightedOption()
				{
					item = new LootTables.MaterialReward(tag, amount),
					condition = condition,
					weight = weight
				});
		}

		/// <summary>
		/// Makes an item smokeable.
		/// </summary>
		/// <param name="tag">The raw item to smoke.</param>
		/// <param name="smokedTag">The smoked form.</param>
		public static void AddSmokeableFood(string tag, string smokedTag, float timeInCycles)
		{
			AddSmokeableFood(tag, smokedTag, timeInCycles, SmokeCookable.DEFAULT_TEMPERATURE);
		}

		/// <summary>
		/// Makes an item smokeable with custom temperature.
		/// </summary>
		/// <param name="tag">The raw item to smoke.</param>
		/// <param name="smokedTag">The smoked form.</param>
		/// <param name="requiredTemperatureKelvin">The minimum temperature to begin smoking.</param>
		public static void AddSmokeableFood(string tag, string smokedTag, float timeInCycles, float requiredTemperatureKelvin)
		{
			SmokeCookable.smokables[tag] = new SmokeCookable.SmokableInfo(smokedTag, timeInCycles, requiredTemperatureKelvin);
		}

		/// <summary>
		/// Removes an item from being smokeable
		/// </summary>
		/// <param name="tag"></param>
		public static void RemoveSmokeableFood(string tag) => SmokeCookable.smokables.Remove(tag);

		/// <summary>
		/// Add a fur emitter component to this critter
		/// </summary>
		/// <param name="prefabID"></param>
		public static void RegisterFurEmitter(string prefabID) => FurSource.furries.Add(prefabID);

		/// <summary>
		/// Add a new plushie type. Call in Db.Initialize Postfix!
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="animFile"></param>
		/// <param name="effectId">This effect gets applied to the dupe when sleeping with this plushie. null for none</param>
		/// <param name="offset">offset of the anim file for the bed. use debug tool Mods/Beached/Debug for help.</param>
		/// <param name="onSleepingWithPlush">Called when a dupe goes to sleep with this plushie. The GameObject is the dupe. null for none</param>
		/// <param name="canPlaceOnBed">Provides a Sleepable as a parameter, this decides if a plushie can be rolled for the specific bed.</param>
		public static void RegisterPlushie(
			string ID,
			string name,
			string animFile,
			string effectId,
			Action<GameObject> onSleepingWithPlush,
			Func<GameObject, bool> canPlaceOnBed,
			Vector3 offset)
		{
			var plushie = BDb.plushies.Add(ID, name, animFile, effectId, offset);
			if (onSleepingWithPlush != null)
				plushie.OnSleptWith((GameObject go) => onSleepingWithPlush(go));

			if (canPlaceOnBed != null)
				plushie.PlaceCheck((GameObject go) => canPlaceOnBed(go));
		}

		/// <summary>
		/// Set how much this element is affected by Sulfuric Acid, 0-1
		/// The default is 0.5 (slow reaction)
		/// </summary>
		/// <param name="elementId"></param>
		/// <param name="value">
		/// 0 = unaffected
		/// 1 = violent reaction</param>
		public static void SetElementAcidVulnerability(string elementId, float value)
		{
			var element = ElementLoader.FindElementByName(elementId);
			if (element != null)
				SetElementAcidVulnerability(element.id, value);
			else if (Mod.debugMode)
				Log.Warning("No element with ID " + elementId);
		}

		/// <summary>
		/// Set how much this element is affected by Sulfuric Acid, 0-1
		/// Note: explosions happen to elements with Metal tag that have > 0 vulnerability.
		/// The default is 0.05 (very slowly chewed away).
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value">
		/// 0 = unaffected
		/// 1 = nearly instant destruction</param>
		public static void SetElementAcidVulnerability(SimHashes id, float value)
		{
			value = Mathf.Clamp01(value);
			Elements.acidVulnerabilities[id] = value;
		}

		/// <summary>
		/// Can be called at prefab creation or at Db Init postfix
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="levelCount"></param>
		/// <param name="defaultGrowthRate"></param>
		/// <param name="itemDroppedOnShear"></param>
		/// <param name="massDropped"></param>
		/// <param name="diseaseCount"></param>
		/// <param name="diseaseIdx">Provide byte.MaxValue for none</param>
		/// <param name="germPuffCooldown"></param>
		/// <param name="targetSymbol"></param>
		public static void AddLimpetHost(GameObject prefab, int levelCount, float defaultGrowthRate, Tag itemDroppedOnShear,
			float massDropped, int diseaseCount, byte diseaseIdx, float germPuffCooldown, string targetSymbol, string limpetKanim)
		{
			var host = prefab.AddOrGetDef<LimpetHost.Def>();
			host.maxLevel = levelCount;
			host.defaultGrowthRate = defaultGrowthRate;
			host.itemDroppedOnShear = itemDroppedOnShear;
			host.massDropped = massDropped;
			host.diseaseIdx = diseaseIdx;
			host.diseaseCount = diseaseCount;
			host.germPuffCooldown = germPuffCooldown;
			host.targetSymbol = targetSymbol ?? "beached_limpetgrowth";
			host.limpetKanim = limpetKanim ?? "beached_pincher_limpetgrowth_kanim";
		}
	}
}
