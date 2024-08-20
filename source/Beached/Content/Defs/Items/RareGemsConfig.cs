using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class RareGemsConfig : IMultiEntityConfig
	{
		public static List<GameObject> items = [];

		public const string AMBER_INCLUSION_BUG = "Beached_Item_AmberInclusionBug";
		public const string AMBER_INCLUSION_HATCH = "Beached_Item_AmberInclusionHatch";
		public const string AMBER_INCLUSION_MICRORAPTOR = "Beached_Item_AmberInclusionMicroraptor";
		public const string AMBER_INCLUSION_SCORPION = "Beached_Item_AmberInclusionScorpion";
		public const string FLAWLESS_DIAMOND = "Beached_Item_Flawless_Diamond";
		public const string HADEAN_ZIRCON = "Beached_Item_HadeanZircon";
		public const string MAXIXE = "Beached_Item_Maxixe";
		public const string MOTHER_PEARL = "Beached_Item_MotherPearl";
		public const string STRANGE_MATTER = "Beached_Item_StrangeMatter";

		public List<GameObject> CreatePrefabs() =>
			[
				BEntityTemplates.CreateSimpleItem(FLAWLESS_DIAMOND, STRINGS.ITEMS.GEMS.FLAWLESS_DIAMOND.NAME, STRINGS.ITEMS.GEMS.FLAWLESS_DIAMOND.DESCRIPTION, "beached_flawless_diamond_kanim", DECOR.BONUS.TIER2, SimHashes.Diamond),
				BEntityTemplates.CreateSimpleItem(HADEAN_ZIRCON, STRINGS.ITEMS.GEMS.HADEAN_ZIRCON.NAME, STRINGS.ITEMS.GEMS.HADEAN_ZIRCON.DESCRIPTION, "beached_hadean_zircon_kanim", DECOR.BONUS.TIER2, Elements.zirconiumOre),
				BEntityTemplates.CreateSimpleItem(MAXIXE, STRINGS.ITEMS.GEMS.MAXIXE.NAME, STRINGS.ITEMS.GEMS.MAXIXE.DESCRIPTION, "beached_maxixe_kanim", DECOR.BONUS.TIER2, Elements.aquamarine),
				BEntityTemplates.CreateSimpleItem(STRANGE_MATTER, STRINGS.ITEMS.GEMS.STRANGE_MATTER.NAME, STRINGS.ITEMS.GEMS.STRANGE_MATTER.DESCRIPTION, "beached_strange_matter_kanim", DECOR.BONUS.TIER2, SimHashes.Unobtanium)
			];

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
