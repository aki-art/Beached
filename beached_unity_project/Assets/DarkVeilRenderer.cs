using System.IO;
using UnityEngine;

[ExecuteAlways]
public class DarkVeilRenderer : MonoBehaviour
{
	public Material material;
	public Texture2D originalTexture;
	public RenderTexture renderTexture;

	public bool update;
	public bool saveImage;

	[ExecuteAlways]
	private void Start()
	{
		renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0);
		GetComponent<MeshRenderer>().materials[0].SetTexture("_ZoneTypes", renderTexture);
	}


	[ExecuteAlways]
	private void LateUpdate()
	{
		if (renderTexture == null)
			renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0);

		if (material != null)
			Graphics.Blit(originalTexture, renderTexture, material, 0);

		if (update)
		{
			GetComponent<MeshRenderer>().materials[0].SetTexture("_ZoneTypes", renderTexture);
			update = false;
		}

		if (saveImage)
		{
			var path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "test_image.png");
			SaveImage(renderTexture, path);
			Debug.Log("saved image to " + path);
			Application.OpenURL(path);

			saveImage = false;
		}
	}


	public static void SaveImage(Texture textureToWrite, string path)
	{
		var texture2D = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.RGBA32, false);

		var renderTexture = new RenderTexture(textureToWrite.width, textureToWrite.height, 32);
		Graphics.Blit(textureToWrite, renderTexture);

		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture2D.Apply();

		var bytes = texture2D.EncodeToPNG();

		File.WriteAllBytes(path, bytes);
	}
}

