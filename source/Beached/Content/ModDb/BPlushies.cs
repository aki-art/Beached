using HarmonyLib;
using System;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BPlushies : ResourceSet<Plushie>
	{
		public const string
			PACU_ID = "Beached_Plushie_Pacu",
			PUFT_ID = "Beached_Plushie_Puft",
			VOLE_ID = "Beached_Plushie_Vole";

		public string[] ids; // used for debugging only
		public static readonly Vector2 standardOffset = new(1f, 0.9f);

		public BPlushies()
		{
			Add(PACU_ID, STRINGS.MISC.PLUSHIES.PACU, "beached_plushie_pacu_kanim", BEffects.PLUSHIE_PACU, standardOffset);
			Add(PUFT_ID, STRINGS.MISC.PLUSHIES.PUFT, "beached_plushie_puft_kanim", BEffects.PLUSHIE_PUFT, standardOffset);
			Add(VOLE_ID, STRINGS.MISC.PLUSHIES.PACU, "beached_plushie_vole_kanim", BEffects.PLUSHIE_VOLE, standardOffset);
		}

		public void Add(string ID, string name, string animFile, string effectId, Vector3 offset, Func<GameObject> onSleepingWithPlush = null)
		{
			Add(new Plushie(ID, name, animFile, effectId, offset, onSleepingWithPlush));
			ids = ids.AddToArray(ID);
		}
	}
}
