Shader "Beached/LiquidRefraction" {
    Properties {
        _WaveSpeed ("Wave Speed", float) = 0.01
        _WaveAmplitude ("Amplitude", float) = 1
        _WaveFrequency ("Frequency", float) = 1
        _LiquidTex("Liquid Texture", 2D) = "white" {}
        _RenderedLiquid("Rendered Liquid", 2D) = "white" {}
        _BlendAlpha("Alpha", Range(0, 1)) = 0.6
        _LiquidAlphaTreshold("Alpha", Range(0, 1)) = 0.6
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0

            uniform sampler2D _GrabTexture;
            uniform sampler2D _LiquidTex;
            
            sampler2D _MainTex; float4 _MainTex_ST;
            sampler2D _RenderedLiquid;
            
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD2;
                float2 displacementUV : TEXCOORD1;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0, _MainTex;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            
            uniform float3 _Position;
            uniform float4 _WorldCameraPos;

            float _WaveSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _BlendAlpha;
            
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                
                float4 liquid = tex2D(_LiquidTex, i.uv0);

                fixed2 disPos;
                disPos.x = cos(_Time * _WaveSpeed + (i.uv0.x + i.uv0.y) * _WaveFrequency);
                disPos.y = sin(_Time * _WaveSpeed + (i.uv0.x + i.uv0.y) * _WaveFrequency);

                // TODO: the wave is inconsistent with zoom (orto camera size) level
                float2 sceneUVs = i.projPos + disPos * _WaveAmplitude;// * (1 /_WorldCameraPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                
                float4 renderedLiquid = tex2D(_RenderedLiquid, i.projPos);
                fixed a = renderedLiquid.a == 1 ? 0 : 1;

                float4 finalColor = fixed4(sceneColor.rgb, a * ceil(liquid.a)); 
                
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}