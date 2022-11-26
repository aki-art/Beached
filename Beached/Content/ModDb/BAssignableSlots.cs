using Database;

namespace Beached.Content.ModDb
{
    public class BAssignableSlots
    {
        public static AssignableSlot Jewellery;
        public const string JEWELLERY_ID = "Beached_AssignableSlot_Jewellery";

        public static void Register(AssignableSlots parent)
        {
            Jewellery = parent.Add(new EquipmentSlot(JEWELLERY_ID, "Jewellery", true));
        }
    }
}
