using Beached.Content.Scripts;
using FUtility.FUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Beached.Content.Codex.Beached_ArcheologyCodexWidget;

namespace Beached.Content.Codex
{
	public class Beached_ArcheologyCodexWidget(Tag tag, TreasureChances.TreasureSource source, ElementUsage @in, WeightedElementUsage[] outs) : CodexWidget<Beached_ArcheologyCodexWidget>
	{
		private LocText label;
		private GameObject materialPrefab;
		private GameObject ingredientsContainer;
		private GameObject resultsContainer;
		private GameObject skillIconContainer;
		private Tag tag = tag;
		private WeightedElementUsage[] outs = outs;

		public class WeightedElementUsage(
			Tag tag,
			float amount,
			float chance,
			bool continuous = false,
			System.Func<Tag, float, bool, string> customFormating = null) : ElementUsage(tag, amount, continuous, customFormating)
		{
			public readonly float chance = chance;
		}

		public static void ConfigurePrefab(CodexScreen codexScreen)
		{
			var prefab = Object.Instantiate(codexScreen.PrefabTemperatureTransitionPanel);

			var arrow = prefab.transform.Find("Contents/Arrow");
			if (arrow != null)
			{
				// a little scuffed but disabling it breaks the whole layout
				//arrow.GetComponent<Image>().color = new Color(0, 0, 0, 0);
				arrow.GetComponent<Image>().enabled = false;
				arrow.gameObject.AddOrGet<CanvasGroup>().blocksRaycasts = false;
			}


			var references = prefab.GetComponent<HierarchyReferences>();
			if (references.TryGetReference<RectTransform>("TemperaturePanel", out var fabricator))
			{
				var fabricatorReferences = fabricator.GetComponent<HierarchyReferences>();
				if (fabricatorReferences.TryGetReference<Image>("Icon", out var icon))
				{
					var uiSprite1 = Assets.GetSprite("beached_archeology_codex_conversion_icon");
					icon.sprite = uiSprite1;

					var rect = icon.GetComponent<RectTransform>();

					rect.sizeDelta = new Vector2(0, 20);
					rect.localScale *= 1.25f;
				}
				else
					Log.Warning("Beached_ArcheologyCodexWidget - no Icon reference");

				if (references.TryGetReference<RectTransform>("SourceContainer", out var source))
					source.GetComponent<RectTransform>().sizeDelta = new Vector2(-30.0f, 0);
			}
			else
				Log.Warning("Beached_ArcheologyCodexWidget - no TemperaturePanel reference");

			codexScreen.ContentPrefabs[typeof(Beached_ArcheologyCodexWidget)] = prefab;
		}

		public static Beached_ArcheologyCodexWidget Generate(Tag tag, TreasureChances.TreasureSource source)
		{
			var baseChance = source.extraLootChance;
			var totalWeight = source.treasures.Sum(t => t.weight);

			var @in = new ElementUsage(tag, 1000, false);
			var outs = source.treasures
				.Select(t => new WeightedElementUsage(
					t.tag,
					t.amount,
					baseChance * (t.weight / totalWeight),
					false,
					(tag, amount, continous) =>
					{
						var percentChanceStr = $"{STRINGS.CREATURES.SPECIES.BEACHED_SLAGMITE.ODDS} {GameUtil.GetStandardPercentageFloat(baseChance * ((t.weight / totalWeight) * 100f))}{(global::STRINGS.UI.UNITSUFFIXES.PERCENT)}";

						return $"{percentChanceStr}";//\n{GameUtil.GetFormattedMass(amount)}";
					}))
				.ToArray();

			return new Beached_ArcheologyCodexWidget(tag, source, @in, outs);
		}

		public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
		{
			var component = contentGameObject.GetComponent<HierarchyReferences>();
			label = component.GetReference<LocText>("HeaderLabel");
			materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
			ingredientsContainer = component.GetReference<RectTransform>("SourceContainer").gameObject;
			resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
			skillIconContainer = component.GetReference<RectTransform>("TemperaturePanel").gameObject;

			ClearPanel();
			ConfigureConversion();
		}

