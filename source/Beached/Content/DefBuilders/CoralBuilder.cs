using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Scripts.Entities.Plant;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.DefBuilders
{
	public class CoralBuilder
	{
		public GameObject seedPrefab;
		private string name;
		private string description;
		private string ID;
		private int width, height;
		private string animName, initialAnim;
		private List<Tag> tags = [BTags.aquaticPlant, BTags.coral];
		private EffectorValues decor;
		private float
			tempMinLethal = MiscUtil.CelsiusToKelvin(0f),
			tempMinWarning = MiscUtil.CelsiusToKelvin(10f),
			tempMaxLethal = MiscUtil.CelsiusToKelvin(40f),
			tempMaxWarning = MiscUtil.CelsiusToKelvin(50f);
		private SimHashes[] safeElements = CoralTemplate.ALL_WATERS;
		private float minRadiation = 0, maxRadiation = 7500f;
		private float minPressure = 0, lowPressure = 0.15f;
		private bool pressureSensitive = false;
		private float maxAge = 2400f;

		private float seedW = 0.25f, seedH = 0.25f;
		private string seedAnimName;
		private List<Tag> seedTags;
		private int seedSortOrder = 0;
		private bool harvestable = false;
		private float defaultTemperaure = 293f;

		public CoralBuilder(string ID, string animName)
		{
			this.ID = ID;
			width = 1;
			height = 1;
			this.animName = animName;
			initialAnim = "idle_grown";
			decor = TUNING.DECOR.NONE;
			safeElements = CoralTemplate.ALL_WATERS;
			name = Strings.Get($"STRINGS.CREATURES.SPECIES.{ID.ToUpperInvariant()}.NAME");
			description = Strings.Get($"STRINGS.CREATURES.SPECIES.{ID.ToUpperInvariant()}.DESC");
		}

		public CoralBuilder Width(int width)
		{
			this.width = width;
			return this;
		}

		public CoralBuilder Height(int height)
		{
			this.height = height;
			return this;
		}

		public CoralBuilder InitialAnim(string initialAnim)
		{
			this.initialAnim = initialAnim;
			return this;
		}

		public CoralBuilder AddTags(params Tag[] tags)
		{
			this.tags.AddRange([.. tags]);
			return this;
		}

		public CoralBuilder Decor(EffectorValues decor)
		{
			this.decor = decor;
			return this;
		}

		public CoralBuilder SafeTemperaturesCelsius(float min, float low, float high, float max)
		{
			tempMinLethal = MiscUtil.CelsiusToKelvin(min);
			tempMinWarning = MiscUtil.CelsiusToKelvin(low);
			tempMaxWarning = MiscUtil.CelsiusToKelvin(high);
			tempMaxLethal = MiscUtil.CelsiusToKelvin(max);

			return this;
		}

		public CoralBuilder SafeTemperaturesKelvin(float min, float low, float high, float max)
		{
			tempMinLethal = min;
			tempMinWarning = low;
			tempMaxWarning = high;
			tempMaxLethal = max;

			return this;
		}

		public CoralBuilder SafeIn(params SimHashes[] elements)
		{
			safeElements = elements;
			return this;
		}

		public CoralBuilder Radiation(float min, float max)
		{
			minRadiation = min;
			maxRadiation = max;
			return this;
		}

		public CoralBuilder SafePressure(float min = 0, float low = 0.15f)
		{
			minPressure = min;
			lowPressure = low;
			pressureSensitive = true;
			return this;
		}

		public CoralBuilder Age(float age)
		{
			maxAge = age;
			return this;
		}

		public CoralBuilder DefaultTemperatureCelsius(float temp)
		{
			defaultTemperaure = MiscUtil.CelsiusToKelvin(temp);
			return this;
		}

		public CoralBuilder Harvestable()
		{
			harvestable = true;
			return this;
		}

		public CoralBuilder Frag(string anim, float width = 0.25f, float height = 0.25f, int sortOrder = 0, params Tag[] additionalTags)
		{
			seedAnimName = anim;
			seedW = width; seedH = height;
			seedTags = [BTags.aquaticPlant, BTags.coral, BTags.smallAquariumSeed];
			seedSortOrder = sortOrder;

			if (additionalTags != null)
			{
				seedTags.AddRange(additionalTags);
			}

			return this;
		}

		public CoralBuilder GrowDirection(SingleEntityReceptacle.ReceptacleDirection direction)
		{
			if (seedPrefab != null && seedPrefab.TryGetComponent(out PlantableSeed seed))
				seed.direction = direction;

			return this;
		}

		public (GameObject entityPrefab, GameObject seedPrefab) Build()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				name,
				description,
				10f,
				Assets.GetAnim(animName),
				initialAnim,
				Grid.SceneLayer.BuildingBack,
				width,
				height,
				decor,
				additionalTags: tags,
				defaultTemperature: defaultTemperaure);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				tempMinLethal,
				tempMinWarning,
				tempMaxWarning,
				tempMaxLethal,
				safeElements,
				pressureSensitive,
				minPressure,
				lowPressure,
				null,
				false,
				true,
				true,
				true,
				maxAge,
				minRadiation,
				maxRadiation,
				ID + "Original",
				name);

			prefab.AddOrGet<SubmersionMonitor>();
			prefab.AddOrGet<LoopingSounds>();
			prefab.AddOrGet<Prioritizable>();

			prefab.AddOrGet<AquariumPlantedMonitor>();

			if (harvestable)
			{
				prefab.AddOrGet<Harvestable>();
				prefab.AddOrGet<HarvestDesignatable>().defaultHarvestStateWhenPlanted = false;
			}


			if (seedAnimName != null)
			{
				var name = Strings.Get($"STRINGS.CREATURES.SPECIES.SEEDS.{ID.ToUpperInvariant()}.NAME");
				var description = Strings.Get($"STRINGS.CREATURES.SPECIES.SEEDS.{ID.ToUpperInvariant()}.DESC");
				var domesicatedDesc = Strings.Get($"STRINGS.CREATURES.SPECIES.{ID.ToUpperInvariant()}.DOMESTICATEDDESC");

				seedPrefab = EntityTemplates.CreateAndRegisterSeedForPlant(
					prefab,
					null,
					SeedProducer.ProductionType.DigOnly,
					ID + "Seed",
					name,
					description,
					Assets.GetAnim(seedAnimName),
					additionalTags: seedTags,
					sortOrder: seedSortOrder,
					domesticatedDescription: domesicatedDesc ?? this.description,
					width: seedW,
					height: seedH,
					ignoreDefaultSeedTag: true);

				EntityTemplates.CreateAndRegisterPreviewForPlant(
					seedPrefab,
					ID + "Preview",
					Assets.GetAnim(animName),
					"place",
					width,
					height);
			}

			return (prefab, seedPrefab);
		}
	}
}
