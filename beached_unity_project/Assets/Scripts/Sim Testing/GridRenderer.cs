using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
	public bool dirty;
	private SpriteRenderer[] spriteRenderer;
	public SpriteRenderer prefab;
	public Vector3 spacing;
	public static HashSet<int> dirtyCells = new HashSet<int>();

	void Start()
	{
		dirty = true;
		spriteRenderer = new SpriteRenderer[Grid.WidthInCells * Grid.HeightInCells];
		for (int x = 0; x < Grid.WidthInCells; x++)
		{
			for (int y = 0; y < Grid.HeightInCells; y++)
			{
				var r = Instantiate(prefab);
				r.transform.position = new Vector3(spacing.x * x, spacing.y * y);
				r.gameObject.SetActive(true);
				r.transform.SetParent(transform);

				spriteRenderer[x * Grid.WidthInCells + y] = r;
			}
		}

	}

	public void SetColor(int idx, short element)
	{
		spriteRenderer[idx].color = Elements.elements[element].color;
	}

	public void MarkDirty(int cell)
	{
		dirtyCells.Add(cell);
		dirty = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (!dirty)
			return;

		foreach (var cell in dirtyCells)
		{
			spriteRenderer[cell].color = Elements.elements[Grid.ElementIdx[cell]].color;
		}

		dirtyCells.Clear();
		dirty = false;
	}
}
