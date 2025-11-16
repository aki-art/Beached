using System;
using UnityEngine;
using static Beached.Integration.CritterShedding;

namespace Beached.Integration
{
	public class SetStartDupes
	{
		public delegate void AddTraitRemovalDelegate(string traitId, Action<GameObject> fn);
		public static AddTraitRemovalDelegate AddTraitRemovalAction;

		public static void Initialize()
		{
			var type = Type.GetType("SetStartDupes.ModApi, SetStartDupes");
			if (type != null)
			{
				var methodInfo = type.GetMethod(
					"AddTraitRemovalAction",
					[
						typeof(string),
						typeof(Action<GameObject>)
					]);

				AddTraitRemovalAction = (AddTraitRemovalDelegate)Delegate.CreateDelegate(typeof(CrittersShedFurOnBrushDelegate), methodInfo);
			}
		}
	}
}
