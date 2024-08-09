using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class MiniFridgeShelfDisplay : KMonoBehaviour
	{
		[MyCmpReq]
		private SymbolOverrideController symbolOverrideController;

		[MyCmpReq]
		private KBatchedAnimController kbac;

		[MyCmpReq]
		private Storage storage;

		private KBatchedAnimController[] shelfItems;
		public static Vector3[] offsets = new[]
		{
			new Vector3(-0.1f, 0.1f, 0.1f), // bl
            new Vector3(-0.1f, 0.4f, 0.2f), // tl
            new Vector3(0.2f, 0.1f, 0.05f), // br
            new Vector3(0.2f, 0.4f, 0.15f)  // tr
        };

		public override void OnSpawn()
		{
			base.OnSpawn();

			shelfItems = new KBatchedAnimController[4];
			Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
			Beached_Mod.Instance.Subscribe(ModHashes.debugDataChange, OnDebugDataChange);
			OnStorageChange(null);
		}

		private void OnDebugDataChange(object obj)
		{
			for (var i = 0; i < 4; i++)
			{
				if (shelfItems[i] != null)
				{
					shelfItems[i].transform.position = transform.position + offsets[i];
				}
			}

			OnStorageChange(null);
		}

		private KBatchedAnimController CreateShelfItem(int index, KAnimFile animFile, bool show)
		{
			if (animFile == null)
			{
				Log.Warning("Mini-fridge: shelf item anim file is null");
				return null;
			}

			var item = shelfItems[index];

			if (item != null)
			{
				item.enabled = show;
				if (show)
				{
					item.SwapAnims(new[]
					{
						animFile
					});
				}

				return item;
			}

			var pos = transform.position;

			var go = new GameObject("Beached Fridge shelf display");
			go.SetActive(false);

			var result = go.AddComponent<KBatchedAnimController>();
			result.AnimFiles = new[]
			{
				animFile
			};

			result.initialAnim = "ui";
			result.sceneLayer = Grid.SceneLayer.Building;
			result.animScale *= 0.4f;

			result.enabled = show;

			go.transform.parent = gameObject.transform;
			go.transform.position = pos + offsets[index];
			go.SetActive(true);

			shelfItems[index] = result;

			return result;
		}


		private void OnStorageChange(object obj)
		{
			var storedItems = storage.GetItems();
			storedItems.OrderBy(i => i.GetComponent<PrimaryElement>().Mass);

			/*            // if we have one type of food and a bunch of it, fill the fridge with it
                        if (storedItems.Count == 1)
                        {
                            ShowSingleItem(storedItems);
                            return;
                        }*/

			for (var i = 0; i < 4; i++)
			{
				if (storedItems.Count > i)
				{
					var item = storedItems[i];
					ShowItem(i, item);
				}
				else
				{
					HideItem(i);
				}
			}
		}

		private void ShowSingleItem(List<GameObject> storedItems)
		{
			var fullness = storage.MassStored() / storage.capacityKg;
			fullness = Mathf.Clamp01(fullness);
			var count = Mathf.CeilToInt(fullness * 4);
			var item = storedItems[0];

			for (var i = 0; i < 4; i++)
			{
				if (i >= count - 1)
				{
					ShowItem(i, item);
				}
				else
				{
					HideItem(i);
				}
			}
		}

		private void HideItem(int index)
		{
			if (shelfItems[index] != null)
			{
				shelfItems[index].enabled = false;
			}
		}

		private void ShowItem(int index, GameObject item)
		{
			if (item.TryGetComponent(out KBatchedAnimController itemKbac))
			{
				if (itemKbac.AnimFiles == null || itemKbac.AnimFiles.Length == 0)
				{
					Log.Debug("anim files null or empty " + item.PrefabID());
					HideItem(index);
					return;
				}

				CreateShelfItem(index, itemKbac.AnimFiles[0], true);
			}
		}

	}
}
