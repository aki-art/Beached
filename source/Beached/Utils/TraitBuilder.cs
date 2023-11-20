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
		private int rarity = DUPLICANTSTATS.RARITY_COMMON;

		public class TraitValBuilder
		{
			public DUPLICANTSTATS.TraitVal val;

			public TraitValBuilder(string id)
			{
				val = new()
				{
					id = id,
					dlcId = DlcManager.VANILLA_ID
				};
			}

			public TraitValBuilder Rarity(int rarity)
			{
				val.rarity = rarity;
				return this;
			}

			public TraitValBuilder ExclusiveWithTraits(params string[] traits)
			{
				val.mutuallyExclusiveTraits = traits.ToList();
				return this;
			}

			public TraitValBuilder ExclusiveWithAptitudes(params HashedString[] traits)
			{
				val.mutuallyExclusiveAptitudes = traits.ToList();
				return this;
			}

			public TraitValBuilder Dlc(string dlcId)
			{
				val.dlcId = dlcId;
				return this;
			}

			public TraitValBuilder Impact(int impact)
			{
				val.impact = impact;
				return this;
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

		public TraitValBuilder AddToTraits(List<DUPLICANTSTATS.TraitVal> traitVals)
		{
			var traitVal = new TraitValBuilder(trait.Id);
			traitVals.Add(traitVal.val);
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

		public TraitBuilder DisableChoreGroups(ChoreGroup[] choreGroups)
		{
			trait.disabledChoreGroups = choreGroups;
			return this;
		}
	}
}
