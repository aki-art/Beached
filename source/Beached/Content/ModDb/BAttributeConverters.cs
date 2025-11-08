using Klei.AI;
using UnityEngine;

namespace Beached.Content.ModDb
{
	public class BAttributeConverters
	{
		public static AttributeConverter precisionSpeed;
		public static AttributeConverter digAmount;

		[DbEntry]
		public static void Register(Database.AttributeConverters __instance)
		{
			precisionSpeed = __instance.Create(
				"Beached_PrecisionSpeed",
				"Precision Work Speed",
				STRINGS.DUPLICANTS.ATTRIBUTES.BEACHED_PRECISION.SPEEDMODIFIER,
				Db.Get().Attributes.Get(BAttributes.PRECISION_ID),
				0.1f,
				0.0f,
				new ToPercentAttributeFormatter(1f));

			digAmount = __instance.Create(
				"Beached_DigAmount",
				"Diggable Amount",
				STRINGS.DUPLICANTS.ATTRIBUTES.BEACHED_PRECISION.AMOUNTMODIFIER,
				Db.Get().Attributes.Get(BAttributes.PRECISION_ID),
				0.025f,
				0.5f,
				new DigPercentAttributeFormatter());
		}

		public class DigPercentAttributeFormatter()
			: StandardAttributeFormatter(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
			public override string GetFormattedAttribute(AttributeInstance instance)
			{
				return GetFormattedValue(instance.GetTotalDisplayValue(), DeltaTimeSlice);
			}

			public override string GetFormattedModifier(AttributeModifier modifier)
			{
				return GetFormattedValue(modifier.Value, DeltaTimeSlice);
			}

			public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
			{
				var clampedValue = Mathf.Clamp01(value);
				return GameUtil.GetStandardPercentageFloat(clampedValue * 100f, false) + (global::STRINGS.UI.UNITSUFFIXES.PERCENT);
			}
		}
	}
}
