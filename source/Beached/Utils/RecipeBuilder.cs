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
		private string visualizerAnim;

		public static RecipeBuilder Create(string fabricatorID, float time)
		{
			return Create(fabricatorID, null, time);
		}

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

		public RecipeBuilder Input(Dictionary<Tag, float> ins, bool inheritElement = true)
		{
			Input([.. ins.Keys], [.. ins.Values], inheritElement);
			return this;
		}

		public RecipeBuilder Input(Tag tag, float amount = 1f, bool inheritElement = true)
		{
			inputs.Add(new RecipeElement(tag, amount, inheritElement));
			return this;
		}

		public RecipeBuilder Input(Tag[] tags, float[] amounts, bool inheritElement = true)
		{
			inputs.Add(new RecipeElement(tags, amounts)
			{
				inheritElement = inheritElement
			});

			return this;
		}

		public RecipeBuilder Input(Tag[] tags, float amount = 1f, bool inheritElement = true)
		{
			inputs.Add(new RecipeElement(tags, amount)
			{
				inheritElement = inheritElement
			});

			return this;
		}

		public RecipeBuilder Output(Tag tag, float amount = 1f, TemperatureOperation tempOp = TemperatureOperation.AverageTemperature, bool store = false)
		{
			outputs.Add(new RecipeElement(tag, amount, tempOp, store));
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

			//recipeID = $"Beached_{recipeID}"; // this breaks codex recipe listings :(

			recipeID = $"{recipeID}_Beached";

			var obsoleteId = ComplexRecipeManager.MakeObsoleteRecipeID(fabricator, inputs[0].material);
			ComplexRecipeManager.Get().AddObsoleteIDMapping(obsoleteId, recipeID);

			if (description == null)
			{
				Log.Warning("No description for recipe " + recipeID);
				description = "";
			}

			var recipe = new ComplexRecipe(recipeID, i, o)
			{
				time = time,
				description = description,
				nameDisplay = nameDisplay,
				fabricators = [fabricator],
				requiredTech = requiredTech,
			};

			if (!visualizerAnim.IsNullOrWhiteSpace())
				recipe.SetFabricationAnim(visualizerAnim);

			return recipe;
		}

		public RecipeBuilder Visualizer(string animFile)
		{
			this.visualizerAnim = animFile;
			return this;
		}
	}
}