// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Enviroment/Nebula"
{
	Properties
	{
		_DetailMask("Detail Mask", 2D) = "black" {}
		_DetailMaskSize("Detail Mask Size", Range(1, 100)) = 50
		_DetailMaskDepth("Detail Mask Depth", Range(0, 1)) = 0.5
		_Stars("Stars", 2D) = "black" {}
		_StarsSize("Stars Size", Range(1, 100)) = 50
		_StarsDepth("Stars Depth", Range(0, 1)) = 0.5
		_NebulaBase("Nebula Base", 2D) = "black" {}
		_NebulaBaseSize("Nebula Base Size", Range(1, 100)) = 50
		_NebulaBaseDepth("Nebula Base Depth", Range(0, 1)) = 0.5
		_NebulaDetail("Nebula Detail", 2D) = "black" {}
		_NebulaDetailSize("Nebula Detail Size", Range(1, 100)) = 50
		_NebulaDetailDepth("Nebula Detail Depth", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Geometry-100"}

		Pass
		{
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
			sampler2D _NebulaBase;
			sampler2D _NebulaDetail;

			float _StarsSize;
			float _StarsDepth;
			float _DetailMaskSize;
			float _DetailMaskDepth;
			float _NebulaBaseSize;
			float _NebulaBaseDepth;
			float _NebulaDetailSize;
			float _NebulaDetailDepth;

			float2 DepthParallaxOffset(float depth, float3 viewDir) {
				return -_WorldSpaceCameraPos.xz * depth;
			}
			
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
				fixed3 detailMask = tex2D(_DetailMask, (i.uv + DepthParallaxOffset(_DetailMaskDepth, i.viewDir)) / _DetailMaskSize).rgb;
				fixed3 nebulaBase = tex2D(_NebulaBase, (i.uv + DepthParallaxOffset(_NebulaBaseDepth, i.viewDir)) / _NebulaBaseSize).rgb;
				fixed3 nebulaDetail = tex2D(_NebulaDetail, (i.uv + DepthParallaxOffset(_NebulaDetailDepth, i.viewDir)) / _NebulaDetailSize).rgb;
				fixed3 stars = tex2D(_Stars, (i.uv + DepthParallaxOffset(_StarsDepth, i.viewDir)) / _StarsSize).rgb;

				fixed3 col = nebulaBase + nebulaDetail + stars * pow(detailMask, 5) * 3;
				return fixed4(col, 1);
			}
			ENDCG
		}
	}
}
