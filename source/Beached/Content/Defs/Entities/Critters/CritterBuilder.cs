using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Entities.Critters
{
	public class CritterBuilder(string ID, string anim)
	{
		public const string
			ADULT = "Adult";

		private readonly string id = ID;
		private readonly string anim = anim;

		private GameObject prefab;
		private KPrefabID kPrefabID;
		private string[] drops;
		private string navigationGrid = NAVIGATION.WALKER_1X1;
		private NavType navType = NavType.Floor;
		private FactionManager.FactionID faction = FactionManager.FactionID.Prey;
		private float moveSpeed = 2f;
		private int maxProbingRadius = 32;
		private bool canDrown = true;
		private bool diesEntombed = true;
		private ChoreTable.Builder choreTable;
		private bool trappable;
		private float mass = 50f;
		private float? defaultTemperature = 0;
		private int width = 1, height = 1;
		private BabyCritterBuilder babyCritterBuilder;
		private BrainBuilder brain;
		private TraitsBuilder traitsBuilder;
		private int maxPenSize;
		private string parentId;
		private bool isBaby;
		private int growUpOnCycle;
		private string dropOnGrowUp;
		private bool forceAdultNavType;
		private float tempMinWarning = 283.15f, tempMinLethal = 243.15f, tempMaxWarning = 293.15f, tempMaxLethal = 343.15f;
		private EffectorValues decor = DECOR.NONE;
		private HashSet<Tag> tags = [];
		private AttackProperties attackProperties;
		private WeaponBuilder weapon;
		private bool baggable;

		public string GetName()
		{
			return isBaby
				? Strings.Get($"STRINGS.CREATURES.SPECIES.{parentId.ToUpperInvariant()}.BABY_NAME")
				: Strings.Get($"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.NAME");
		}

		public string GetDescription()
		{
			return isBaby
				? Strings.Get($"STRINGS.CREATURES.SPECIES.{parentId.ToUpperInvariant()}.BABY_DESC")
				: Strings.Get($"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.DESC");
		}

		public class NAVIGATION
		{
			public const string
				WALKER_BABY = "WalkerBabyNavGrid",
				WALKER_1X1 = "WalkerNavGrid1x1",
				WALKER_1X2 = "WalkerNavGrid1x2",
				MINION = "MinionNavGrid",
				ROBOT = "RobotNavGrid",
				DIGGER = "DiggerNavGrid",
				DRECKO = "DreckoNavGrid",
				DRECKO_BABY = "DreckoBabyNavGrid",
				FLYER_1X1 = "FlyerNavGrid1x1",
				FLYER_1X2 = "FlyerNavGrid1x2",
				FLYER_2X2 = "FlyerNavGrid2x2",
				SLICKSTER = "FloaterNavGrid",
				SWIMMER = "SwimmerNavGrid",
				PIP = "SquirrelNavGrid";
		}

		public CritterBuilder Baby(string parentId, int growUpOnCycle = 5, string dropOnGrowUp = null, bool forceNavType = false)
		{
			this.parentId = parentId;
			isBaby = true;
			this.growUpOnCycle = growUpOnCycle;
			this.dropOnGrowUp = dropOnGrowUp;
			this.forceAdultNavType = forceNavType;

			return this;
		}

		public CritterBuilder Baggable()
		{
			baggable = true;
			return this;
		}

		public BrainBuilder Brain(Tag species)
		{
			brain = new BrainBuilder(this, species);
			return brain;
		}

		public CritterBuilder CanBurrow()
		{
			diesEntombed = false;
			return this;
		}

		public CritterBuilder CanNotDrown()
		{
			canDrown = false;
			return this;
		}
		public CritterBuilder Decor(int value, int range) => Decor(new EffectorValues(value, range));

		public CritterBuilder Decor(EffectorValues decor)
		{
			this.decor = decor;
			return this;
		}


		public CritterBuilder Drops(params string[] tags)
		{
			drops = tags;
			return this;
		}

		public CritterBuilder DefaultTemperatureC(float celsius) => DefaultTemperatureK(MiscUtil.CelsiusToKelvin(celsius));

		public CritterBuilder DefaultTemperatureK(float kelvin)
		{
			this.defaultTemperature = kelvin;
			return this;
		}

		public BabyCritterBuilder Egg(string babyId, string eggKanim)
		{
			babyCritterBuilder = new BabyCritterBuilder(this, babyId, eggKanim);
			return babyCritterBuilder;
		}

		public CritterBuilder Faction(FactionManager.FactionID faction)
		{
			this.faction = faction;
			return this;
		}

		private NavType GetNavTypeForNav(string nav)
		{
			return nav switch
			{
				NAVIGATION.FLYER_1X1 or NAVIGATION.FLYER_1X2 or NAVIGATION.FLYER_2X2 or NAVIGATION.SLICKSTER => NavType.Hover,
				NAVIGATION.SWIMMER => NavType.Swim,
				_ => NavType.Floor,
			};
		}

		public CritterBuilder Mass(float kg)
		{
			mass = kg;
			return this;
		}

		public CritterBuilder MaxPenSize(int maxCritterPerPen)
		{
			maxPenSize = maxCritterPerPen;
			return this;
		}
		public CritterBuilder Navigator(string navigationGrid, float speed = 2f)
		{
			return Navigator(navigationGrid, GetNavTypeForNav(navigationGrid), speed);
		}

		public CritterBuilder Navigator(string navigationGrid, NavType navType, float speed = 2f, int maxProbingRadius = 32)
		{
			this.navigationGrid = navigationGrid;
			this.navType = navType;
			this.maxProbingRadius = maxProbingRadius;
			moveSpeed = speed;

			return this;
		}

		public CritterBuilder Sorting(int sorting)
		{
			CREATURES.SORTING.CRITTER_ORDER[id] = sorting;
			return this;
		}

		public CritterBuilder SortAfter(string critter)
		{
			CREATURES.SORTING.CRITTER_ORDER[id] = CREATURES.SORTING.CRITTER_ORDER[critter] + 4;
			return this;
		}

		public CritterBuilder SortBefore(string critter)
		{
			CREATURES.SORTING.CRITTER_ORDER[id] = CREATURES.SORTING.CRITTER_ORDER[critter] - 4;
			return this;
		}

		public CritterBuilder ShedFur(float amount, Color color)
		{
			Integration.CritterShedding.Shed(id, amount, color);
			return this;
		}

		public CritterBuilder Size(int width, int height)
		{
			this.width = width;
			this.height = height;
			return this;
		}

		public CritterBuilder Speed(float speed)
		{
			moveSpeed = speed;
			return this;
		}

		public CritterBuilder Tags(HashSet<Tag> tags)
		{
			foreach (Tag tag in tags)
				this.tags.Add(tag);

			return this;
		}
		public CritterBuilder Tag(Tag tag)
		{
			tags.Add(tag);
			return this;
		}

		public CritterBuilder Tag(string tag)
		{
			tags.Add(tag);
			return this;
		}

		public CritterBuilder TemperatureCelsius(float minLethal, float minWarning, float maxWarning, float maxLethal)
		{
			return TemperatureKelvin(
				MiscUtil.CelsiusToKelvin(minLethal),
				MiscUtil.CelsiusToKelvin(minWarning),
				MiscUtil.CelsiusToKelvin(maxWarning),
				MiscUtil.CelsiusToKelvin(maxLethal));
		}

		public CritterBuilder TemperatureKelvin(float minLethal, float minWarning, float maxWarning, float maxLethal)
		{
			tempMinLethal = minLethal;
			tempMinWarning = minWarning;
			tempMaxWarning = maxWarning;
			tempMaxLethal = maxLethal;

			return this;
		}

		public TraitsBuilder Traits()
		{
			Log.Debug("adding traits to " + id);
			traitsBuilder ??= new TraitsBuilder(this);
			return traitsBuilder;
		}

		public CritterBuilder Trappable()
		{
			trappable = true;
			return this;
		}

		public GameObject Build()
		{
			if (!Assets.TryGetAnim(anim, out var animFile))
				Log.Warning($"Error in configuring {id}: anim {anim} is not a valid kanim id");

			if (isBaby && babyCritterBuilder != null)
				Log.Warning($"This baby can make babies?? huhcat.mp4");

			if (brain == null)
				Log.Warning($"Error in configuring {id}: Brain was not set up");

			Log.Debug("Adding critter");
			Log.Debug($"Name: {GetName()}");
			Log.Debug($"Desc: {GetDescription()}");

			prefab = EntityTemplates.CreatePlacedEntity(
				id,
				GetName(),
				GetDescription(),
				mass,
				animFile,
				"idle_loop",
				Grid.SceneLayer.Creatures,
				width,
				height,
				decor);

			kPrefabID = prefab.GetComponent<KPrefabID>();
			brain?.Apply();

			if (navigationGrid.IsNullOrWhiteSpace())
				Log.Warning($"Error in configuring {id}: navigation was not set up");

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				faction,
				id + "BaseTrait",
				navigationGrid,
				navType,
				maxProbingRadius,
				moveSpeed,
				"",
				0,
				canDrown,
				diesEntombed,
				tempMinWarning,
				tempMaxWarning,
				tempMinLethal,
				tempMaxLethal);

			if (maxPenSize > 0)
				EntityTemplates.ExtendEntityToWildCreature(prefab, maxPenSize);

			if (drops != null)
				prefab.AddOrGet<Butcherable>().SetDrops(drops);

			if (trappable)
				prefab.AddOrGet<Trappable>();

			if (mass > 0)
				prefab.AddOrGet<PrimaryElement>().Mass = mass;

			if (baggable)
				EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true);

			switch (navType)
			{
				case NavType.Floor:
					kPrefabID.AddTag(GameTags.Creatures.Walker);
					prefab.AddOrGetDef<CreatureFallMonitor.Def>();
					break;
				case NavType.Swim:
					kPrefabID.AddTag(GameTags.Creatures.Swimmer);
					prefab.AddOrGetDef<CreatureFallMonitor.Def>();
					break;
				case NavType.Hover:
					if (navigationGrid == NAVIGATION.SLICKSTER)
						kPrefabID.AddTag(GameTags.Creatures.Hoverer);
					else
						kPrefabID.AddTag(GameTags.Creatures.Flyer);
					break;
			}

			if (defaultTemperature.HasValue && defaultTemperature > 0)
			{
				kPrefabID.prefabSpawnFn += go =>
				{
					if (go.TryGetComponent(out PrimaryElement primaryElement))
						primaryElement.SetTemperature(defaultTemperature.Value);
				};
			}

			babyCritterBuilder?.Build(prefab);

			weapon?.Apply();

			foreach (var tag in tags)
				kPrefabID.AddTag(tag);

			if (isBaby)
			{
				EntityTemplates.ExtendEntityToBeingABaby(prefab, parentId, dropOnGrowUp, forceAdultNavType, growUpOnCycle);
			}

			return prefab;
		}

		public WeaponBuilder Weapon()
		{
			weapon = new WeaponBuilder(this);
			return weapon;
		}

		public class WeaponBuilder(CritterBuilder instance)
		{
			private readonly CritterBuilder instance = instance;
			private AttackProperties properties = new AttackProperties();

			public WeaponBuilder Damage(float min, float max, AttackProperties.DamageType type = AttackProperties.DamageType.Standard)
			{
				properties.base_damage_min = min;
				properties.base_damage_max = max;
				properties.damageType = type;

				return this;
			}

			public WeaponBuilder AOE(float radius)
			{
				properties.aoe_radius = radius;
				return this;
			}

			public WeaponBuilder Target(AttackProperties.TargetType type)
			{
				properties.targetType = type;
				return this;
			}

			public WeaponBuilder Effect(string effect, float chance)
			{
				properties.effects ??= [];
				properties.effects.Add(new AttackEffect(effect, chance));

				return this;
			}

			public WeaponBuilder MaxHits(int hits)
			{
				properties.maxHits = hits;
				return this;
			}

			public CritterBuilder Done() => instance;

			public void Apply()
			{
				var weapon = instance.prefab.AddWeapon(
					properties.base_damage_min,
					properties.base_damage_max,
					properties.damageType,
					properties.targetType,
					properties.maxHits,
					properties.aoe_radius);

				if (properties.effects != null)
				{
					foreach (var effect in properties.effects)
						weapon.AddEffect(effect.effectID, effect.effectProbability);
				}
			}
		}

		public class BabyCritterBuilder(CritterBuilder parent, string babyId, string eggKanim)
		{
			private float mass = 1.0f;
			private bool ranchable = true;
			private float fertilityCycles = 50f;
			private float incubationCycles = 20f;
			private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			private int eggSortOrder = 10;
			private List<FertilityMonitor.BreedingChance> breedingChances = [];
			private readonly CritterBuilder parent = parent;

			public BabyCritterBuilder Fertility(float cycles)
			{
				this.fertilityCycles = cycles;
				return this;
			}

			public BabyCritterBuilder Incubation(float cycles)
			{
				this.incubationCycles = cycles;
				return this;
			}

			public BabyCritterBuilder NotRanchable()
			{
				ranchable = false;
				return this;
			}

			public BabyCritterBuilder Dlc(string[] dlcIds)
			{
				this.dlcIds = dlcIds;
				return this;
			}

			public BabyCritterBuilder EggChance(Tag egg, float weight)
			{
				this.breedingChances.Add(new FertilityMonitor.BreedingChance()
				{
					weight = weight,
					egg = egg,
				});

				return this;
			}

			public BabyCritterBuilder SortOrder(int eggSortOrder)
			{
				this.eggSortOrder = eggSortOrder;
				return this;
			}

			public BabyCritterBuilder Mass(float mass)
			{
				this.mass = mass;
				return this;
			}

			public void Build(GameObject prefab)
			{
				var parentId = prefab.PrefabID().ToString();
				var id = $"{parentId}Egg";

				Log.Debug("Added Egg: " + Strings.Get($"STRINGS.CREATURES.SPECIES.{parentId.ToUpperInvariant()}.EGG_NAME"));

				if (!Assets.TryGetAnim(eggKanim, out var _))
					Log.Warning($"Error configuring {parentId} {id}");

				EntityTemplates.ExtendEntityToFertileCreature(
					prefab,
					$"{parentId}Egg",
					Strings.Get($"STRINGS.CREATURES.SPECIES.{parentId.ToUpperInvariant()}.EGG_NAME"),
					parent.GetDescription(),
					eggKanim,
					mass,
					babyId,
					fertilityCycles,
					incubationCycles,
					breedingChances,
					dlcIds,
					eggSortOrder,
					is_ranchable: ranchable);
			}

			public CritterBuilder Done()
			{
				return parent;
			}
		}

		public class TraitsBuilder
		{
			private readonly Trait trait;
			private readonly string name;
			private readonly Db db;
			private readonly CritterBuilder instance;

			public TraitsBuilder(CritterBuilder instance)
			{
				db = Db.Get();
				name = instance.GetName();
				this.instance = instance;

				trait = db.CreateTrait(instance.id + "BaseTrait", name, name, null, false, null, true, true);
			}

			public TraitsBuilder Add(string modifier, float value, bool multiplier = false)
			{
				trait.Add(new AttributeModifier(modifier, value, is_multiplier: multiplier));
				return this;
			}

			public TraitsBuilder HP(float hp) => Add(db.Amounts.HitPoints.maxAttribute.Id, hp);

			public TraitsBuilder MaxAge(float cycles) => Add(db.Amounts.Age.maxAttribute.Id, cycles);

			public TraitsBuilder Stomach(float calCapacity, float dailyCal)
			{
				return Add(db.Amounts.Calories.maxAttribute.Id, calCapacity)
					.Add(db.Amounts.Calories.deltaAttribute.Id, dailyCal / CONSTS.CYCLE_LENGTH);
			}

			public CritterBuilder Done() => instance;
		}


		public class BrainBuilder : ChoreTable.Builder
		{
			public readonly CritterBuilder instance;
			private readonly Tag species;
			private HashSet<string> conditions = [];

			public BrainBuilder(CritterBuilder instance, Tag species)
			{
				this.instance = instance;
				this.species = species;

				if (!instance.isBaby)
					conditions.Add(ADULT);
			}

			public CritterBuilder Done() => instance;

			public void SetCondition(string condition) => conditions.Add(condition);

			public void Apply()
			{
				EntityTemplates.AddCreatureBrain(instance.prefab, this, species, null);
			}

			public CritterBuilder Configure(Action<BrainBuilder, HashSet<string>> configureAI)
			{
				configureAI(this, conditions);
				return instance;
			}
		}
	}
}
