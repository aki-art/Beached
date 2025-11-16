using KSerialization;

namespace Beached.Content.Scripts.Buildings
{
	// Adds a lore bearer only to copies of a building present at world generation
	public class WorldgenOnlyLoreBearer : KMonoBehaviour
	{
		[Serialize] public bool hasLore;

		public override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.NewGameSpawn, OnNewGameSpawn);
		}

		private void OnNewGameSpawn(object obj)
		{
			hasLore = true;
		}
	}
}
