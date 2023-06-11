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

		public class LIFE_GOALS
		{
			public class CATEGORIES
			{
				public const string
					GENERIC = "Generic",
					JEWELLERY = "Jewellery",
					MINNOW = "Minnow",
					PET = "Pet",
					BURT = "Burt";
			}

			public const string
				JEWELLERY_MAXIXE = "Beached_Trait_WantsJewellery_Maxixe",
				JEWELLERY_STRANGE_MATTER = "Beached_Trait_WantsJewellery_StrangeMatter",
				JEWELLERY_PEARLS = "Beached_Trait_WantsJewellery_FlawlessDiamond",
				BEDROOM_SURFBOARD = "Beached_Trait_WantsSurfboardInBedroom";
		}

		public static readonly Dictionary<string, List<string>> LIFE_GOAL_CATEGORIES = new()
		{
			{
				LIFE_GOALS.CATEGORIES.JEWELLERY,
				new()
				{
					LIFE_GOALS.JEWELLERY_STRANGE_MATTER,
					LIFE_GOALS.JEWELLERY_PEARLS,
					LIFE_GOALS.JEWELLERY_MAXIXE
				}
			},
			{
				LIFE_GOALS.CATEGORIES.MINNOW,
				new()
				{
					LIFE_GOALS.BEDROOM_SURFBOARD
				}
			},
			{
				LIFE_GOALS.CATEGORIES.GENERIC,
				new()
				{
					LIFE_GOALS.JEWELLERY_STRANGE_MATTER,
					LIFE_GOALS.JEWELLERY_PEARLS,
					LIFE_GOALS.JEWELLERY_MAXIXE
				}
			}
		};

		public static List<string> lifeGoals = new();

		public static Dictionary<string, string> LifeGoalsPerDupe = new()
		{
			{ "PEI" , LIFE_GOALS.CATEGORIES.JEWELLERY },
			{ "ARI" , LIFE_GOALS.CATEGORIES.JEWELLERY },
			{ "ELLIE" , LIFE_GOALS.CATEGORIES.JEWELLERY },
			{ "GOSSMANN" , LIFE_GOALS.CATEGORIES.JEWELLERY },
			{ "MINNOW" , LIFE_GOALS.CATEGORIES.MINNOW },
		};

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

			var sirenTrait = db.CreateTrait(
				SIREN,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_SIREN.NAME,
				STRINGS.DUPLICANTS.TRAITS.BEACHED_SIREN.DESC,
				null,
				true,
				null,
				false,
				true);

			sirenTrait.OnAddTrait += OnAddSiren;

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
				LIFE_GOALS.JEWELLERY_MAXIXE,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME,
				"This duplicant really wishes to express themselves by wearing a Maxixe Pendant.",
				MaxixePendantConfig.ID);

			AddJewelleryTrait(
				LIFE_GOALS.JEWELLERY_PEARLS,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_PEARLNECKLACE.NAME,
				"This duplicant would love to don a Pearl Necklace.",
				PearlNecklaceConfig.ID);

			AddJewelleryTrait(
				LIFE_GOALS.JEWELLERY_STRANGE_MATTER,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME,
				"This duplicant has a deep desire the wear a Strange Matter Amulet.",
				StrangeMatterAmuletConfig.ID);

			AddBedroomTrait(
				LIFE_GOALS.BEDROOM_SURFBOARD,
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

			DUPLICANTSTATS.GOODTRAITS.AddRange(new List<DUPLICANTSTATS.TraitVal>()
			{
				new DUPLICANTSTATS.TraitVal()
				{
					id = DEXTEROUS,
					rarity = DUPLICANTSTATS.RARITY_COMMON,
					dlcId = "",
					mutuallyExclusiveTraits = new List<string>
					{
						//"Anemic"
					}
				},
				new DUPLICANTSTATS.TraitVal()
				{
					id = COMFORT_SEEKER,
					rarity = DUPLICANTSTATS.RARITY_COMMON,
					dlcId = "",
					mutuallyExclusiveTraits = new List<string>
					{
						"DecorUp", // "Stylish"
						"Fashionable" // need type trait normally unused, but Bio-Inks reintroduces it
					}
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

			DUPLICANTSTATS.JOYTRAITS.Add(new DUPLICANTSTATS.TraitVal()
			{
				id = PLUSHIE_MAKER,
				dlcId = ""
			});

			DUPLICANTSTATS.STRESSTRAITS.Add(new DUPLICANTSTATS.TraitVal()
			{
				id = SIREN,
				dlcId = ""
			});


			lifeGoals.Add(LIFE_GOALS.JEWELLERY_MAXIXE);
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
			var category = LifeGoalsPerDupe.TryGetValue(personality.Id, out var goalsPerDupe)
				? goalsPerDupe
				: LIFE_GOALS.CATEGORIES.GENERIC;

			if (LIFE_GOAL_CATEGORIES.TryGetValue(category, out var options))
				return Db.Get().traits.TryGet(options.GetRandom());

			Log.Warning("Could not roll goal for personality: " + personality.Id);
			return null;
		}
	}
}
