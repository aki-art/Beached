using Beached.Content.Defs.Entities.Corals;
using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Mites;
using Beached.Content.Defs.Entities.Critters.SlickShells;
using Beached.Content.Defs.Flora;
using Beached.Content.Defs.Foods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TUNING;

namespace Beached.Content.WikiHelper
{
	public class FoodGraphGenerator
	{
		public static Dictionary<Tag, Input[]> preferredPaths = new()
		{
			{ "Water", null },
			{ SimHashes.DirtyWater.CreateTag(), null },
			{ "Dirt", null },
			{ "Meat", [new Input(MoleConfig.ID, 0.06f) ]},
			{ WashuSpongeConfig.SEED_ID, [new Input(WashuSpongeConfig.ID, 0f) ]},
			{ ShellfishMeatConfig.ID, [new Input(CrabFreshWaterConfig.ID, 0.25f) ]},
			{ RawSnailConfig.ID, [new Input(SlickShellConfig.ID, 1f) ]},
			{ FishMeatConfig.ID, [new Input(PacuConfig.ID, 1f) ]},
			{ CracklingsConfig.ID, [new Input(SlagmiteConfig.ID, 0.25f) ]},
			{ RawEggConfig.ID, [new Input(InfertileEggConfig.ID, 1f) ]},
			{ InfertileEggConfig.ID, [new Input(KaracooConfig.ID, 1f) ]},
			{ RawKelpConfig.ID, [new Input(KelpConfig.ID, 0.5f) ]},
		};

		public struct Input(Tag tag, float amount)
		{
			public Tag tag = tag;
			public float amount = amount;
		}

		public static void Generate(string outputPath)
		{
			var builder = new StringBuilder();
			var foods = Assets.GetPrefabsWithComponent<Edible>();
			var recipes = ComplexRecipeManager.Get();
			var set = new HashSet<Tag>();

			foreach (var food in foods)
			{
				if (!food.PrefabID().name.StartsWith("Beached_"))
					continue;

				set.Clear();

				var amountMultiplier = EdiblesManager.GetFoodInfo(food.PrefabID().Name).CaloriesPerUnit;
				amountMultiplier = 1_000_000f / amountMultiplier;

				var tag = food.PrefabID();
				builder.AppendLine("``` mermaid");
				builder.AppendLine("flowchart TD");
				AppendItem(tag, 1f, amountMultiplier, builder, recipes, ref set);
				builder.AppendLine("```");
				builder.AppendLine();
				builder.AppendLine();
			}

			File.WriteAllText(outputPath, builder.ToString());
		}

		private static void AppendItem(Tag parentTag, float baseAmount, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			var id = parentTag.name;

			if (set.Contains(id))
				return;

			set.Add(id);
			try
			{
				var edible = EdiblesManager.GetFoodInfo(parentTag.Name);
				var isEdible = edible != null && edible.CaloriesPerUnit > 0;

				var unit = isEdible ? "kcal" : "";
				var amountFormatted = (baseAmount * amountMultiplier).ToString("0.##");

				if (isEdible)
					amountFormatted = $"{GameUtil.GetFormattedCaloriesForItem(parentTag, baseAmount * amountMultiplier)} ({amountFormatted})";

				if (ElementLoader.FindElementByName(parentTag.name) != null)
					amountFormatted = GameUtil.GetFormattedMass(baseAmount * amountMultiplier);

				if (parentTag == KaracooConfig.ID)
				{
					amountFormatted = $"~{amountFormatted}";
				}

				builder.AppendLine($"    {id}(\"<img height=40 src='../../img/icons/{id.ToLower()}.png' /> </br>{amountFormatted}</br> <b>{parentTag.ProperNameStripLink()}</b>\")");

				if (preferredPaths.TryGetValue(parentTag, out var inputs))
				{
					if (inputs == null)
						return;

					foreach (var input in inputs)
					{
						AppendItem(input.tag, input.amount, amountMultiplier, builder, recipes, ref set);
						builder.AppendLine($"    {input.tag.name} --> {parentTag.name}");
					}
				}
				else if (GenerateCropIngredients(parentTag, baseAmount, amountMultiplier, builder, recipes, ref set)) { }
				else
				{
					if (GenerateCropInputs(parentTag, amountMultiplier, builder, recipes, ref set))
						return;

					if (GenerateSeedInputs(parentTag, amountMultiplier, builder, recipes, ref set))
						return;
					//GenerateCritterDrops(parentTag, amountMultiplier, builder, recipes, ref set);
					GenerateRecipeIngredients(parentTag, amountMultiplier, builder, recipes, ref set);
				}

			}
			catch (Exception e)
			{
				Log.Warning($"{e.GetType()} {e.Message} {e.StackTrace}");
			}
		}

