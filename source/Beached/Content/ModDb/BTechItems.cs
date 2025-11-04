using Beached.Content.Defs.Equipment;
using Database;

namespace Beached.Content.ModDb
{
	public class BTechItems
	{
		public static void OnInit(TechItems techItems)
		{
			techItems.AddTechItem(
				RubberBootsConfig.ID,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_RUBBERBOOTS.NAME,
				STRINGS.EQUIPMENT.PREFABS.BEACHED_EQUIPMENT_RUBBERBOOTS.DESCRIPTION,
				techItems.GetPrefabSpriteFnBuilder(RubberBootsConfig.ID),
				null,
				null,
				false);

			techItems.AddTechItem(
				Elements.rubber.CreateTag().ToString(),
				STRINGS.RESEARCH.TECHITEMS.BEACHED_RUBBER.NAME,
				STRINGS.RESEARCH.TECHITEMS.BEACHED_RUBBER.DESC,
				techItems.GetPrefabSpriteFnBuilder(Elements.rubber.CreateTag()),
				null,
				null,
				false);
		}
	}
}
