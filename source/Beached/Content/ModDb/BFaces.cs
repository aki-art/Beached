using Database;

namespace Beached.Content.ModDb
{
	public class BFaces
	{
		public static Face limpetFace;
		public const string LIMPETFACE_ID = "Beached_LimpetFace";

		[DbEntry]
		public static void Register(Faces __instance)
		{
			limpetFace = __instance.Add(new Face(LIMPETFACE_ID, "headfx_limpet")
			{
				hash = new HashedString("Sick")
			});
		}
	}
}
