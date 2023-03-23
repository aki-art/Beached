using Database;
using Klei.AI;

namespace Beached.Content.ModDb.Germs
{
    public class BDiseases
    {
        public static Disease plankton;
        public static Disease limpetEggs;
        public static Disease mushroomSpore;
        public static Disease poffSpore;

        public static void Register(Diseases diseases, bool statsOnly)
        {
            plankton = RegisterGerm(diseases, PlanktonGerms.ID, new PlanktonGerms(statsOnly));
            limpetEggs = RegisterGerm(diseases, LimpetEggGerms.ID, new LimpetEggGerms(statsOnly));
            mushroomSpore = RegisterGerm(diseases, CapSporeGerms.ID, new CapSporeGerms(statsOnly));
            poffSpore = RegisterGerm(diseases, PoffSporeGerms.ID, new PoffSporeGerms(statsOnly));
        }

        private static Disease RegisterGerm<T>(Diseases diseases, string ID, T germInstance) where T : Disease
        {
            Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info(ID)
            {
                overlayColourName = ID
            });

            return diseases.Add(germInstance);
        }
    }
}
