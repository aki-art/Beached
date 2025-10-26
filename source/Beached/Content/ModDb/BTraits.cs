using Beached.Content.BWorldGen;
using Beached.Content.Defs;
using Beached.Content.Defs.Equipment;
using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.AI;
using Beached.Content.Scripts.LifeGoals;
using Klei.AI;
using ProcGen;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BTraits
	{
		public const string
			// intrinsic trait of Vahano
			CARNIVOROUS = "Beached_Carnivorous",

			// trait only for dupes who met Drales i space
			HOPEFUL = "Beached_Hopeful",

			//intrinsic trait of Minnow
			GILLS = "Beached_Gills",
			// Minnow joy trait
			PLUSHIE_MAKER = "Beached_PlushieMaker",
			// Minnow stress trait
			SIREN = "Beached_Siren",

			// Bad traits
			CLUMSY = "Beached_Clumsy",
			FUR_ALLERGY = "Beached_FurAllergy",
			VEGETARIAN = "Beached_Vegetarian",
			// sweaty

			// Good traits
			COMFORT_SEEKER = "Beached_ComfortSeeker",
			DEXTEROUS = "Beached_Dexterous",
			THALASSOPHILE = "Beached_Thalassophile",
			HOT_BLOODED = "Beached_HotBlooded",

			// Bionic upgrade
			PRECISION_BOOSTER = "";

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

		public static readonly List<string> LIFE_GOALS =
		[
			LIFE_GOAL_IDS.GOLDENTHRONE,
			LIFE_GOAL_IDS.JEWELLERY_MAXIXE,
			LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
			LIFE_GOAL_IDS.JEWELLERY_PEARLS,
			LIFE_GOAL_IDS.BEDROOM_SURFBOARD,
			LIFE_GOAL_IDS.HAS_50_DECOR,
		];

		public static Dictionary<string, string> LifeGoalsPerDupe = new()
		{
			{ "PEI" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "ARI" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "ELLIE" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "GOSSMANN" , LIFE_GOAL_IDS.CATEGORIES.JEWELLERY },
			{ "BEACHED_MINNOW" , LIFE_GOAL_IDS.CATEGORIES.MINNOW },
			{ "MEEP" , LIFE_GOAL_IDS.CATEGORIES.MEEP },
			{ "RUBY" , LIFE_GOAL_IDS.CATEGORIES.RUBY },
		};

		public static void Register()
		{
			var db = Db.Get();

			new TraitBuilder(HOPEFUL, true)
				.Modifier(db.Amounts.Stress.deltaAttribute.Id, MiscUtil.PerCycle(-5f));

			new TraitBuilder(PLUSHIE_MAKER, true)
				.OnAdd(OnAddPlushieMaker)
				.AddToTraits()
					.Build(DUPLICANTSTATS.JOYTRAITS);

			new TraitBuilder(SIREN, false)
				.OnAdd(OnAddSiren)
				.AddToTraits()
					.Build(DUPLICANTSTATS.STRESSTRAITS);

			new TraitBuilder(CLUMSY, false)
				.DisableChoreGroups(BChoreGroups.handyWork)
				.AddToTraits()
					.Rarity(DUPLICANTSTATS.RARITY_EPIC)
					.StatBonus(DUPLICANTSTATS.LARGE_STATPOINT_BONUS)
					.ExclusiveWithTraits(DEXTEROUS)
					.ExclusiveWithAptitudes(BSkillGroups.PRECISION_ID)
					.Build(DUPLICANTSTATS.BADTRAITS);

			new TraitBuilder(FUR_ALLERGY, false)
				.Tag(BTags.furAllergic)
				.AddToTraits()
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.ExclusiveWithTraits("Allergies")
					.Build(DUPLICANTSTATS.BADTRAITS);

			new TraitBuilder(COMFORT_SEEKER, true)
				.AddToTraits()
					.ExclusiveWithTraits("DecorUp", "Fashionable")
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.Build(DUPLICANTSTATS.GOODTRAITS);

			new TraitBuilder(HOT_BLOODED, true)
				.Modifier(BAttributes.HEAT_RESISTANCE_ID, 1f, true)
				.Modifier(db.Attributes.Athletics.Id, 2f, false)
				.Tag(BTags.easilyTriggers)
				.ExtendedTooltip(() => "Stress response triggers at 90% Stress")
				.AddToTraits()
					.ExclusiveWithTraits("FrostProof")
					.Rarity(DUPLICANTSTATS.RARITY_UNCOMMON)
					.Build(DUPLICANTSTATS.GOODTRAITS);

			/*			for (var i = 0; i < DUPLICANTSTATS.GOODTRAITS.Count; i++)
						{
							var traitVal = DUPLICANTSTATS.GOODTRAITS[i];
							if (traitVal.id == "FrostProof")
							{
								traitVal.mutuallyExclusiveTraits ??= [];
								traitVal.mutuallyExclusiveTraits.Add(HOT_BLOODED);
							}
						}*/

			new TraitBuilder(GILLS, true)
				.OnAdd(go => AddTag(go, BTags.amphibious))
				.Modifier(db.Attributes.AirConsumptionRate.Id, -0.005f)
				.ExtendedTooltip(GetGillsTooltip);

			new TraitBuilder(THALASSOPHILE, true)
				.OnAdd(OnAddThalassophile)
				.ExtendedTooltip(GetThalassophileTooltip)
				.AddToTraits()
					//.ExclusiveWithTraits(CLUMSY)
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.Build(DUPLICANTSTATS.GOODTRAITS);

			new TraitBuilder(CARNIVOROUS, true)
				.OnAdd(go => AddTag(go, BTags.carnivorous));

			new TraitBuilder(VEGETARIAN, false)
				.OnAdd(go => AddTag(go, BTags.vegetarian))
				.AddToTraits()
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.ExclusiveWithTraits(CARNIVOROUS)
					.Build(DUPLICANTSTATS.BADTRAITS);

			new TraitBuilder(DEXTEROUS, true, BAttributes.PRECISION_ID)
				.Modifier(BAttributes.PRECISION_ID, TRAITS.GOOD_ATTRIBUTE_BONUS)
				.AddToTraits()
					.ExclusiveWithTraits(CLUMSY)
					.Rarity(DUPLICANTSTATS.RARITY_COMMON)
					.Build(DUPLICANTSTATS.GOODTRAITS);

			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_MAXIXE,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_MAXIXEPENDANT.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.MAXIXE_PENDANT.DESCRIPTION,
				MaxixePendantConfig.ID);

			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_PEARLS,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_PEARLNECKLACE.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.PEARL_PENDANT.DESCRIPTION,
				PearlNecklaceConfig.ID);

			AddJewelleryTrait(
				LIFE_GOAL_IDS.JEWELLERY_STRANGE_MATTER,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_STRANGEMATTERAMULET.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.STRANGE_MATTER_PENDANT.DESCRIPTION,
				StrangeMatterAmuletConfig.ID);

			AddAssignableTrait(
				LIFE_GOAL_IDS.GOLDENTHRONE,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.GOLDEN_LAVATORY.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.GOLDEN_LAVATORY.DESCRIPTION,
				slot => slot.assignable != null
					&& slot.assignable.PrefabID() == FlushToiletConfig.ID
					&& slot.assignable.TryGetComponent(out PrimaryElement pe)
					&& (pe.ElementID == SimHashes.GoldAmalgam || pe.ElementID == SimHashes.Gold));

			AddAssignableTrait(
				LIFE_GOAL_IDS.HAS_50_DECOR,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.FASHION_IDOL.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.FASHION_IDOL.DESCRIPTION,
				slot =>
				{
					var assignee = slot.assignable.assignee;
					return assignee is KMonoBehaviour component
						&& component.TryGetComponent(out ClothingWearer clothingWearer)
						&& clothingWearer.decorModifier.Value >= 50f;
				}); // TODO

			AddBedroomTrait(
				LIFE_GOAL_IDS.BEDROOM_SURFBOARD,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.BEACHED_MINNOW.NAME,
				STRINGS.DUPLICANTS.TRAITS.LIFE_GOALS.BEACHED_MINNOW.DESCRIPTION,
				MechanicalSurfboardConfig.ID);

			DUPLICANTSTATS.ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY[BSkillGroups.PRECISION_ID] = [
				ExtraBionicUpgradeComponentConfig.PRECISION1_ID,
				ExtraBionicUpgradeComponentConfig.PRECISION2_ID
				];
		}

		private static string GetThalassophileTooltip()
		{
			var effect = Db.Get().effects?.Get(BEffects.THALASSOPHILE_BONUS);

			// fail safe
			if (effect == null)
				return $"{STRINGS.DUPLICANTS.TRAITS.BEACHED_THALASSOPHILE.DESC_EXTENDED}\n    • Morale +2\n    • Operating Speed: +10%";

			return $"{STRINGS.DUPLICANTS.TRAITS.BEACHED_THALASSOPHILE.DESC_EXTENDED}{Effect.CreateTooltip(effect, false, showHeader: false)}";
		}

		private static void OnAddSiren(GameObject go)
		{
			var statusItem = new StatusItem(
				"Beached_StressSignalSiren",
				STRINGS.DUPLICANTS.STATUSITEMS.BEACHED_SIREN.NAME,
				STRINGS.DUPLICANTS.STATUSITEMS.BEACHED_SIREN.TOOLTIP,
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
					["interrupt_binge_eat"],
					KAnim.PlayMode.Once,
					() => statusItem)),
				chore_provider => new EmptyChore(chore_provider),
				"anim_loco_binge_eat_kanim", 30f)
				.StartSM();
		}

		private static void AddTag(GameObject go, Tag tag)
		{
			if (go.TryGetComponent(out KPrefabID kPrefabID))
				kPrefabID.AddTag(tag, true);
		}

		private static void OnAddThalassophile(GameObject go)
		{
			var component = go.GetComponent<KMonoBehaviour>();
			new Beached_ThalassoTraitMonitor.Instance(component, new Beached_ThalassoTraitMonitor.Def()
			{
				seaBiomes = [ZoneTypes.depths, ZoneTypes.beach, ZoneTypes.sea, ZoneTypes.coralReef, SubWorld.ZoneType.Ocean],
				effectId = BEffects.THALASSOPHILE_BONUS
			}).StartSM();
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

		private static string GetGillsTooltip() => STRINGS.DUPLICANTS.TRAITS.BEACHED_GILLS.WATERBREATHING;


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
