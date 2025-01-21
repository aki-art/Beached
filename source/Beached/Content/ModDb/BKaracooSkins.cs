namespace Beached.Content.ModDb
{
	public class BKaracooSkins : ResourceSet<KaracooSkin>
	{
		public const string ID = "Beached_KaracooSkins";

		public BKaracooSkins(ResourceSet parent) : base(ID, parent)
		{
			Add("green_orange", "7bb906", "cc8c19");
			Add("blue", "6bb9f4", "77eef2");
			Add("canary", "ffd54c", "ff6f6a");
			Add("lime_juice", "73ff4c", "f8ff6a");
			Add("funky_pink", "ff4cb1", "886aff");
			Add("cockatoo", "f7eecf", "ffbf00");
			Add("ring_neck", "25afc2", "3447c3").AddLayer(Util.ColorFromHex("ff647c"), ["beak"]);
			Add("blue_gold", "3985ef", "f0c62c");
			Add("red_blue", "ef393f", "409ef5");
			Add("red_orange", "ef4539", "fbac3d");
			Add("raven", "383d53", "2f4659");
		}

		public KaracooSkin Add(string id, string primary, string secondary) => Add(KaracooSkin.CreateBasicSkin(id, primary, secondary));

		public KaracooSkin GetRandom() => resources.GetRandom();
	}
}
