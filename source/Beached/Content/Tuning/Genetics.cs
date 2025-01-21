using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Tuning
{
	public class Genetics
	{
		public const string RULE_ALL = "All";
		public const string RULE_MEATDROPPERS = "MeatDroppers";
		public const string RULE_DECORPROVIDERS = "DecorProviders";
		public const string RULE_FURRY = "Furry";
		public const string RULE_DROPSANYTHING = "DropsAnything";

		public static Dictionary<string, Func<GameObject, bool>> rules = new()
		{
			{
				RULE_ALL,
				go => true
			},
			{
				RULE_MEATDROPPERS,
				go => go.TryGetComponent(out Butcherable butcherable)  && butcherable.drops.Contains(MeatConfig.ID)
			},
			{
				RULE_DROPSANYTHING,
				go => go.TryGetComponent(out Butcherable butcherable)  && butcherable.drops.Length > 0
			},
			{
				RULE_DECORPROVIDERS,
				go => go.TryGetComponent(out DecorProvider decorProvider) && decorProvider.baseDecor > 0
			},
			{
				RULE_FURRY,
				go =>
				{
					if(go.TryGetComponent(out CreatureBrain brain))
					{
						return brain.species == GameTags.Creatures.Species.SquirrelSpecies; // TODO: Maki
					}

					return false;
				}
			}
		};
	}
}
