using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
	Vector3 dragOrigin;
	Camera targetCamera;

	public float zoomStep = 3f;

	// Start is called before the first frame update
	void Start()
	{
		targetCamera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(2))
		{
			dragOrigin = targetCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetMouseButton(2))
		{
			var diff = dragOrigin - targetCamera.ScreenToWorldPoint(Input.mousePosition);
			targetCamera.transform.position += diff; // * speed;
		}

		if (Input.mouseScrollDelta.y > 0)
			targetCamera.orthographicSize -= zoomStep;
		else if (Input.mouseScrollDelta.y < 0)
			targetCamera.orthographicSize += zoomStep;
	}

}
