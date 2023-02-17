using Beached.Content.Scripts;
using Beached.Content.Scripts.Entities.AI;

namespace Beached.Content.ModDb
{
    public class BStatusItems
    {
        public static StatusItem desiccation;
        public static StatusItem secretingMucus;
        public static StatusItem lubricated;

        public static void Register()
        {
            desiccation = new(
                "Beached_Desiccation",
                "CREATURES",
                string.Empty,
                StatusItem.IconType.Exclamation,
                NotificationType.Bad,
                false,
                OverlayModes.None.ID,
                true);

            desiccation.SetResolveStringCallback((str, data) => data is MoistureMonitor.Instance moistureMonitor ? string.Format(str, moistureMonitor.timeUntilDeath) : str);

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

        }

        private static string GetLubricantString(string str, object data)
        {
            return data is Lubricatable lubricatable
                ? string.Format(str, lubricatable.GetUsesRemaining())
                : str;
        }
    }
}
