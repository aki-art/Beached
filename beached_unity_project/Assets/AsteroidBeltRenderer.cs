using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AsteroidBeltRenderer : Graphic
{
	public float scale;
	public int asteroidCount;

	public bool setDirty;

	public void Update()
	{
		if (setDirty)
			SetVerticesDirty();

		setDirty = false;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		Debug.Log("on populate mesh");

		Vector2 corner1 = Vector2.zero;
		Vector2 corner2 = Vector2.zero;

		corner1.x = 0f;
		corner1.y = 0f;
		corner2.x = 1f;
		corner2.y = 1f;

		corner1.x -= rectTransform.pivot.x;
		corner1.y -= rectTransform.pivot.y;
		corner2.x -= rectTransform.pivot.x;
		corner2.y -= rectTransform.pivot.y;

		corner1.x *= rectTransform.rect.width;
		corner1.y *= rectTransform.rect.height;
		corner2.x *= rectTransform.rect.width;
		corner2.y *= rectTransform.rect.height;

		vh.Clear();

		UIVertex vert = UIVertex.simpleVert;

		vert.position = new Vector2(corner1.x, corner1.y);
		vert.color = color;
		vh.AddVert(vert);

		vert.position = new Vector2(corner1.x, corner2.y);
		vert.color = color;
		vh.AddVert(vert);

		vert.position = new Vector2(corner2.x, corner2.y);
		vert.color = color;
		vh.AddVert(vert);

		vert.position = new Vector2(corner2.x, corner1.y);
		vert.color = color;
		vh.AddVert(vert);

		vh.AddTriangle(0, 1, 2);
		vh.AddTriangle(2, 3, 0);
	}
}