		private static bool GenerateSeedInputs(Tag parentTag, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			var prefabs = Assets.GetPrefabsWithComponent<SeedProducer>();


			foreach (var prefab in prefabs)
			{
				var seedProducer = prefab.GetComponent<SeedProducer>();
				if (seedProducer.seedInfo.seedId == parentTag)
				{
					var producedSeedsPerHarvest = seedProducer.seedInfo.newSeedsProduced;
					var cyclesForPlant = seedProducer.GetComponent<Crop>().cropVal.cropDuration;

					var cyclePerSeed = cyclesForPlant / producedSeedsPerHarvest;

					AppendItem(seedProducer.PrefabID(), cyclePerSeed, amountMultiplier, builder, recipes, ref set);
					builder.AppendLine($"    {seedProducer.PrefabID()} --> {parentTag.name}");

					return true;
				}
			}

			return false;
		}

		private static void GenerateRecipeIngredients(Tag parentTag, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			var recipe = recipes.recipes.Find(
			r => r.results != null
			&& r.results.Any(result => result.material == parentTag));

			if (recipe != null && recipe.ingredients != null)
			{
				foreach (var item in recipe.ingredients)
				{
					if (set.Contains(item.material))
					{
						Log.Warning("Circular recipe detected: " + item.material.name);
						continue;
					}

					var outputAmount = recipe.results.First(recipe => recipe.material == parentTag).amount;

					AppendItem(item.material, item.amount / outputAmount, amountMultiplier, builder, recipes, ref set);

					builder.AppendLine($"    {item.material.name} --> {parentTag.name}");
				}
			}
		}

		private static void GenerateCritterDrops(Tag parentTag, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			foreach (var critter in Assets.GetPrefabsWithTag(GameTags.CreatureBrain))
			{
				var calorieMonitor = critter.GetDef<CreatureCalorieMonitor.Def>();
				if (calorieMonitor != null)
				{
					foreach (var diet in calorieMonitor.diet.infos)
					{
						if (diet.producedElement == parentTag)
						{
							foreach (var element in diet.consumedTags)
							{
								AppendItem(element, 0, amountMultiplier, builder, recipes, ref set);
								builder.AppendLine($"    {element} --> {parentTag.name}");
							}

							return;
						}
					}
				}

				var butcherable = critter.GetComponent<Butcherable>();
				if (butcherable != null && butcherable.drops != null)
				{
					foreach (var drop in butcherable.drops.Distinct())
					{
						AppendItem(drop, butcherable.drops.Count(d => d == drop), amountMultiplier, builder, recipes, ref set);
						builder.AppendLine($"    {drop} --> {parentTag.name}");
					}
				}
			}
		}

		private static bool GenerateCropInputs(Tag parentTag, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			var plant = Assets.TryGetPrefab(parentTag);
			if (plant == null)
				return false;

			var irrigation = plant.GetDef<IrrigationMonitor.Def>();
			if (irrigation != null)
			{
				foreach (var element in irrigation.consumedElements)
				{
					AppendItem(element.tag, element.massConsumptionRate, amountMultiplier, builder, recipes, ref set);
					builder.AppendLine($"    {element.tag} --> {parentTag.name}");
				}

				return true;
			}

			var fertilizable = plant.GetDef<FertilizationMonitor.Def>();

			if (fertilizable != null)
			{
				foreach (var element in fertilizable.consumedElements)
				{
					AppendItem(element.tag, element.massConsumptionRate, amountMultiplier, builder, recipes, ref set);
					builder.AppendLine($"    {element.tag} --> {parentTag.name}");
				}

				return true;
			}

			return false;
		}

		private static bool GenerateCropIngredients(Tag parentTag, float baseAmount, float amountMultiplier, StringBuilder builder, ComplexRecipeManager recipes, ref HashSet<Tag> set)
		{
			var cropInfoIdx = CROPS.CROP_TYPES.FindIndex(c => c.cropId == parentTag.name);
			if (cropInfoIdx != -1)
			{
				var cropInfo = CROPS.CROP_TYPES[cropInfoIdx];

				var prefab = Assets.GetPrefab(parentTag);
				foreach (var plant in Assets.GetPrefabsWithComponent<Crop>())
				{
					if (plant.GetComponent<Crop>().cropId == cropInfo.cropId)
					{
						var cropMass = prefab.GetComponent<PrimaryElement>().Mass;
						var cyclesToGrowOnePlant = cropInfo.cropDuration / 600f;
						var oneCycleProduction = (1f / cyclesToGrowOnePlant) / cropMass;
						var minimumPlantsNeeded = baseAmount / oneCycleProduction;


						AppendItem(plant.PrefabID(), minimumPlantsNeeded, amountMultiplier, builder, recipes, ref set);
						builder.AppendLine($"    {plant.PrefabID().name} --> {parentTag.name}");

						return true;
					}
				}
			}

			return false;
		}
	}
}
