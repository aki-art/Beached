using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class IceWrathSickness : Sickness
	{
		public const string ID = "Beached_Sickness_IceWrath";

		public const float IMMUNE_ATTACK_STRENGTH = TUNING.DISEASE.IMMUNE_ATTACK_STRENGTH_PERCENT.FAST_2;
		public static float duration = 7f * CONSTS.CYCLE_LENGTH;

		public static List<InfectionVector> limpetsInfectionVectors =
		[
			InfectionVector.Contact,
			InfectionVector.Inhalation,
			InfectionVector.Exposure,
		];

		public IceWrathSickness() : base(
			ID,
			SicknessType.Pathogen,
			Severity.Major,
			IMMUNE_ATTACK_STRENGTH,
			limpetsInfectionVectors,
			duration,
			BEffects.ICEWRATH_DUPLICANT_RECOVERY)
		{
			AddSicknessComponent(new IceWrathAggressionEffect());
		}
	}
}