		private void ConfigureConversion()
		{
			label.text = $"Excavating {tag.ProperName()}"; // TODO STRING

			if (skillIconContainer.TryGetComponent(out HierarchyReferences skillRefs))
			{
				if (skillRefs.TryGetReference<Image>("Icon", out var icon))
				{
					icon.sprite = Assets.GetSprite("beached_archeology_codex_conversion_icon");

					icon.preserveAspect = true;
					icon.sprite.texture.filterMode = FilterMode.Bilinear;

					icon.transform.parent.GetComponent<LayoutElement>().minHeight = 86;

					if (!icon.TryGetComponent(out
					FButton button))
					{
						button = icon.gameObject.AddComponent<FButton>();
						button.disabledColor = button.hoverColor = button.normalColor = Color.white;
					}

					button.OnClick += () =>
					{
						ManagementMenu.Instance.codexScreen.ChangeArticle(global::STRINGS.UI.ExtractLinkID(STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME));
					};

					var tooltipMessage = $"Naturally spawned {@in.tag.ProperName()} has a chance to drop these materials when dug by a duplicant with the {STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME} skill."; // TODO STRING;

					if (!icon.TryGetComponent(out ToolTip tooltip))
						Helper.AddSimpleToolTip(icon.gameObject, tooltipMessage, wrapWidth: 120.0f);
					else
						tooltip.toolTip = tooltipMessage;
				}

				if (skillRefs.TryGetReference<LocText>("Label", out var label))
				{
					label.text = $"<size=80%><b>{STRINGS.DUPLICANTS.ROLES.ARCHEOLOGY.NAME}</size>";
					label.color = Color.black;

					label.enableWordWrapping = false;
					label.overflowMode = TMPro.TextOverflowModes.Overflow;
					label.autoSizeTextContainer = true;
				}

			}

			ConfigureItem(materialPrefab, ingredientsContainer, @in);

			foreach (var @out in outs)
				ConfigureItem(materialPrefab, resultsContainer, @out);
		}

		private void ConfigureItem(GameObject prefab, GameObject parent, ElementUsage elementUsage)
		{
			var inRefs = Util.KInstantiateUI(prefab, parent, true).GetComponent<HierarchyReferences>();

			var uiSprite = GetUISprite(elementUsage.tag);
			if (uiSprite != null)
			{
				if (inRefs.TryGetReference<Image>("Icon", out var icon))
				{
					icon.sprite = uiSprite.first;
					icon.color = uiSprite.second;
				}
			}

			if (inRefs.TryGetReference<LocText>("Title", out var amountLabel))
			{
				amountLabel.text = elementUsage.customFormating == null
					? GameUtil.GetFormattedByTag(tag, elementUsage.amount, GameUtil.TimeSlice.None)
					: elementUsage.customFormating(tag, elementUsage.amount, false);

				amountLabel.color = Color.black;
			}

			if (inRefs.TryGetReference<ToolTip>("ToolTip", out var tooltip))
				tooltip.toolTip = elementUsage.tag.ProperName();

			if (inRefs.TryGetReference<KButton>("Button", out var button))
			{
				button.onClick += (() => ManagementMenu.Instance.codexScreen.ChangeArticle(global::STRINGS.UI.ExtractLinkID(tag.ProperName())));
			}
		}

		private static Tuple<Sprite, Color> GetUISprite(Tag tag)
		{
			if (ElementLoader.GetElement(tag) != null)
				return Def.GetUISprite(ElementLoader.GetElement(tag));

			if (Assets.GetPrefab(tag) != null)
				return Def.GetUISprite(Assets.GetPrefab(tag));

			return Assets.GetSprite(tag.Name) != null
				? new Tuple<Sprite, Color>(Assets.GetSprite(tag.Name), Color.white)
				: Def.GetUISprite(tag);
		}


		private void ClearPanel()
		{
			foreach (Component component in ingredientsContainer.transform)
				Object.Destroy(component.gameObject);

			foreach (Component component in resultsContainer.transform)
				Object.Destroy(component.gameObject);
		}
	}
}
