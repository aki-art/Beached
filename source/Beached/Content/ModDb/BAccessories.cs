using Database;

namespace Beached.Content.ModDb
{
	public class BAccessories
	{
		public const string POFFMOUTH = "beached_poffmouth";

		// must start with hat_ 
		public const string ARCHEOLOGY_HAT = "hat_role_beached_archeology";

		public static void Register(Accessories accessories, AccessorySlots slots)
		{
			Log.Debug("Loading accessories for custom hats");
			var archeologyHat = Assets.GetAnim("hat_role_beached_archeology_kanim");
			AddAccessories(archeologyHat, slots.Hat, accessories);

			Log.Debug("Loading accessories for diseases");
			accessories.AddCustomAccessories(Assets.GetAnim("beached_poffmouth_mouth"), accessories, slots);
			var limpetFace = Assets.GetAnim("limpet_face_kanim");

#if !NO_MINNOW
			Log.Debug("Loading accessories for Minnow");
			var minnowHead = Assets.GetAnim("minnow_head_kanim");
			var hairSwapAnim = Assets.GetAnim("hair_swap_kanim");

			AddAccessories(minnowHead, slots.Hair, accessories);
			AddAccessories(minnowHead, slots.HatHair, accessories);
			AddAccessories(minnowHead, slots.Mouth, accessories);
			AddAccessories(minnowHead, slots.HeadShape, accessories);
#endif

#if !NO_VAHANO
			Log.Debug("Loading accessories for Vahano");
			var vahanoHead = Assets.GetAnim("beached_vahano_head_kanim");
			Log.Debug((vahanoHead != null).ToString());

			AddAccessories(vahanoHead, slots.Hair, accessories);
			AddAccessories(vahanoHead, slots.HatHair, accessories);
			AddAccessories(vahanoHead, slots.Mouth, accessories);
			AddAccessories(vahanoHead, slots.HeadShape, accessories);
			AddAccessories(vahanoHead, slots.Eyes, accessories);
#endif

			AddAccessories(limpetFace, slots.HeadEffects, accessories);
		}

		public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
		{
			Log.Debug(slot.Id);

			var build = file.GetData().build;
			var id = slot.Id.ToLower();

			for (var i = 0; i < build.symbols.Length; i++)
			{
				var symbolName = HashCache.Get().Get(build.symbols[i].hash);
				Log.Debug(symbolName);

				if (symbolName.StartsWith(id))
				{
					var accessory = new Accessory(symbolName, parent, slot, file.batchTag, build.symbols[i]);
					slot.accessories.Add(accessory);
					HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);

					Log.Debug("Added accessory: " + accessory.Id);
				}
				else
				{
					Log.Debug($"Symbol {symbolName} in file {file.name} is not starting with {id}");
				}
			}
		}
	}
}
