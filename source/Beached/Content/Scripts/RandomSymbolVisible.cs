using ImGuiNET;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RandomSymbolVisible : KMonoBehaviour, IImguiDebug
	{
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField] public int minVisibleSymbols;
		[SerializeField] public int maxVisibleSymbols;
		[Serialize][SerializeField] public List<string> targetSymbols;

		[Serialize] private List<string> visibleSymbols;

		override public void OnSpawn()
		{
			if (visibleSymbols == null)
			{
				Log.Debug("setting symbols");
				var count = Random.Range(minVisibleSymbols, maxVisibleSymbols + 1);

				visibleSymbols = [];
				targetSymbols.Shuffle();

				for (int i = 0; i < count; i++)
				{
					visibleSymbols.Add(targetSymbols[i]);
				}
			}

			StartCoroutine(UpdateNextFrame());
		}

		private IEnumerator UpdateNextFrame()
		{
			yield return SequenceUtil.waitForEndOfFrame;
			UpdateSymbols();
		}

		private void UpdateSymbols()
		{
			Log.Debug("Updating symbols");

			if (visibleSymbols == null || targetSymbols == null)
				return;

			Log.Debug("has data");

			foreach (var symbol in targetSymbols)
			{
				Log.Debug($"{symbol} {visibleSymbols.Contains(symbol)}");
				kbac.SetSymbolVisiblity(symbol, visibleSymbols.Contains(symbol));
			}

			kbac.SetDirty();
			kbac.UpdateFrame(0);
			kbac.Play("idle");
		}

		public void OnImguiDraw()
		{
			if (ImGui.Button("Refresh animation"))
				UpdateSymbols();
		}
	}
}
