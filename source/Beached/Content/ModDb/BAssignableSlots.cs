using Database;

namespace Beached.Content.ModDb
{
	public class BAssignableSlots
	{
		public static AssignableSlot Jewellery;
		public const string JEWELLERY_ID = "Beached_AssignableSlot_Jewellery";
		public static AssignableSlot Shoes;
		public const string SHOES_ID = "Beached_AssignableSlot_Shoes";

		public static void Register(AssignableSlots parent)
		{
			Jewellery = parent.Add(new EquipmentSlot(JEWELLERY_ID, "Jewellery"));
			Shoes = parent.Add(new EquipmentSlot(SHOES_ID, "Shoes"));
		}
	}
}
