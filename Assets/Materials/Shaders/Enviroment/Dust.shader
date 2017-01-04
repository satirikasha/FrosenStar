// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Enviroment/Dust"
{
	Properties
	{
		_DetailMask("Detail Mask", 2D) = "black" {}
		_DetailMaskSize("Detail Mask Size", Range(1, 100)) = 50
		_Stars("Stars", 2D) = "black" {}
		_StarsSize("Stars Size", Range(1, 100)) = 50
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

		Pass
		{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			sampler2D _Stars;
			sampler2D _DetailMask;

			float _StarsSize;
			float _DetailMaskSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = mul(unity_ObjectToWorld, v.vertex).xz;
				o.viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float detailMask = tex2D(_DetailMask, i.uv / _DetailMaskSize).r;
				fixed4 stars = tex2D(_Stars, i.uv / _StarsSize);
				fixed4 col = stars * pow(detailMask, 5) * 3;
				return col;
			}
			ENDCG
		}
	}
}
