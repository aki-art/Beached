using System.Threading.Tasks;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public static int WidthInCells = 128;
	public static int HeightInCells = 128;
	public static int CellCount => WidthInCells * HeightInCells;

	public static short[] ElementIdx;

	public float updateS = 0.02f;
	public float elapsedTime = 0;
	public bool initialized;

	private FastNoiseLite noise;

	GridRenderer gridRenderer;

	void Start()
	{
		gridRenderer = GetComponent<GridRenderer>();
		ElementIdx = new short[CellCount];

		noise = new FastNoiseLite();
		noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
		Generate();
	}

	private void Generate()
	{
		noise.SetSeed(Random.Range(0, int.MaxValue));
		for (int i = 0; i < CellCount; i++)
		{
			short element = Elements.VACUUM;

			var value = noise.GetNoise(CellRow(i), CellColumn(i));

			if (value >= 0.6f) element = Elements.VACUUM;
			else if (value >= 0.35f) element = Elements.WATER;
			else if (value >= 0.1f) element = Elements.SAND;
			else if (value >= 0.1f) element = Elements.DIRT;
			else if (value >= 0f) element = Elements.NEUTRONiUM;

			SetElement(i, element);
		}

		for (int i = 0; i < HeightInCells; i++)
		{
			SetElement(i * WidthInCells, Elements.NEUTRONiUM);
			SetElement(i * WidthInCells + HeightInCells - 1, Elements.NEUTRONiUM);
		}

		for (int i = 0; i < HeightInCells; i++)
		{
			SetElement(i, Elements.NEUTRONiUM);
			SetElement((WidthInCells - 1) * WidthInCells + i, Elements.NEUTRONiUM);
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Generate();
		}
	}

	public static int CellRow(int cell) => cell / WidthInCells;

	public static int CellColumn(int cell) => cell % WidthInCells;

	public void SetElement(int index, short element)
	{
		ElementIdx[index] = element;
		gridRenderer.MarkDirty(index);
	}

	void FixedUpdate()
	{
		elapsedTime += Time.fixedDeltaTime;
		if (elapsedTime > updateS)
		{
			UpdateSim();
			elapsedTime = 0;
		}
	}

	public static int CellAbove(int cell) => cell + Grid.WidthInCells;

	public static int CellBelow(int cell) => cell - Grid.WidthInCells;

	public static int CellLeft(int cell) => cell % Grid.WidthInCells <= 0 ? -1 : cell - 1;

	public static int CellRight(int cell) => cell % Grid.WidthInCells >= Grid.WidthInCells - 1 ? -1 : cell + 1;

	private void UpdateSim()
	{
		//Parallel.For(0, CellCount, UpdateCell);

		for (int i = CellCount - 1; i >= 0; i--)
		{
			UpdateCell(i, null);
		}
	}

	public static bool IsValidCell(int cell) => cell >= 0 && cell < Grid.CellCount;


	private void UpdateCell(int cell, ParallelLoopState state)
	{
		var elementIdx = ElementIdx[cell];
		var element = Elements.elements[elementIdx];

		if (element.state == Elements.ElementData.State.Liquid)
		{

		}
		else if (element.gravity)
		{
			var cellBelow = CellBelow(cell);
			if (IsValidCell(cellBelow))
			{
				var elementBelow = Elements.GetData(ElementIdx[cellBelow]);
				if (elementBelow.state != Elements.ElementData.State.Solid)
				{
					SetElement(cell, ElementIdx[cellBelow]);
					SetElement(cellBelow, elementIdx);
				}
			}
		}
	}
}
