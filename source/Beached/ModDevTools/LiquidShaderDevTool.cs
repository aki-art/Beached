/*using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.ModDevTools
{
	public class LiquidShaderDevTool : DevTool
	{
		private static Dictionary<string, ShaderPropertyInfo> refractionShaderProperties;
		private static Material mat;

		public LiquidShaderDevTool()
		{
			RequiresGameRunning = true;
			mat = ModAssets.Materials.liquidRefractionMat;
			refractionShaderProperties = new Dictionary<string, ShaderPropertyInfo>();

			RegisterRefractionShaderProperty(mat, "_WaveSpeed", 61.1f);
			RegisterRefractionShaderProperty(mat, "_BlendAlpha", 15.5f);
			RegisterRefractionShaderProperty(mat, "_WaveFrequency", 1f);
			RegisterRefractionShaderProperty(mat, "_WaveAmplitude", 0.015f);
			RegisterRefractionShaderProperty(mat, "_EdgeSize", 0.55f);
			RegisterRefractionShaderProperty(mat, "_EdgeMultiplier", 2f);
		}

		public override void RenderTo(DevPanel panel)
		{
			foreach (var prop in refractionShaderProperties.Values)
			{
				if (prop is ShaderPropertyInfo<float> floatProperty)
				{
					var speed = (floatProperty.maxValue - floatProperty.minValue) / 1000f;
					if (ImGui.DragFloat(floatProperty.propertyKey, ref floatProperty.newValue, speed, floatProperty.minValue, floatProperty.maxValue, floatProperty.format))
						floatProperty.RefreshValue();
				}
			}
		}

		private void RegisterRefractionShaderProperty(Material material, string propertyKey, float value, float minValue = -1, float maxValue = -1, string format = "%.10f")
		{
			refractionShaderProperties.Add(propertyKey, new ShaderPropertyInfo<float>(material, propertyKey, value, minValue, maxValue, format));
		}
	}
}
*/