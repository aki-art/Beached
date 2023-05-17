Shader "Beached/ForceField"
{
    Properties
    {
        _MainTex ("Grid", 2D) = "white" {}
        _BlurTex ("Blurmap", 2D) = "white" {}
        _ScrollSpeed ("ScrollSpeed", float) = 1
        _BlurSize ("Blur", float) = 1
        _AlphaMultiplier ("Alpha Multiplier", float) = 28.87
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Blend One One
        ZWrite Off

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
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BlurTex;
            float4 _MainTex_ST;
            float4 _BlurTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _BlurTex);
                return o;
            }

            float _ScrollSpeed;
            float _BlurSize;
            float _AlphaMultiplier;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 scrolledUV = i.uv;
                fixed xScrollValue = _ScrollSpeed * _Time;
                scrolledUV += fixed2(0, xScrollValue);
                
				fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);
                float blursize = _BlurSize * tex2D(_BlurTex, i.uv2).r;

				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y - 4.0 * blursize)) * 0.05;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y - 3.0 * blursize)) * 0.09;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y - 2.0 * blursize)) * 0.12;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y - blursize)) * 0.15;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y)) * 0.16;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y + blursize)) * 0.15;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y + 2.0 * blursize)) * 0.12;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y + 3.0 * blursize)) * 0.09;
				sum += tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y + 4.0 * blursize)) * 0.05;

                sum *= blursize * _AlphaMultiplier;
                //fixed4 col = tex2D(_MainTex, scrolledUV);
                return sum;

                //return tex2D(_BlurTex, i.uv2);
            }
            ENDCG
        }
    }
}
