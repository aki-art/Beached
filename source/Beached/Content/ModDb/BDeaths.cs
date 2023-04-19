namespace Beached.Content.ModDb
{
    public class BDeaths
    {
        public static Death desiccation;

        public static void Register()
        {
            new Death(
                "Desiccation",
                Db.Get().Deaths,
                STRINGS.DEATHS.DESICCATION.NAME,
                STRINGS.DEATHS.DESICCATION.DESCRIPTION,
                "death_suffocation",
                "dead_on_back");
        }
    }
}
