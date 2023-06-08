namespace Beached.Content.Scripts.ClassExtensions
{
	public static class ElementExtension
	{
		/// Modders, to set by name: <see cref="ModAPI.SetElementAcidVulnerability(string, float)"/>
		/// to set by SimHash: <see cref="ModAPI.SetElementAcidVulnerability(SimHashes, float)"/>
		public static float AcidVulnerability(this Element element)
		{
			// if not defined, consider unaffected with 100% resistance
			return Elements.acidVulnerabilities.TryGetValue(element.id, out var result) ? result : CONSTS.CORROSION_VULNERABILITY.MEDIUM;
		}
	}
}
