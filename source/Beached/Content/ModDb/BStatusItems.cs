using Beached.Content.Scripts;
using Beached.Content.Scripts.Buildings;
using Beached.Content.Scripts.Entities.AI;
using Beached.Content.Scripts.Items;

namespace Beached.Content.ModDb
{
	public class BStatusItems
	{
		public static StatusItem desiccation;
		public static StatusItem secretingMucus;
		public static StatusItem lubricated;
		public static StatusItem smoking;
		public static StatusItem geneticallyMofidied;
		public static StatusItem gunked;
		public static StatusItem plushed;

		public static void Register(Db db)
		{
			gunked = new(
				"Beached_Gunked",
				"BUILDINGS",
				string.Empty,
				StatusItem.IconType.Exclamation,
				NotificationType.Bad,
				false,
				OverlayModes.None.ID
				);

			plushed = new(
				"Beached_Plushed",
				"BUILDINGS",
				string.Empty,
				StatusItem.IconType.Info,
				NotificationType.Good,
				false,
				OverlayModes.None.ID);

			plushed.SetResolveStringCallback(Beached_PlushiePlaceable.GetStatusItemTooltip);

			desiccation = new(
				"Beached_Desiccation",
				"CREATURES",
				string.Empty,
				StatusItem.IconType.Exclamation,
				NotificationType.Bad,
				false,
				OverlayModes.None.ID);

			desiccation.SetResolveStringCallback((str, data) => data is MoistureMonitor.Instance moistureMonitor ? string.Format(str, moistureMonitor.timeUntilDeath) : str);

			geneticallyMofidied = new(
				"Beached_GeneticallyModified",
				"CREATURES",
				"status_item_unknown_mutation",
				StatusItem.IconType.Custom,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID);

			geneticallyMofidied.SetResolveStringCallback((str, data) =>
			{
				return data is Beached_GeneticallyModifiableEgg egg ? egg.GetStatusItemString(str) : str;
			});

			secretingMucus = new(
				"Beached_SecretingMucus",
				"CREATURES",
				string.Empty,
				StatusItem.IconType.Exclamation,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID,
				false);

			lubricated = new(
				"Beached_Lubricated",
				"BUILDINGS",
				string.Empty,
				StatusItem.IconType.Info,
				NotificationType.Good,
				false,
				OverlayModes.None.ID,
				false);

			lubricated.SetResolveStringCallback(GetLubricantString);

			smoking = new(
				"Beached_Smoking",
				"CREATURES",
				string.Empty,
				StatusItem.IconType.Info,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID);

			smoking.SetResolveStringCallback((str, data) => data is SmokeCookable smokable ? smokable.GetStatusItemTooltip(str) : str);

			db.CreatureStatusItems.Add(desiccation);
			db.CreatureStatusItems.Add(lubricated);
			db.CreatureStatusItems.Add(smoking);
		}

		private static string GetLubricantString(string str, object data)
		{
			return data is Lubricatable lubricatable
				? string.Format(str, lubricatable.GetUsesRemaining())
				: str;
		}
	}
}
