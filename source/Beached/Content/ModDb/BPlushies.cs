using HarmonyLib;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BPlushies : ResourceSet<Plushie>
	{
		public const string
			MUFFIN_ID = "Beached_Plushie_Muffin",
			PACU_ID = "Beached_Plushie_Pacu",
			PUFT_ID = "Beached_Plushie_Puft",
			VOLE_ID = "Beached_Plushie_Vole";

		public string[] ids; // used for debugging only
		public static readonly Vector2 standardOffset = new(1f, 0.9f);

		public BPlushies()
		{
			Add(MUFFIN_ID, STRINGS.MISC.PLUSHIES.MUFFIN, "beached_plushie_muffin_kanim", BEffects.PLUSHIE_MUFFIN, standardOffset);
			Add(PACU_ID, STRINGS.MISC.PLUSHIES.PACU, "beached_plushie_pacu_kanim", BEffects.PLUSHIE_PACU);
			Add(PUFT_ID, STRINGS.MISC.PLUSHIES.PUFT, "beached_plushie_puft_kanim", BEffects.PLUSHIE_PUFT);
			Add(VOLE_ID, STRINGS.MISC.PLUSHIES.PACU, "beached_plushie_vole_kanim", BEffects.PLUSHIE_VOLE);
		}

		public Plushie Add(string ID, string name, string animFile, string effectId) => Add(ID, name, animFile, effectId, standardOffset);

		public Plushie Add(string ID, string name, string animFile, string effectId, Vector3 offset)
		{
			var result = Add(new Plushie(ID, name, animFile, effectId, offset));
			ids = ids.AddToArray(ID);

			return result;
		}
	}
}
