using Newtonsoft.Json;
using System;
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

	public static string GetGameObjectPath(GameObject obj, GameObject parent)
	{
		if (obj.TryGetComponent(out LocTextKeyOverride textOverride))
		{
			return textOverride.keyOverride;
		}

		string stop = parent.name;
		string prefix = "STRINGS.UI";

		if (parent.gameObject.TryGetComponent(out ModInfo modInfo))
		{
			prefix = modInfo.modName + "." + prefix;
		}

		string path = "." + obj.name.ToUpper();
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			string parentName = obj.name;
			if (parentName == stop) break;

			path = "." + parentName.ToUpper() + path;
		}
		return prefix + path;
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


			var textCmp = obj.transform.parent.gameObject.GetComponent<Text>();

			if (textCmp == null)
			{
				textCmp = obj.transform.parent.gameObject.AddComponent<Text>();
			}

			textCmp.text += path + ", " + tmp.text;

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
			? $"Assets/generated assets/tmp converted ui/{modInfo.prefabPath}.prefab"
			: $"Assets/generated assets/tmp converted ui/{obj.name}.prefab";

		PrefabUtility.SaveAsPrefabAsset(obj, prefabName);
		DestroyImmediate(obj);
	}
}
