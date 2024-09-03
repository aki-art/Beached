// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Beached_Parallax"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Distance("Distance", Range(-1, 1)) = 0.5
		_ColorOverlay("Color Overlay", Color) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "Queue" = "Transparent+500"}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Distance;
			fixed4 _ColorOverlay;

			half4 matrixPos(half4 vertexPos, half3 vertexWorld, half3 cam)
			{
				half4x4 _matrix = half4x4
					(
						1, 0, 0, mul(_Distance, (vertexWorld.x - cam.x)),
						0, 1, 0, mul(_Distance, (vertexWorld.y - cam.y)),
						0, 0, 1, 0,
						0, 0, 0, 0
					);

				return mul(_matrix, vertexPos);
			}

			v2f vert(appdata v)
			{
				v2f o;
				half4 _vertexWorld = mul(unity_ObjectToWorld, v.vertex);
				half4 _vertex = matrixPos(v.vertex, _vertexWorld, _WorldSpaceCameraPos);
				o.vertex = UnityObjectToClipPos(_vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			uniform float4 _Beached_TimeOfDayColor;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * _Beached_TimeOfDayColor;
			}
			ENDCG
		}
	}
}
