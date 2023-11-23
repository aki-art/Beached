using HarmonyLib;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace Beached.Utils
{
	public class TraitBuilder
	{
		private Trait trait;

		public class TraitValBuilder
		{
			string id;
			int rarity = DUPLICANTSTATS.RARITY_COMMON;
			int impact = 0;
			int statBonus = 0;
			string dlcId = DlcManager.VANILLA_ID;
			List<string> mutuallyExclusiveTraits;
			List<HashedString> mutuallyExclusiveAptitudes;

			public TraitValBuilder(string id)
			{
				this.id = id;
			}

			public TraitValBuilder StatBonus(int statBonus)
			{
				this.statBonus = statBonus;
				return this;
			}

			public TraitValBuilder Rarity(int rarity)
			{
				this.rarity = rarity;
				return this;
			}

			public TraitValBuilder ExclusiveWithTraits(params string[] traits)
			{
				mutuallyExclusiveTraits = traits.ToList();
				return this;
			}

			public TraitValBuilder ExclusiveWithAptitudes(params HashedString[] traits)
			{
				mutuallyExclusiveAptitudes = traits.ToList();
				return this;
			}

			public TraitValBuilder Dlc(string dlcId)
			{
				this.dlcId = dlcId;
				return this;
			}

			public TraitValBuilder Impact(int impact)
			{
				this.impact = impact;
				return this;
			}

			public DUPLICANTSTATS.TraitVal Build(List<DUPLICANTSTATS.TraitVal> traitsList)
			{
				var item = new DUPLICANTSTATS.TraitVal()
				{
					id = id,
					mutuallyExclusiveAptitudes = mutuallyExclusiveAptitudes,
					mutuallyExclusiveTraits = mutuallyExclusiveTraits,
					dlcId = dlcId,
					impact = impact,
					rarity = rarity,
					statBonus = statBonus,
				};

				traitsList.Add(item);
				return item;
			}
		}


		public TraitBuilder(string ID, bool positive, string groupID = null)
		{
			var name = Strings.Get($"Beached.STRINGS.DUPLICANTS.TRAITS.{ID.ToUpperInvariant()}.NAME");
			var desc = Strings.Get($"Beached.STRINGS.DUPLICANTS.TRAITS.{ID.ToUpperInvariant()}.DESC");

			var trait = Db.Get().CreateTrait(
				ID,
				name,
				desc,
				groupID,
				true,
				null,
				positive,
				true);

			this.trait = trait;
		}

		public TraitValBuilder AddToTraits()
		{
			var traitVal = new TraitValBuilder(trait.Id);

			return traitVal;
		}

		public TraitBuilder NotStarter()
		{
			trait.ValidStarterTrait = false;
			return this;
		}

		public TraitBuilder Modifier(string ID, float value, bool isMultiplier = false)
		{
			trait.Add(new AttributeModifier(
				ID,
				value,
				trait.Name,
				isMultiplier));

			return this;
		}

		public TraitBuilder OnAdd(Action<GameObject> action)
		{
			trait.OnAddTrait += action;
			return this;
		}

		public TraitBuilder Tag(Tag tag)
		{
			trait.OnAddTrait += go => go.AddTag(tag);
			return this;
		}

		public TraitBuilder ExtendedTooltip(Func<string> fn)
		{
			trait.ExtendedTooltip += fn;
			return this;
		}

		public TraitBuilder DisableChoreGroup(string choreGroup)
		{
			trait.disabledChoreGroups ??= new ChoreGroup[1];
			trait.disabledChoreGroups = trait.disabledChoreGroups.AddToArray(Db.Get().ChoreGroups.Get(choreGroup));

			Log.Debug("Added disabled choregroup");
			Log.Debug((trait.disabledChoreGroups != null).ToString());
			foreach (var cg in trait.disabledChoreGroups)
			{
				Log.Debug($"is null? {cg == null}");
				Log.Debug("Disabled: " + cg.Id);
				Log.Debug(cg.Name);
			}

			return this;
		}

		public TraitBuilder DisableChoreGroups(params ChoreGroup[] choreGroups)
		{
			trait.disabledChoreGroups = choreGroups;
			return this;
		}
	}
}
