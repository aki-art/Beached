using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class CappedSickness : Sickness
	{
		public const string ID = "Beached_Sickness_Capped";
		public const float IMMUNE_ATTACK_STRENGTH = 0.00025f;
		public static float duration = 7 * CONSTS.CYCLE_LENGTH;

		public static List<InfectionVector> infectionVectors = new List<InfectionVector>
		{
			InfectionVector.Inhalation,
			InfectionVector.Digestion
		};

		public CappedSickness() : base(
			ID,
			SicknessType.Pathogen,
			Severity.Minor,
			IMMUNE_ATTACK_STRENGTH,
			infectionVectors,
			duration,
			BEffects.CAPPED_RECOVERY)
		{
			AddSicknessComponent(new AttributeModifierSickness(
			[
				new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, -2)
			]));

			AddSicknessComponent(new Capped());
		}
	}
}
