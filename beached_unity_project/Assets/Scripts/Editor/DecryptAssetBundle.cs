using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{

	// https://discussions.unity.com/t/load-asset-bundle-in-editor/21428/3
	public class DecryptAssetBundle : EditorWindow
	{
		public string assetURL = "";
		public string assetPath = "";
		private static WWW request;

		private static bool run = false;
		private static IEnumerator en = null;

		[MenuItem("Assets/Decrypt Asset Bundle")]
		static void Init()
		{
			DecryptAssetBundle window = GetWindow(typeof(DecryptAssetBundle)) as DecryptAssetBundle;
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
		}

		void OnGUI()
		{
			assetURL = EditorGUILayout.TextField("Asset bundle URL: ", assetURL);
			assetPath = EditorGUILayout.TextField("Asset Path: ", assetPath);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear"))
				assetURL = "";

			if (GUILayout.Button("Decrypt"))
				run = true;

			if (GUILayout.Button("Abort"))
				run = false;

			GUILayout.EndHorizontal();
		}

		public void Update()
		{
			if (!run)
			{
				if (en != null)
					en = null;

				return;
			}

			en ??= LoadAsset(assetURL);

			if (!en.MoveNext())
				run = false;
		}

		private IEnumerator LoadAsset(string s)
		{
			Debug.Log("Loading " + s + " ...");
			request = new WWW(s);
			while (!request.isDone)
				// avoid freezing the editor:
				yield return ""; // just wait.
								 // I don't like the following line because it will freeze up 
								 // your editor.  So I commented it out.  
								 // yield return request;

			if (request.error != null)
				Debug.LogError(request.error);

			GameObject g = request.assetBundle.LoadAsset<GameObject>(assetPath);

			if (g == null)
				Debug.LogWarning("Null prefab");
			else
				Instantiate(g);
		}

	}
}