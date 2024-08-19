#if DEVTOOLS
using UnityEngine;

namespace Beached.ModDevTools
{
	public abstract class ShaderPropertyInfo
	{
	}

	public class ShaderPropertyInfo<T> : ShaderPropertyInfo
	{
		public string propertyKey;
		public float minValue;
		public float maxValue;
		public string format;
		public T newValue;
		public T value;
		private Material material;

		public ShaderPropertyInfo(Material material, string propertyKey, T defaultValue, float minValue = -1, float maxValue = -1, string format = null)
		{
			this.propertyKey = propertyKey;
			this.minValue = minValue;
			this.maxValue = maxValue;
			value = newValue = defaultValue;
			this.material = material;
			this.format = format;
		}

		public void RefreshValue()
		{
			if (!newValue.Equals(value))
			{
				if (newValue is int i)
					material.SetInt(propertyKey, i);
				else if (newValue is float f)
					material.SetFloat(propertyKey, f);
				else if (newValue is Color c)
					material.SetColor(propertyKey, c);

				value = newValue;
			}
		}
	}

}
#endif