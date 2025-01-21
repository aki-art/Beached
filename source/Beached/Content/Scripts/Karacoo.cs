using Beached.Content.ModDb;
using ImGuiNET;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Karacoo : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[Serialize] public string skinId;
		[SerializeField] public bool randomizeColors;

		public override void OnSpawn() => RefreshSkin();

		private void RefreshSkin()
		{
			if (!randomizeColors)
				return;

			if (skinId == null || BDb.karacooSkins.TryGet(skinId) == null)
				skinId = BDb.karacooSkins.GetRandom().Id;

			BDb.karacooSkins
				.TryGet(skinId)
				.Apply(kbac);
		}

		public void OnImguiDraw()
		{
			if (ImGui.Button("Recolor"))
			{
				kbac.SetSymbolTint("beak", Color.white);
				skinId = null;
				RefreshSkin();
			}
		}
	}
}
