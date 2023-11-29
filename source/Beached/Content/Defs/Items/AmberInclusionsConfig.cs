using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class AmberInclusionsConfig : IMultiEntityConfig
	{
		public const string
			FLYING_CENTIPEDE = "Beached_AmberInclusion_FlyingCentipede",
			STRANGE_HATCH = "Beached_AmberInclusion_StangeHatch",
			FEATHER = "Beached_AmberInclusion_Feather";

		public static EffectorValues SMALL = new(10, 2);
		public static EffectorValues MEDIUM = new(15, 2);
		public static EffectorValues BIG = new(20, 2);

		public List<GameObject> CreatePrefabs() => new()
		{
			CreateAmber(FLYING_CENTIPEDE, "beached_amberinclusion_flyingcentipede_kanim", MEDIUM),
			CreateAmber(STRANGE_HATCH, "beached_amberinclusion_strangehatch_kanim", BIG),
			CreateAmber(FEATHER, "beached_amberinclusion_feather_kanim", SMALL)
		};

		private static GameObject CreateAmber(string ID, string anim, EffectorValues decor)
		{
			string partialID = ID.Replace("Beached_AmberInclusion_", "").ToUpperInvariant();

			var name = Strings.Get($"STRINGS.ITEMS.AMBER_INCLUSIONS.{partialID}.NAME");
			var desc = Strings.Get($"STRINGS.ITEMS.AMBER_INCLUSIONS.{partialID}.DESCRIPTION");

			var item = BEntityTemplates.CreateSimpleItem(ID, name, desc, anim, decor, Elements.amber);

			item.AddTags(BTags.amberInclusion, BTags.amberInclusion);

			return item;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
