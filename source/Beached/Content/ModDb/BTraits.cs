﻿using Beached.Content.Defs.Equipment;
using Beached.Content.Scripts;
using Beached.Content.Scripts.LifeGoals;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BTraits
	{
		public const string DEXTEROUS = "Beached_Dexterous";
		public const string FUR_ALLERGY = "Beached_FurAllergy";
		public const string GILLS = "Beached_Gills";
		public const string COMFORT_SEEKER = "Beached_ComfortSeeker";
		public const string PLUSHIE_MAKER = "Beached_PlushieMaker";

		public class LifeGoals
		{
			public const string JEWELLERY_AQUAMARINE = "Beached_Trait_WantsJewellery";
			public const string BEDROOM_SURFBOARD = "Beached_Trait_WantsSurfboardInBedroom";
		}

		public static List<string> LIFEGOALS = new();

		public static void Register()
		{
			var db = Db.Get();

			var plushieMakerTrait = db.CreateTrait(
				PLUSHIE_MAKER,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_PLUSHIE_MAKER.NAME,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_PLUSHIE_MAKER.DESC,
				null,
				true,
				null,
				true,
				true);

			plushieMakerTrait.OnAddTrait += OnAddPlushieMaker;

			var dexterousTrait = db.CreateTrait(
				DEXTEROUS,
				STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.NAME,
				STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.DESC,
				BAttributes.PRECISION_ID,
				true,
				null,
				true,
				true);

			dexterousTrait.Add(new AttributeModifier(
				BAttributes.PRECISION_ID,
				TRAITS.GOOD_ATTRIBUTE_BONUS,
				STRINGS.DUPLICANTS.TRAITS.PRECISIONUP.NAME));

			var furAllergyTrait = db.CreateTrait(
				FUR_ALLERGY,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_FURALLERGY.NAME,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_FURALLERGY.DESC,
				null,
				true,
				null,
				false,
				true);

			furAllergyTrait.OnAddTrait += go => go.AddTag(BTags.furAllergic);

			AddJewelleryTrait(
				LifeGoals.JEWELLERY_AQUAMARINE,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME,
				string.Format("This duplicant really wishes to express themselves by wearing a {0}.", STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME),
				MaxixePendantConfig.ID);

			AddBedroomTrait(
				LifeGoals.BEDROOM_SURFBOARD,
				"Surfin' and Snoozin'",
				string.Format("This duplicant sannot stop talking about how cool it would be to have a {0} in their bedroom.", global::STRINGS.BUILDINGS.PREFABS.MECHANICALSURFBOARD.NAME),
				MechanicalSurfboardConfig.ID);

			var gillsTrait = Db.Get().CreateTrait(
					GILLS,
					STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.NAME,
					STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.DESC,
					null,
					true,
					null,
					true,
					true);

			gillsTrait.Add(new AttributeModifier(
				Db.Get().Attributes.AirConsumptionRate.Id,
				-0.005f,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.NAME));

			gillsTrait.ExtendedTooltip = GetGillsTooltip;
			gillsTrait.OnAddTrait = OnAddGills;

			var comfortSeeker = Db.Get().CreateTrait(
					COMFORT_SEEKER,
					STRINGS.DUPLICANTS.TRAITS.BEACHED_COMFORT_SEEKER.NAME,
					STRINGS.DUPLICANTS.TRAITS.BEACHED_COMFORT_SEEKER.DESC,
					null,
					true,
					null,
					true,
					true);

			DUPLICANTSTATS.GOODTRAITS.Add(new DUPLICANTSTATS.TraitVal()
			{
				id = DEXTEROUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					//"Anemic"
				}
			});

			DUPLICANTSTATS.BADTRAITS.Add(new DUPLICANTSTATS.TraitVal()
			{
				id = FUR_ALLERGY,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Allergies"
				}
			});

			LIFEGOALS.Add(LifeGoals.JEWELLERY_AQUAMARINE);
		}

		private static void OnAddGills(GameObject go)
		{
			if (go.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.AddTag(BTags.amphibious, true);
		}

		private static void OnAddPlushieMaker(GameObject go)
		{
			var component = go.GetComponent<KMonoBehaviour>();
			new BalloonArtist.Instance(component).StartSM();
			new JoyBehaviourMonitor.Instance(
				component,
				"anim_loco_happy_balloon_kanim",
				null,
				Db.Get().Expressions.Balloon)
				.StartSM();
		}

		private static string GetGillsTooltip()
		{
			return STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.WATERBREATHING;
		}

		private static void AddJewelleryTrait(string id, string name, string desc, Tag targetTag, Func<string> extendedDescFn = null)
		{
			var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
			trait.OnAddTrait = go =>
			{
				go.AddOrGet<Beached_LifeGoalTracker>().wantTag = targetTag;
				go.AddOrGet<EquipmentGoal>();
			};

			trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";

			if (extendedDescFn != null)
				trait.ExtendedTooltip += extendedDescFn;
		}

		private static void AddBedroomTrait(string id, string name, string desc, Tag targetTag, Func<string> extendedDescFn = null)
		{
			var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
			trait.OnAddTrait = go =>
			{
				Log.Debug("on add traits");
				go.AddOrGet<Beached_LifeGoalTracker>().wantTag = targetTag;
				Log.Debug(targetTag);
				go.FindOrAddUnityComponent<BedroomBuildingGoal>();
			};


			trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";

			if (extendedDescFn != null)
			{
				trait.ExtendedTooltip += extendedDescFn;
			}
		}
		public static Trait GetGoalForPersonality(Personality personality)
		{
			return Db.Get().traits.Get(LifeGoals.BEDROOM_SURFBOARD);
		}
	}
}