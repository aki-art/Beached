using UnityEngine;

public class ModInfo : MonoBehaviour
{
	[SerializeField]
	public string modName;

	[SerializeField]
	public string overrideStringKeyPrefix;

	[SerializeField]
	public string prefabPath;

	public bool attachTooltips;

	public bool convertTMP;

	public string tooltipPrefix = "{modName}.STRINGS.UI.TOOLTIPS";
}
