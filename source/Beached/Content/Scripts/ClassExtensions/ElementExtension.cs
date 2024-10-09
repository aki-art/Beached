namespace Beached.Content.Scripts.ClassExtensions
{
	public static class ElementExtension
	{
		/// Modders, to set by name: <see cref="ModAPI.SetElementAcidVulnerability(string, float)"/>
		/// to set by SimHash: <see cref="ModAPI.SetElementAcidVulnerability(SimHashes, float)"/>
		public static float AcidVulnerability(this Element element)
		{
			return Elements.acidVulnerabilities.GetOrDefault(element.id, CONSTS.CORROSION_VULNERABILITY.MEDIUM);
		}

		public static float LubricantStrength(this Element element)
		{
			return Elements.lubricantStrengths.GetOrDefault(element.id, 0);
		}
	}
}
