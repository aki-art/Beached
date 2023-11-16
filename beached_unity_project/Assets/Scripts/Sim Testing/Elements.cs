using System.Collections.Generic;
using UnityEngine;

// mockup element list for testing
public class Elements : MonoBehaviour
{
	public const short
		VACUUM = 0,
		WATER = 1,
		DIRT = 2,
		ACID = 3,
		NEUTRONiUM = 4,
		SAND = 5;

	public static Dictionary<short, ElementData> elements = new Dictionary<short, ElementData>()
	{
		{
			VACUUM,
			new ElementData()
			{
				state = ElementData.State.Gas,
				hardness = 0,
				reactivity = 0,
				name = "vacuum",
				color = new Color(0, 0, 0, 0)
			}
		},
		{
			WATER,
			new ElementData()
			{
				state = ElementData.State.Liquid,
				reactivity = 0,
				name = "water",
				color = Color.blue
			}
		},
		{
			DIRT,
			new ElementData()
			{
				state = ElementData.State.Solid,
				hardness = 255,
				reactivity = 100,
				name = "dirt",
				color = new Color(0.5f, 0.5f, 0)
			}
		},
		{
			ACID,
			new ElementData()
			{
				state = ElementData.State.Solid,
				reactivity = 0,
				name = "acid",
				color = Color.green
			}
		},
		{
			NEUTRONiUM,
			new ElementData()
			{
				state = ElementData.State.Solid,
				hardness = 255,
				reactivity = 0,
				name = "neutronium",
				color = Color.black
			}
		},
		{
			SAND,
			new ElementData()
			{
				state = ElementData.State.Solid,
				hardness = 255,
				gravity = true,
				reactivity = 0,
				name = "sand",
				color = Color.yellow
			}
		},
	};

	public void Start()
	{
		foreach (var element in elements)
		{
			element.Value.idx = element.Key;
		}
	}

	public static ElementData GetData(short index)
	{
		if (elements.TryGetValue(index, out ElementData data)) return data;

		return default;
	}

	public class ElementData
	{
		public short idx;
		public string name;
		public State state;
		public float hardness;
		public float reactivity;
		public Color color;
		public bool gravity;

		public enum State
		{
			Solid,
			Liquid,
			Gas
		}
	}
}
