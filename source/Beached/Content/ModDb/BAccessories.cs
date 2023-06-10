using Database;

namespace Beached.Content.ModDb
{
	internal class BAccessories
	{
		public const string POFFMOUTH = "beached_poffmouth";

		public static void Register(Accessories accessories, AccessorySlots slots)
		{
			accessories.AddCustomAccessories(Assets.GetAnim("beached_poffmouth_mouth"), accessories, slots);
			var minnowHead = Assets.GetAnim("minnow_head_kanim");
			var limpetFace = Assets.GetAnim("limpet_face_kanim");
			var hairSwapAnim = Assets.GetAnim("hair_swap_kanim");

			AddAccessories(minnowHead, slots.Hair, accessories);
			AddAccessories(minnowHead, slots.HatHair, accessories);
			AddAccessories(minnowHead, slots.Mouth, accessories);
			AddAccessories(minnowHead, slots.HeadShape, accessories);
			AddAccessories(limpetFace, slots.HeadEffects, accessories);
		}

		public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
		{
			var build = file.GetData().build;
			var id = slot.Id.ToLower();

			for (var i = 0; i < build.symbols.Length; i++)
			{
				var symbolName = HashCache.Get().Get(build.symbols[i].hash);
				if (symbolName.StartsWith(id))
				{
					var accessory = new Accessory(symbolName, parent, slot, file.batchTag, build.symbols[i]);
					slot.accessories.Add(accessory);
					HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);

					Log.Debug("Added accessory: " + accessory.Id);
				}
			}
		}
	}
}
