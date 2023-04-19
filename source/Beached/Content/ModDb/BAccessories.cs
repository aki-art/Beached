using Database;

namespace Beached.Content.ModDb
{
    internal class BAccessories
    {
        public const string POFFMOUTH = "beached_poffmouth";

        public static void Register(Accessories accessories, AccessorySlots slots)
        {
            accessories.AddCustomAccessories(Assets.GetAnim("beached_poffmouth_mouth"), accessories, slots);
        }
    }
}
