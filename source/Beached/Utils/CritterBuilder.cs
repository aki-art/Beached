using System.Collections.Generic;
using UnityEngine;

namespace Beached.Utils
{
	public class CritterBuilder
	{
		private GameObject prefab;
		int width, height;
		string ID;
		string animFile, defaultAnimation;
		string navGrid;
		EffectorValues decor;
		NavType navType;
		float speed = 2;
		int maxProbingRadius = 32;
		(string, int)[] drops;
		FactionManager.FactionID faction = FactionManager.FactionID.Friendly;
		float mass = 30;
		float minK, lowK, highK, maxK;
		bool canDrown, canEntomb;
		string name, description;
		bool baby;

		public static CritterBuilder Create(string ID, string animFile, string defaultAnimation = "idle_loop")
		{
			return new CritterBuilder()
			{
				ID = ID,
				animFile = animFile,
				defaultAnimation = defaultAnimation
			};
		}

		public CritterBuilder Baby()
		{
			baby = true;
			return this;
		}

		public CritterBuilder Name(string name)
		{
			this.name = name;
			return this;
		}

		public CritterBuilder Description(string description)
		{
			this.description = description;
			return this;
		}

		public CritterBuilder Mass(float mass)
		{
			this.mass = mass;
			return this;
		}

		public CritterBuilder Decor(EffectorValues decor)
		{
			this.decor = decor;
			return this;
		}

		public CritterBuilder Size(int width, int height)
		{
			this.width = width;
			this.height = height;
			return this;
		}

		public CritterBuilder Navigation(string navGrid, NavType navType, float speed = 2f, int maxProbingRadius = 32)
		{
			this.navGrid = navGrid;
			this.navType = navType;
			this.speed = speed;
			this.maxProbingRadius = maxProbingRadius;

			return this;
		}

		public CritterBuilder DeathDrops(params (string tag, int amount)[] drops)
		{
			this.drops = drops;
			return this;
		}

		public CritterBuilder Faction(FactionManager.FactionID faction)
		{
			this.faction = faction;
			return this;
		}

		public CritterBuilder ShedFur(float amount, Color color)
		{
			Integration.CritterShedding.Shed(ID, amount, color);
			return this;
		}

		public CritterBuilder Temperatures(float cold, float fatalCold, float hot, float fatalHot)
		{
			minK = GameUtil.GetTemperatureConvertedToKelvin(fatalCold, GameUtil.TemperatureUnit.Celsius);
			lowK = GameUtil.GetTemperatureConvertedToKelvin(cold, GameUtil.TemperatureUnit.Celsius);
			highK = GameUtil.GetTemperatureConvertedToKelvin(hot, GameUtil.TemperatureUnit.Celsius);
			maxK = GameUtil.GetTemperatureConvertedToKelvin(fatalHot, GameUtil.TemperatureUnit.Celsius);

			return this;
		}

		public GameObject Build()
		{
			name ??= Strings.Get($"STRINGS_OLD.CREATURES.SPECIES.{ID.ToUpperInvariant()}.NAME");
			description ??= Strings.Get($"STRINGS_OLD.CREATURES.SPECIES.{ID.ToUpperInvariant()}.DESC");

			prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				name,
				description,
				mass,
				Assets.GetAnim(animFile),
				defaultAnimation,
				Grid.SceneLayer.Creatures,
				width,
				height,
				decor);

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				faction,
				ID + "Original",
				navGrid,
				navType,
				maxProbingRadius,
				speed,
				"",
				0,
				canDrown,
				canEntomb,
				lowK,
				highK,
				minK,
			maxK);

			if (drops != null)
			{
				var flatDrops = new List<string>();
				foreach (var drop in drops)
				{
					for (int i = 0; i < drop.Item2; i++)
						flatDrops.Add(drop.Item1);
				}

				prefab.AddOrGet<Butcherable>().SetDrops(flatDrops.ToArray());
			}

			return prefab;
		}

		public CritterBuilder CanDrown()
		{
			canDrown = true;
			return this;
		}

		public CritterBuilder CanEntomb()
		{
			canEntomb = true;
			return this;
		}

		/*		public BrainBuilder AddBrain(Tag species)
				{
					return new BrainBuilder(species, prefab);
				}

				public class BrainBuilder(Tag species, GameObject prefab)
				{
					Tag species = species;
					private GameObject prefab = prefab;
					ChoreTable.Builder choreTableBuilder;

					public BrainBuilder Add<SmType>(params object[] parameters) where SmType : StateMachine.BaseDef, new()
					{
						var sm = Activator.CreateInstance(typeof(SmType), parameters) as SmType;
						choreTableBuilder.Add(sm);
						var type = sm.GetType();

						if (type == typeof(FallStates.Def))
							prefab.AddOrGetDef<CreatureFallMonitor.Def>();
						else if(type == typeof(FallStates.Def))
					}
				}*/
	}
}
