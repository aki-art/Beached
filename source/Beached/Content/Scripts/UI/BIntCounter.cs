using FUtility.FUI;
using System;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	public class BIntCounter : KMonoBehaviour
	{
		[SerializeField] public FButton left;
		[SerializeField] public FButton right;
		[SerializeField] public FInputField2 inputField;

		public Action<int> onValueChanged;

		private int value;
		private bool initialized;

		[SerializeField] public int minimum;
		[SerializeField] public int maximum;
		[SerializeField] public int defaultValue;

		public int Value
		{
			set
			{
				if (value < minimum || value > maximum)
					return;

				this.value = value;
				inputField.Text = value.ToString();
				onValueChanged?.Invoke(value);
			}

			get => value;
		}

		public override void OnPrefabInit()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (inputField == null)
			{
				inputField = transform.Find("InputField").gameObject.AddComponent<FInputField2>();
				left = transform.Find("Left1").gameObject.AddComponent<FButton>();
				right = transform.Find("Right1").gameObject.AddComponent<FButton>();
			}

			if (!initialized)
			{
				left.OnClick += () => Value--;
				right.OnClick += () => Value++;
				inputField.OnValueChanged.AddListener(str =>
				{
					if (int.TryParse(str, out var num) && num >= minimum && num <= maximum)
						value = num;
				});

				initialized = true;
			}
		}
	}
}
