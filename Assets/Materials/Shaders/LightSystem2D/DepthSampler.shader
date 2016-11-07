Shader "Hidden/DepthSampler" {
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
            Tags{ "RenderType" = "Opaque" }
            Pass{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct v2f {
                 float4 pos : SV_POSITION;
                 float2 depth : DEPTH;
            };

            v2f vert (appdata_base v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.depth = -mul(UNITY_MATRIX_MV, v.vertex) * _ProjectionParams.w / 0.414;
                return o;
            }

            half4 frag (v2f i) : SV_Target{
                float depth = (1 - i.depth);
                return fixed4(depth, depth, depth, 1);
            }
            ENDCG
            }
            }
        }