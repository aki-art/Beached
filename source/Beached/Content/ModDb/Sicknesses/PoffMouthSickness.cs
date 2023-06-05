using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class PoffMouthSickness : Sickness
	{
		public const string ID = "Beached_Sickness_PoffMouth";
		public const float IMMUNE_ATTACK_STRENGTH = 0.00025f;
		public static float duration = 3 * CONSTS.CYCLE_LENGTH;

		public static List<InfectionVector> vectors = new()
		{
			InfectionVector.Inhalation,
			InfectionVector.Digestion
		};

		public PoffMouthSickness() : base(
			ID,
			SicknessType.Pathogen,
			Severity.Minor,
			IMMUNE_ATTACK_STRENGTH,
			vectors,
			duration,
			BEffects.POFFMOUTH_RECOVERY)
		{
			AddSicknessComponent(new PoffMouth());
		}
	}
}
