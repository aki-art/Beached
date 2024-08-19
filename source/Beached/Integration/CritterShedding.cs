using System;
using UnityEngine;

namespace Beached.Integration
{
	public class CritterShedding
	{
		public delegate void CrittersShedFurOnBrushDelegate(Tag prefabTag, float amount, Color color);
		private static CrittersShedFurOnBrushDelegate CrittersShedFurOnBrush;

		public static void Shed(Tag prefabTag, float amount, Color color) => CrittersShedFurOnBrush?.Invoke(prefabTag, amount, color);

		public static void Initialize()
		{
			var type = Type.GetType("CrittersShedFurOnBrush.ModAssets, CrittersShedFurOnBrush");
			if (type != null)
			{
				var methodInfo = type.GetMethod(
					"AddFluffyCritter",
					[
						typeof(Tag),
						typeof(float),
						typeof(Color),
					]);

				CrittersShedFurOnBrush = (CrittersShedFurOnBrushDelegate)Delegate.CreateDelegate(typeof(CrittersShedFurOnBrushDelegate), methodInfo);
			}
		}
	}
}
