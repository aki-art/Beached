using Database;

namespace Beached.Content.ModDb
{
	public class BExpressions
	{
		public static Expression limpetFace;

		[DbEntry]
		public static void Register(Expressions __instance)
		{
			limpetFace = new Expression("Beached_Limpetface", __instance, BFaces.limpetFace)
			{
				priority = __instance.SickSpores.priority + 1
			};
		}
	}
}
