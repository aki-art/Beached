using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	// Allows a critter to have multiple different resources as excretion
	public class AdditionalPoopTags : KMonoBehaviour, ICodexEntry
	{
		[SerializeField] public Entry[] entries;
		[SerializeField] public Vector3 offset;
		[Tooltip("The final ratio of the resulting poop, where 100% includes the poop.")]
		[SerializeField] public float totalMassPercent01;

		private float massMultiplier;
		private float lastKnownPoopSize;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)ModHashes.prePoop, BeforePoop);
			Subscribe((int)GameHashes.Poop, AfterPoop);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			massMultiplier = 1f / ((1f / totalMassPercent01) - 1f);
		}

		private float GetTotalPoop() => Game.Instance.savedInfo.creaturePoopAmount.GetOrDefault(this.PrefabID(), 0);

		private void BeforePoop(object _)
		{
			lastKnownPoopSize = GetTotalPoop();
		}

		private void AfterPoop(object _)
		{
			var poopSize = GetTotalPoop() - lastKnownPoopSize;
			var mass = poopSize * massMultiplier;

			if (mass <= 0)
				return;

			var targetCell = Grid.PosToCell(transform.GetPosition());
			foreach (var entry in entries)
			{
				var temperature = GetComponent<PrimaryElement>().Temperature;

				var element = ElementLoader.FindElementByTag(entry.tag);
				if (element != null)
				{
					element.substance.SpawnResource(Grid.CellToPosCCC(targetCell, Grid.SceneLayer.Ore) + offset, mass, temperature, byte.MaxValue, 0);
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, transform);
				}
				else
				{
					var item = FUtility.Utils.Spawn(entry.tag, transform.position + offset);
					item.GetComponent<PrimaryElement>().SetMassTemperature(mass, temperature);
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, item.GetProperName(), transform);
				}
			}
		}

		public void ModifyCalorieDescriptors(ref List<Descriptor> descriptors)
		{
			if (descriptors == null)
			{
				Log.Debug("descriptors were null??");
				descriptors = [];
			}

			if (entries == null)
			{
				Log.Debug("entries were null??");
				return;
			}

			foreach (var entry in entries)
			{
				var slag = entry.tag.ProperName();
				descriptors.Add(new Descriptor(
					STRINGS.UI.DIET.EXTRA_PRODUCE
						.Replace("{tag}", slag),
					STRINGS.UI.DIET.EXTRA_PRODUCE_TOOLTIP
						.Replace("{tag}", slag)
						.Replace("{percent}", GameUtil.GetFormattedPercent(totalMassPercent01 * 100f))));
			}
		}

		public void AddCodexEntries(CodexEntryGenerator_Elements.ElementEntryContext context, KPrefabID prefab)
		{
			var diets = DietManager.Instance.GetPrefabDiet(prefab.gameObject);

			if (diets == null)
				return;

			foreach (var entry in entries)
			{
				foreach (var info in diets.infos)
				{
					if (context.madeMap != null && context.madeMap.map.TryGetValue(info.producedElement, out var conversions))
					{
						foreach (var conversion in conversions)
						{
							if (!conversion.prefab.IsPrefabID(prefab.PrefabID()))
								continue;

							if (conversion.outSet == null || conversion.outSet.Count == 0)
								continue;

							var firstOut = conversion.outSet.ToList().Find(usage => usage.tag == info.producedElement);
							if (firstOut == null)
								continue;

							conversion.outSet
								.Add(new ElementUsage(entry.tag, firstOut.amount * entry.ratioToOutput, firstOut.continuous));

							context.madeMap.map
								.GetOrAdd(entry.tag, () => [])
								.Add(conversion);
						}
					}
				}
			}
		}

		public int CodexEntrySortOrder() => 0;

		[Serializable]
		public struct Entry
		{
			public Tag tag;
			public float ratioToOutput;
		}
	}
}
