using FUtility;
using Klei.AI;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BGeyserTraits
	{
		public const string GEYSER_GROUP = "Beached_GeyserTraits";

		public const string
			LARGE = "Beached_Geyser_Large",
			SMALL = "Beached_Geyser_Small",
			LONG_CYCLE = "Beached_Geyser_LongCycle",
			RAPID_CYCLE = "Beached_Geyser_RapidCycle",
			HYDROGENIZED = "Beached_Geyser_Hydrogenized",
			LIMPETED = "Beached_Geyser_Limpeted",
			SLIMELUNG = "Beached_Geyser_SlimeLung",
			LEAKY = "Beached_Geyser_Leaky";
		// max pressure change, wide pipe/narrow pipe
		// unpredictable
		// clogged (sometimes stops randomly)
		// gunky (random slime)


		private List<TraitOption> options;

		public static void Register()
		{
			var large = CreateBasicTrait(LARGE, true);
			large.OnAddTrait += go => ScaleAnim(go, 1.1f);

			CreateBasicTrait(SMALL, false).OnAddTrait += go => ScaleAnim(go, 0.95f);
		}

		private class TraitOption(string traitId, float weight = 1f, Func<Geyser, bool> isAvailable = null) : IWeighted
		{
			public string traitId = traitId;
			public float weight { get; set; } = weight;
			private readonly Func<Geyser, bool> isAvailable = isAvailable;

			public bool IsValidForGeyser(Geyser geyser) => isAvailable == null || isAvailable(geyser);
		}

		public BGeyserTraits()
		{
			options =
				[
					new TraitOption(LARGE),
					new TraitOption(SMALL),
				];
		}

		private static void ScaleAnim(GameObject geyser, float scale)
		{
			if (geyser.TryGetComponent(out KBatchedAnimController kbac))
				kbac.animScale *= scale;
		}

		private static Trait CreateBasicTrait(string ID, bool isPositive)
		{
			var trait = Db.Get().CreateTrait(
				ID,
				Strings.Get($"Beached.STRINGS.CREATURES.TRAITS.GEYSERS.{ID.ToUpperInvariant()}.NAME"),
				Strings.Get($"Beached.STRINGS.CREATURES.TRAITS.GEYSERS.{ID.ToUpperInvariant()}.DESC"),
				GEYSER_GROUP,
				true,
				null,
				isPositive,
				false);

			return trait;
		}

		public string GetRandomTrait(Geyser geyser)
		{
			return options
				.Where(option => option.IsValidForGeyser(geyser))
				.GetWeightedRandom()?.traitId;
		}
	}
}
