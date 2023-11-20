using Beached.Content.Defs.Equipment;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.AI;
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
		public const string
			DEXTEROUS = "Beached_Dexterous",
			FUR_ALLERGY = "Beached_FurAllergy",
			GILLS = "Beached_Gills",
			COMFORT_SEEKER = "Beached_ComfortSeeker",
			PLUSHIE_MAKER = "Beached_PlushieMaker",
			SIREN = "Beached_Siren";

		public class LIFE_GOAL_IDS
		{
			public class CATEGORIES
			{
				public const string
					GENERIC = "Generic",
					JEWELLERY = "Jewellery",
					MINNOW = "Minnow",
					PET = "Pet",
					BURT = "Burt",
					RUBY = "Ruby",
					MEEP = "Meep";
			}

			public const string
				GOLDENTHRONE = "Beached_Trait_GoldenThrone",
				JEWELLERY_MAXIXE = "Beached_Trait_WantsJewellery_Maxixe",
				JEWELLERY_STRANGE_MATTER = "Beached_Trait_WantsJewellery_StrangeMatter",
				JEWELLERY_PEARLS = "Beached_Trait_WantsJewellery_FlawlessDiamond",
				BEDROOM_SURFBOARD = "Beached_Trait_WantsSurfboardInBedroom",
				HAS_50_DECOR = "Beached_Trait_Wants50Decor";
		}

		public static readonly Dictionary<string, List<string>> LIFE_GOAL_CATEGORIES = new()
		{
			{
				LIFE_GOAL_IDS.CATEGORIES.JEWELLERY,
				new()
				{
					LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
					LIFE_GOAL_IDS.JEWELLERY_PEARLS,
					LIFE_GOAL_IDS.JEWELLERY_MAXIXE
				}
			},
			{
				LIFE_GOAL_IDS.CATEGORIES.MINNOW,
				new()
				{
					LIFE_GOAL_IDS.BEDROOM_SURFBOARD
				}
			},
			{
				LIFE_GOAL_IDS.CATEGORIES.MEEP,
				new()
				{
					LIFE_GOAL_IDS.GOLDENTHRONE
				}
			},
			{
				LIFE_GOAL_IDS.CATEGORIES.RUBY,
				new()
				{
					LIFE_GOAL_IDS.HAS_50_DECOR
				}
			},
			{
				LIFE_GOAL_IDS.CATEGORIES.GENERIC,
				new()
				{
					LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
					LIFE_GOAL_IDS.JEWELLERY_PEARLS,
					LIFE_GOAL_IDS.JEWELLERY_MAXIXE
				}
			}
		};

		public static readonly List<string> LIFE_GOALS = new()
		{
			LIFE_GOAL_IDS.GOLDENTHRONE,
			LIFE_GOAL_IDS.JEWELLERY_MAXIXE,
			LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
			LIFE_GOAL_IDS.JEWELLERY_PEARLS,
			LIFE_GOAL_IDS.BEDROOM_SURFBOARD,
			LIFE_GOAL_IDS.HAS_50_DECOR,
		};

		public static Dictionary<string, string> LifeGoalsPerDupe = new()
		{
			{ "PEI" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "ARI" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "ELLIE" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "GOSSMANN" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "MINNOW" , LIFE_GOAL_IDS.CATEGORIES.MINNOW },
			{ "MEEP" , LIFE_GOAL_IDS.CATEGORIES.MEEP },
			{ "RUBY" , LIFE_GOAL_IDS.CATEGORIES.RUBY },
		};

		public static void Register()
		{
			var db = Db.Get();

			new TraitBuilder(PLUSHIE_MAKER, true)
				.OnAdd(OnAddPlushieMaker)
				.AddToTraits(DUPLICANTSTATS.JOYTRAITS);

			new TraitBuilder(SIREN, false)
				.OnAdd(OnAddSiren)
				.AddToTraits(DUPLICANTSTATS.STRESSTRAITS);

			new TraitBuilder(FUR_ALLERGY, false)
				.Tag(BTags.furAllergic)
				.AddToTraits(DUPLICANTSTATS.BADTRAITS)
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.ExclusiveWithTraits("Allergies");

			new TraitBuilder(COMFORT_SEEKER, true)
				.AddToTraits(DUPLICANTSTATS.GOODTRAITS)
					.ExclusiveWithTraits("DecorUp", "Fashionable");

			new TraitBuilder(GILLS, true)
				.OnAdd(OnAddGills)
				.Modifier(db.Attributes.AirConsumptionRate.Id, -0.005f)
				.ExtendedTooltip(GetGillsTooltip);

			new TraitBuilder(DEXTEROUS, true, BAttributes.PRECISION_ID)
				.Modifier(BAttributes.PRECISION_ID, TRAITS.GOOD_ATTRIBUTE_BONUS)
				.AddToTraits(DUPLICANTSTATS.GOODTRAITS);


			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_MAXIXE,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME,
				"This duplicant really wishes to express themselves by wearing a Maxixe Pendant.",
				MaxixePendantConfig.ID);

			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_PEARLS,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_PEARLNECKLACE.NAME,
				"This duplicant would love to don a Pearl Necklace.",
				PearlNecklaceConfig.ID);

			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME,
				"This duplicant has a deep desire the wear a Strange Matter Amulet.",
				StrangeMatterAmuletConfig.ID);

			AddAssignableTrait(
				LIFE_GOAL_IDS.GOLDENTHRONE,
				"Golden Lavatory",
				"This duplicant wishes to own their very own golden lavatory.",
				slot => slot.assignable != null
					&& slot.assignable.PrefabID() == FlushToiletConfig.ID
					&& slot.assignable.TryGetComponent(out PrimaryElement pe)
					&& pe.ElementID == SimHashes.GoldAmalgam);

			AddAssignableTrait(
				LIFE_GOAL_IDS.HAS_50_DECOR,
				"Fashion Idol",
				"This duplicants dream is to be as slick as it can get. Achieve 50 additional decor with equipment to achieve their life goal.",
				slot =>
				{
					var assignee = slot.assignable.assignee;
					return assignee is KMonoBehaviour component
						&& component.TryGetComponent(out ClothingWearer clothingWearer)
						&& clothingWearer.decorModifier.Value >= 50f;
				}); // TODO

			AddBedroomTrait(
				LIFE_GOAL_IDS.BEDROOM_SURFBOARD,
				"Surfin' and Snoozin'",
				string.Format("This duplicant sannot stop talking about how cool it would be to have a {0} in their bedroom.", global::STRINGS.BUILDINGS.PREFABS.MECHANICALSURFBOARD.NAME),
				MechanicalSurfboardConfig.ID);
		}

		private static void OnAddSiren(GameObject go)
		{
			var statusItem = new StatusItem(
				"Beached_StressSignalSiren",
				"Siren",
				"",
				"",
				StatusItem.IconType.Info,
				NotificationType.BadMinor,
				false,
				OverlayModes.None.ID);

			var component = go.GetComponent<KMonoBehaviour>();

			new Siren.Instance(component).StartSM();

			new StressBehaviourMonitor.Instance(go.GetComponent<KMonoBehaviour>(),
				(chore_provider => new StressEmoteChore(
					chore_provider,
					Db.Get().ChoreTypes.StressEmote,
					"anim_interrupt_binge_eat_kanim",
					new HashedString[] { "interrupt_binge_eat" },
					KAnim.PlayMode.Once,
					() => statusItem)),
				chore_provider => new EmptyChore(chore_provider),
				"anim_loco_binge_eat_kanim", 30f)
				.StartSM();
		}

		private static void OnAddGills(GameObject go)
		{
			if (go.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.AddTag(BTags.amphibious, true);
		}

		private static void OnAddPlushieMaker(GameObject go)
		{
			var component = go.GetComponent<KMonoBehaviour>();
			new PlushieGifter.Instance(component).StartSM();
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

		private static void AddAssignableTrait(string id, string name, string desc, Tag wantedAssignable)
		{
			var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
			trait.OnAddTrait = go =>
			{
				go.AddOrGet<Beached_LifeGoalTracker>().AddSimpleAssignable(wantedAssignable);
				go.AddOrGet<AssignableGoal>();
			};

			trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";
		}

		private static void AddAssignableTrait(string id, string name, string desc, Func<AssignableSlotInstance, bool> hasWantedAssignableFn)
		{
			var trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
			trait.OnAddTrait = go =>
			{
				go.AddOrGet<Beached_LifeGoalTracker>().hasWantedAssignableFn = hasWantedAssignableFn;
				go.AddOrGet<AssignableGoal>();
			};

			trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";
		}

		private static void AddJewelleryTrait(string id, string name, string desc, Tag targetTag, Func<string> extendedDescFn = null)
		{
			var trait = Db.Get().CreateTrait(
				id,
				name,
				desc,
				null,
				true,
				null,
				true,
				true);
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
				go.AddOrGet<Beached_LifeGoalTracker>().wantTag = targetTag;
				go.AddOrGet<BedroomBuildingGoal>();
			};

			trait.ExtendedTooltip += () => "Complete this objective to motivate this duplicant.\n\n";

			if (extendedDescFn != null)
				trait.ExtendedTooltip += extendedDescFn;
		}

		public static Trait GetGoalForPersonality(Personality personality)
		{
			var category = LifeGoalsPerDupe.TryGetValue(personality.Id, out var goalsPerDupe)
				? goalsPerDupe
				: LIFE_GOAL_IDS.CATEGORIES.GENERIC;

			if (LIFE_GOAL_CATEGORIES.TryGetValue(category, out var options))
				return Db.Get().traits.TryGet(options.GetRandom());

			Log.Warning("Could not roll goal for personality: " + personality.Id);
			return null;
		}
	}
}
