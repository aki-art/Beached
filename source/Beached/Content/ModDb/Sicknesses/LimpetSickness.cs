using FUtility;
using Klei.AI;
using System.Collections.Generic;

namespace Beached.Content.ModDb.Sicknesses
{
	public class LimpetsSickness : Sickness
	{
		public const string ID = "Beached_Sickness_Limpets_Duplicant";
		public const float IMMUNE_ATTACK_STRENGTH = TUNING.DISEASE.IMMUNE_ATTACK_STRENGTH_PERCENT.SLOW_3;
		public static float duration = 7 * Consts.CYCLE_LENGTH;

		public static List<InfectionVector> limpetsInfectionVectors = new()
		{
			InfectionVector.Contact,
			InfectionVector.Inhalation,
			InfectionVector.Digestion
		};

		public LimpetsSickness() : base(
			ID,
			SicknessType.Pathogen,
			Severity.Minor,
			IMMUNE_ATTACK_STRENGTH,
			limpetsInfectionVectors,
			duration,
			BEffects.LIMPETS_DUPLICANT_RECOVERY)
		{
			AddSicknessComponent(new AttributeModifierSickness(new[]
			{
				new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -500_000f / Consts.CYCLE_LENGTH, STRINGS.DUPLICANTS.DISEASES.BEACHED_LIMPETS_DUPLICANT.NAME),
				new AttributeModifier(Db.Get().Attributes.Decor.Id, -20)
			}));

			if(BExpressions.limpetFace == null)
				Log.Warning("Limpet face expression is null");

			AddSicknessComponent(new SicknessExpressionEffect(BExpressions.limpetFace));
		}
	}
}
