Shader "Hidden/DepthSampler" {
    SubShader {
            Tags{ "RenderType" = "Opaque" }
            Pass{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Current2DLightRadius;

            struct v2f {
                 float4 pos : SV_POSITION;
                 float depth : DEPTH;
            };

            v2f vert (appdata_base v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z / _Current2DLightRadius;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                float depth = 1 - i.depth;
                depth *= 3;
                return fixed4(clamp(depth, 0, 1), clamp (depth, 1, 2) - 1, clamp (depth, 2, 3) - 2, 1);
            }
            ENDCG
            }
            }
        }