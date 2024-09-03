using UnityEngine;

[ExecuteInEditMode]
public class ShaderUniformTest : MonoBehaviour
{
	[SerializeField] public Color color;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		Shader.SetGlobalColor("_Beached_TimeOfDayColor", color);
	}
}
