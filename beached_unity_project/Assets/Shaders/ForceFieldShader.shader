// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Beached/ForceFieldShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _Color ("Color", Color) = (0.5, 0.5, 1.0, 1.0)
        _RimStrength ("Rim Strength", float) = 1.0
        _DisplacementStrength("Displacement Strenfth", float) = 1.0
        _WorbleSpeed("Worble Speed", float) = 0.2
        _Alpha("Alpha", float) = 0.2
        _NoiseScale("Noise Scale", float) = 0.2
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        Pass
        {
            Blend One One
            Lighting Off
            ZWrite Off
            //Blend One OneMinusSrcAlpha
            //Cull Off 
            //ColorMask A

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 noiseUv : TEXCOORD1;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 noiseUv : TEXCOORD1;
				float4 posWorld : TEXCOORD2;
				float3 normalDir : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float4 _Color;
            float _RimStrength;
            float _DisplacementStrength;
            float _WorbleSpeed;
            float _Alpha;
            float _NoiseScale;

            float2 rotation(float2 uv, float2 center, float angle) 
            {
                float2x2 rot = float2x2(
                    float2(cos(angle), -sin(angle)),
                    float2(sin(angle), cos(angle))
                );

                uv -= center;
                uv = mul(uv, rot);
                uv += center;

                return uv;
            }

            float2 remap(float2 value, float i_min, float i_max, float o_min, float o_max) {
                return float2(
                    o_min + ((value.x - i_min) / (i_max - i_min) * (o_max - o_min)),
                    o_min + ((value.y - i_min) / (i_max - i_min) * (o_max - o_min))
                );
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);

				float3 normal = normalize( mul( float4( v.normal, 0.0 ), unity_WorldToObject ).xyz);
                o.normalDir = normal;
                float2 normalUv = remap(normal.xy, -1, 1, 0, 1);
                normal = float3(rotation(normal.xy, float2(0.5, 0), _Time * _WorbleSpeed), 0);

                float4 noise =  tex2Dlod (_NoiseTex, float4(normal.xy * _NoiseScale, 0, 0));
                o.vertex += (noise * noise * noise * _DisplacementStrength);
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
                half rim = 1 - saturate(dot(normalize(viewDirection), i.normalDir));
                
                float a = tex2D(_MainTex, i.uv).a;
                fixed4 tex = fixed4(a, a, a, 1.0);
                fixed4 col = tex * pow(rim, - _RimStrength + 1) * _Alpha;

                //fixed4 noise =  tex2D(_NoiseTex, i.posWorld.xy * .2);
                fixed4 noise =  tex2D(_NoiseTex, i.normalDir.xy);
                //fixed4 col = tex2D(_MainTex, i.uv);
                //float4 offset = fixed4(i.normalDir.xyz, 1.0);
                //offset *= _SinTime * noise;
                //return offset;

                //return noise;
                return col * _Color;// * fresnel(1.0, );
            }


            // float fresnel(float power, float3 normal, float3 view) 
            // {
            //     return pow(1.0 - clamp(dot(normal, view), 0.0, 1.0), power);
            // }
            ENDCG
        }
    }
}
