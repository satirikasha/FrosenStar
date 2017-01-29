Shader "Hidden/LightShaftSampler"
{
	Properties
	{
        _Color ("Light Color", Color) = (1,1,1,1)
		_DepthTex ("Depth Texture", 2D) = "black" {}
	}
	SubShader
	{
        Tags { "Queue" = "Geometry-1" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
        ZTest Off
        Blend SrcAlpha One

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
				float4 vertex : SV_POSITION;
                float3 localPos : TEXCOORD1;
			};

            float4 _Color;
			int _Depth;
			sampler2D _DepthTex;
			float4 _DepthTex_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.localPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                float dist = distance (0, i.localPos);
                int blur = 2;
                float falloff = (1 - dist) * (1 - dist);
				float shadowCoeff = 0.25;
				float lightMask = 0;
				for (int c = 0; c < _Depth; c++) {
                    //[loop]
                    for (int b = -blur; b <= blur; b++) {
					    fixed4 depthTex = tex2D(_DepthTex, float2(((i.localPos.x + b * _DepthTex_TexelSize.x) / i.localPos.z + 1) / 2, _DepthTex_TexelSize.y * c));
					    float depth = (depthTex.r + depthTex.g + depthTex.b) / 3;
					    lightMask += lerp(falloff * shadowCoeff, falloff, (1 - i.localPos.z) > depth - 0.005);
                    }
				} 
				lightMask /= _Depth * (blur * 2 + 1);

                return  lightMask * _Color;
			}
			ENDCG
		}
	}
}
