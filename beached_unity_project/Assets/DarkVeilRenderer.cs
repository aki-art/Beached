using UnityEngine;

[ExecuteAlways]
public class DarkVeilRenderer : MonoBehaviour
{
	public Material material;
	public Texture2D originalTexture;
	public RenderTexture renderTexture;

	[ExecuteAlways]
	void Start()
	{
		renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0);
		GetComponent<MeshRenderer>().materials[0].SetTexture("_ZoneTypes", renderTexture);
	}


	[ExecuteAlways]
	void LateUpdate()
	{
		if (renderTexture == null)
			renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0);

		if (material != null)
			Graphics.Blit(originalTexture, renderTexture, material, 0);

		//GetComponent<MeshRenderer>().materials[0].SetTexture("_ZoneTypes", renderTexture);
	}
}
