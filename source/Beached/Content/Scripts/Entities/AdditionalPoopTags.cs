using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	// Allows a critter to have multiple different resources as excretion
	public class AdditionalPoopTags : KMonoBehaviour
	{
		[SerializeField] public Entry[] entries; // TODO: unimplemented
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
			var element = ElementLoader.FindElementByHash(Elements.slag);
			var temperature = GetComponent<PrimaryElement>().Temperature;

			element.substance.SpawnResource(Grid.CellToPosCCC(targetCell, Grid.SceneLayer.Ore) + offset, mass, temperature, byte.MaxValue, 0);

			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, transform);
		}

		public void ModifyCalorieDescriptors(ref List<Descriptor> descriptors)
		{
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

		[Serializable]
		public struct Entry
		{
			public Tag tag;
			public float ratioToOutput;
		}
	}
}
