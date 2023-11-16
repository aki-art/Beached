using UnityEngine;

[ExecuteAlways]
public class ShaderSettings : MonoBehaviour
{
	public new Camera camera;

	void Start()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		Shader.SetGlobalVector("_WorldCameraPos", new Vector4(transform.position.x, transform.position.y, transform.position.z, camera.orthographicSize));
		//Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector3.x, vector3.y, 0.0f, 0.0f));
	}
}
