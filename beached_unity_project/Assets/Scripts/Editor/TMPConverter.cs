using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TMPConverter : MonoBehaviour
{
	[Serializable]
	public class TMPSettings
	{
		[JsonProperty]
		public bool Skip { get; set; }

		[JsonProperty]
		public string Font { get; set; } = "NotoSans-Regular";
		[JsonProperty]
		public FontStyles FontStyle { get; set; }
		[JsonProperty]
		public float FontSize { get; set; }
		[JsonProperty]
		public TextAlignmentOptions Alignment { get; set; }
		[JsonProperty]
		public int MaxVisibleLines { get; set; }
		[JsonProperty]
		public bool EnableWordWrapping { get; set; }
		[JsonProperty]
		public bool AutoSizeTextContainer { get; set; }
		[JsonProperty]
		public string Content { get; set; }
		[JsonProperty]
		public float X { get; set; }
		[JsonProperty]
		public float Y { get; set; }
		[JsonProperty]
		public float[] Color { get; set; }
	}

	private class StringEntry
	{
		public List<StringEntry> children;
		public string value;
		public string name;
		public int parentId;
		internal bool isClass;

		public StringEntry(Transform transform, string value)
		{
			this.value = value;
			children = new List<StringEntry>();
			entries.Add(transform.GetInstanceID(), this);
		}
	}

	private static Dictionary<int, StringEntry> entries;

	private static string GetUpper(string str) => str.ToUpperInvariant()
		.Replace(" ", "_")
		.Replace(".", "_");

	/*	private static string GenerateStringEntries(GameObject parent)
		{
			entries = new Dictionary<int, StringEntry>();
			var builder = new StringBuilder();

			foreach (var text in parent.GetComponentsInChildren<TextMeshProUGUI>())
			{
				var currentObject = text.transform;
				int count = 0;

				var currentEntry = new StringEntry(text.transform, text.text);
				currentEntry.name = GetUpper(text.name);

				bool reachedParent = false;
				while (currentObject != null && currentObject != parent && count++ < 16)
				{
					currentObject = currentObject.parent;

					if (currentObject == null)
						break;

					currentEntry.parentId = currentObject.GetInstanceID();

					if (reachedParent)
						break;

					if (currentObject == parent)
						reachedParent = true;

					if (entries.TryGetValue(currentEntry.parentId, out var obj))
					{
						obj.children.Add(currentEntry);
						currentEntry = obj;
					}
					else
					{
						var newEntry = new StringEntry(currentObject.transform, null);
						newEntry.children.Add(currentEntry);
						newEntry.name = GetUpper(currentObject.name);
						newEntry.isClass = true;
						currentEntry = newEntry;
					}
				}
			}

			var pool = entries.Values.ToList();
			var parentEntry = pool.Find(e => e.parentId == 0);
			foreach (var entry in pool)
				Debug.Log($"{entry.parentId} {entry.name} {entry.value}");

			ListChildren(builder, parentEntry);

			return builder.ToString();
		}

		private static void ListChildren(StringBuilder builder, StringEntry item, int level = 0, int maxDepth = 10)
		{
			if (level >= maxDepth || item == null)
			{
				Debug.Log("reached end");
				Debug.Log(item == null);
				return;
			}

			var prefix = string.Concat(Enumerable.Repeat("\t", level));

			if (item.isClass)
			{
				builder.AppendLine($"{prefix}public static class {GetUpper(item.name)}");
				builder.AppendLine($"{prefix}{{");
			}
			else
				builder.AppendLine($"public static LocString {GetUpper(item.name)} = \"{item.value}\";");

			foreach (var child in item.children)
				ListChildren(builder, child, level + 1);

			if (item.isClass)
				builder.AppendLine($"{prefix}}}");
		}
	*/

	public static string GetGameObjectPath(GameObject obj, GameObject parent)
	{
		if (obj.TryGetComponent(out LocTextKeyOverride textOverride))
		{
			return textOverride.keyOverride;
		}

		string stop = parent.name;
		string prefix = "STRINGS.UI";
		string path = "." + obj.name.ToUpper();
		bool skipFirst = false;

		if (parent.TryGetComponent(out ModInfo modInfo))
		{
			if (modInfo.overrideStringKeyPrefix != null)
			{
				prefix = modInfo.overrideStringKeyPrefix;
				skipFirst = true;
			}
			else if (modInfo.modName != null)
			{
				prefix = $"{modInfo.modName}.{prefix}";
			}
		}

		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			string parentName = obj.name;
			if (parentName == stop)
				break;
			else
			{
				var nextUp = obj.transform.parent.gameObject;
				if (skipFirst && (nextUp == null || nextUp.name == stop))
				{
					skipFirst = false;
				}
				else
					path = "." + parentName.ToUpper() + path;
			}
		}

		return $"{prefix}{path}";
	}

	static void SetButtonRefs(GameObject obj)
	{
		if (obj.TryGetComponent(out SettingsDialog settings))
		{
			var dataHolder = new GameObject("SettingsDialogData");
			dataHolder.transform.parent = obj.transform;
			dataHolder.AddComponent<Text>().text = settings.GenerateJson();
			UnityEngine.Object.DestroyImmediate(settings);
		}
	}


	[MenuItem("Assets/Convert TMP")]
	static void BuildTMP()
	{
		BuildObject(Selection.activeGameObject, Selection.activeGameObject.name);
	}

	private static void BuildObject(GameObject original, string name)
	{
		var obj = Instantiate(original, original.transform.parent);
		SetButtonRefs(obj);
		obj.name = name + "_tmpconverted";
		var tmps = obj.GetComponentsInChildren<TextMeshProUGUI>(true);

		if (obj.transform.parent == null)
		{
			Debug.Log("Missing parent!");
			return;
		}

		//var entries = GenerateStringEntries(original);

		var textCmp = obj.transform.parent.gameObject.GetComponent<Text>();

		if (textCmp == null)
		{
			textCmp = obj.transform.parent.gameObject.AddComponent<Text>();
		}

		//textCmp.text = entries;

		foreach (TextMeshProUGUI tmp in tmps)
		{
			var rect = tmp.GetComponent<RectTransform>();
			var path = GetGameObjectPath(tmp.gameObject, obj.transform.parent.gameObject);

			var settings = new TMPSettings
			{
				Alignment = tmp.alignment,
				AutoSizeTextContainer = tmp.autoSizeTextContainer,
				Content = path,
				EnableWordWrapping = tmp.enableWordWrapping,
				FontSize = tmp.fontSize,
				FontStyle = tmp.fontStyle,
				MaxVisibleLines = tmp.maxVisibleLines,
				Font = tmp.font.name,
				X = rect.sizeDelta.x,
				Y = rect.sizeDelta.y,
				Color = new[] { tmp.color.r, tmp.color.g, tmp.color.b }
			};


			string jsonData;
			jsonData = JsonConvert.SerializeObject(settings, Formatting.Indented);

			var parent = tmp.gameObject;
			DestroyImmediate(tmp);

			var extra = parent.GetComponent<LocTextKeyOverride>();
			if (extra != null)
			{
				DestroyImmediate(extra);
			}

			var text = parent.AddComponent<Text>();
			text.text = jsonData;
		}

		var prefabName = obj.transform.parent.gameObject.TryGetComponent(out ModInfo modInfo)
			? $"Assets/generated_assets/tmp converted ui/{modInfo.prefabPath}.prefab"
			: $"Assets/generated_assets/tmp converted ui/{obj.name}.prefab";

		//PrefabUtility.SaveAsPrefabAsset(obj, prefabName);
		//DestroyImmediate(obj);
	}
}
