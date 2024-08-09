using Database;

namespace Beached.Content.ModDb
{
	public class BAssignableSlots
	{
		public static AssignableSlot Jewellery;
		public const string JEWELLERY_ID = "Shared_AssignableSlot_Jewellery";
		public static AssignableSlot Shoes;
		public const string SHOES_ID = "Shared_AssignableSlot_Shoes";

		[DbEntry]
		public static void Register(AssignableSlots __instance)
		{
			// todo add or get
			Jewellery = __instance.Add(new EquipmentSlot(JEWELLERY_ID, "Jewellery"));
			Shoes = __instance.Add(new EquipmentSlot(SHOES_ID, "Shoes"));
		}
	}
}
