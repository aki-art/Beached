using Database;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb.Germs
{
	public class BDiseases
	{
		public static Disease plankton;
		public static Disease limpetEggs;
		public static Disease mushroomSpore;
		public static Disease poffSpore;
		public static Disease iceWrath;

		public static void Register(Diseases diseases, bool statsOnly)
		{
			plankton = RegisterGerm(diseases, PlanktonGerms.ID, new PlanktonGerms(statsOnly));
			limpetEggs = RegisterGerm(diseases, LimpetEggGerms.ID, new LimpetEggGerms(statsOnly));
			mushroomSpore = RegisterGerm(diseases, CapSporeGerms.ID, new CapSporeGerms(statsOnly));
			poffSpore = RegisterGerm(diseases, PoffSporeGerms.ID, new PoffSporeGerms(statsOnly));
			iceWrath = RegisterGerm(diseases, IceWrathGerms.ID, new IceWrathGerms(statsOnly));
		}

		public static void AddGermColors(Dictionary<string, Color32> namedLookup)
		{
			namedLookup[PlanktonGerms.ID] = ModAssets.Colors.plankton;
			namedLookup[LimpetEggGerms.ID] = ModAssets.Colors.limpetEggs;
			namedLookup[CapSporeGerms.ID] = ModAssets.Colors.capSpores;
			namedLookup[PoffSporeGerms.ID] = ModAssets.Colors.poffSpores;
			namedLookup[IceWrathGerms.ID] = ModAssets.Colors.iceWrath;
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
