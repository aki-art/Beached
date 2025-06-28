using UnityEngine;

namespace BeachedUnityBridge
{
	[CreateAssetMenu(fileName = "ModData", menuName = "Modding/ModData")]
	public class TestData : ScriptableObject
	{
		[SerializeField] public string testString;
	}
}
