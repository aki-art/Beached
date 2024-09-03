using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class RandomSymbolVisible : FMonoBehaviour
	{
		[MyCmpReq] private KBatchedAnimController kbac;

		[SerializeField] public int minVisibleSymbols;
		[SerializeField] public int maxVisibleSymbols;
		[SerializeField] public List<KAnimHashedString> targetSymbols;

		[Serialize] private List<KAnimHashedString> visibleSymbols;

		override public void OnSpawn()
		{
			if (visibleSymbols == null)
			{
				DebugLog("setting symbols");
				var count = Random.Range(minVisibleSymbols, maxVisibleSymbols + 1);

				visibleSymbols = [];
				targetSymbols.Shuffle();

				for (int i = 0; i < count; i++)
				{
					visibleSymbols.Add(targetSymbols[i]);
				}
			}

			UpdateSymbols();
		}

		private void UpdateSymbols()
		{
			if (visibleSymbols == null || targetSymbols == null)
			{
				DebugLog("No symbols defined.");
				DebugLog(visibleSymbols == null);
				DebugLog(targetSymbols == null);
				return;
			}

			foreach (var symbol in targetSymbols)
			{
				kbac.SetSymbolVisiblity(symbol, visibleSymbols.Contains(symbol));
			}
		}
	}
}
