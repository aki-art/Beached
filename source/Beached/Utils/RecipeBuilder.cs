using System.Collections.Generic;
using static ComplexRecipe;
using static ComplexRecipe.RecipeElement;

namespace Beached.Utils
{
	public class RecipeBuilder
	{
		private string fabricator;
		private float time;
		private RecipeNameDisplay nameDisplay;
		private string description;
		private string requiredTech;
		private int sortOrder;
		private int visualizerIdx = -1;

		private List<RecipeElement> inputs;
		private List<RecipeElement> outputs;

		public static RecipeBuilder Create(string fabricatorID, string description, float time)
		{
			var builder = new RecipeBuilder
			{
				fabricator = fabricatorID,
				description = description,
				time = time,
				nameDisplay = RecipeNameDisplay.IngredientToResult,
				inputs = [],
				outputs = []
			};

			return builder;
		}

		public RecipeBuilder Visualizer(int idx = 0)
		{
			visualizerIdx = idx;
			return this;
		}

		public RecipeBuilder NameDisplay(RecipeNameDisplay nameDisplay)
		{
			this.nameDisplay = nameDisplay;
			return this;
		}

		public RecipeBuilder Input(Tag tag, float amount, bool inheritElement = true)
		{
			inputs.Add(new RecipeElement(tag, amount, inheritElement));
			return this;
		}

		public RecipeBuilder Output(Tag tag, float amount, TemperatureOperation tempOp = TemperatureOperation.AverageTemperature)
		{
			outputs.Add(new RecipeElement(tag, amount, tempOp));
			return this;
		}

		public RecipeBuilder FacadeOutput(Tag tag, float amount, string facadeID = "", bool storeElement = false, TemperatureOperation tempOp = TemperatureOperation.AverageTemperature)
		{
			outputs.Add(new RecipeElement(tag, amount, tempOp, facadeID, storeElement));
			return this;
		}

		public RecipeBuilder RequireTech(string tech)
		{
			requiredTech = tech;
			return this;
		}

		public RecipeBuilder SortOrder(int sortOrder)
		{
			this.sortOrder = sortOrder;
			return this;
		}

		public ComplexRecipe Build(string facadeID = "")
		{
			var i = inputs.ToArray();
			var o = outputs.ToArray();

			var recipeID = facadeID.IsNullOrWhiteSpace() ? ComplexRecipeManager.MakeRecipeID(fabricator, i, o) : ComplexRecipeManager.MakeRecipeID(fabricator, i, o, facadeID);

			var obsoleteId = ComplexRecipeManager.MakeObsoleteRecipeID(fabricator, inputs[0].material);
			ComplexRecipeManager.Get().AddObsoleteIDMapping(obsoleteId, recipeID);

			var recipe = new ComplexRecipe(recipeID, i, o)
			{
				time = time,
				description = description,
				nameDisplay = nameDisplay,
				fabricators = [fabricator],
				requiredTech = requiredTech,
			};

			if (visualizerIdx > -1)
			{
				if (o.Length >= visualizerIdx)
					Log.Warning($"Cannot set visualizer to idx {visualizerIdx}, there is only {o.Length} items.");
				else
				{
					var prefab = Assets.GetPrefab(o[visualizerIdx].material);

					if (prefab != null)
						recipe.FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(prefab);
					else
						Log.Debug($"No prefab with id {o[visualizerIdx].material}");
				}
			}

			return recipe;
		}
	}
}