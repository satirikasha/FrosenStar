// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Enviroment/Nebula(Planets)_test"
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

		_Stars2("Stars2", 2D) = "black" {}
		_Stars2Size("Stars2 Size", Range(1, 100)) = 50
		_Stars2Depth("Stars2 Depth", Range(0, 1)) = 0.5

		_DetailMask2("Detail Mask", 2D) = "black" {}
		_DetailMask2Size("Detail Mask2 Size", Range(1, 100)) = 50
		_DetailMask2Depth("Detail Mask2 Depth", Range(0, 1)) = 0.5

		_NebulaSolid("NebulaDetailSolid", 2D) = "black" {}
		_NebulaSolidSize("Nebula Solid Size", Range(0, 100)) = 50
		_NebulaSolidDepth("Nebula Solid Depth", Range(0, 1)) = 0.5

		_TriplanarBlendSharpness("TriplanarBlendSharpness", Range(1, 100)) = 50
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			sampler2D _Stars;
			sampler2D _DetailMask;
			sampler2D _NebulaBase;
			sampler2D _NebulaDetail;

			sampler2D _NebulaSolid;
			sampler2D _Stars2;
			sampler2D _DetailMask2;

			float _StarsSize;
			float _StarsDepth;
			float _DetailMaskSize;
			float _DetailMaskDepth;
			float _NebulaBaseSize;
			float _NebulaBaseDepth;
			float _NebulaDetailSize;
			float _NebulaDetailDepth;

			float _Stars2Size;
			float _Stars2Depth;
			float _NebulaSolidSize;
			float _NebulaSolidDepth;
			float _DetailMask2Size;
			float _DetailMask2Depth;

			float _TriplanarBlendSharpness;

			float2 DepthParallaxOffset(float depth, float3 viewDir) {
				return -_WorldSpaceCameraPos.xyz * depth;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target

			{
				//_DetailMask
			//	half2 yUV = i.worldPos.xz / _DetailMaskSize;
			//	half2 xUV = i.worldPos.zy / _DetailMaskSize;
			//	half2 zUV = i.worldPos.xy / _DetailMaskSize;
				// Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
			//	half3 yDiff = tex2D(_DetailMask, yUV);
			//	half3 xDiff = tex2D(_DetailMask, xUV);
			//	half3 zDiff = tex2D(_DetailMask, zUV);

				half2 yUV = i.worldPos.xz / _DetailMaskSize;
				half2 xUV = i.worldPos.zy / _DetailMaskSize;
				half2 zUV = i.worldPos.xy / _DetailMaskSize;
				// Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
				half3 yDiff = tex2D(_DetailMask, yUV);
				half3 xDiff = tex2D(_DetailMask, xUV);
				half3 zDiff = tex2D(_DetailMask, zUV);




				half3 blendWeights = pow(abs(i.worldNormal), _TriplanarBlendSharpness);
				// Divide our blend mask by the sum of it's components, this will make x+y+z=1
				blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
				// Finally, blend together all three samples based on the blend mask.
				fixed3 col = xDiff * blendWeights.x + yDiff * blendWeights.y + zDiff * blendWeights.z;
				return fixed4(col, 1);



			//	fixed3 detailMask = tex2D(_DetailMask, (i.uv + DepthParallaxOffset(_DetailMaskDepth, i.viewDir)) / _DetailMaskSize).rgb;
			//	fixed3 nebulaBase = tex2D(_NebulaBase, (i.uv + DepthParallaxOffset(_NebulaBaseDepth, i.viewDir)) / _NebulaBaseSize).rgb;
			//	fixed3 nebulaDetail = tex2D(_NebulaDetail, (i.uv + DepthParallaxOffset(_NebulaDetailDepth, i.viewDir)) / _NebulaDetailSize).rgb;
			//	fixed3 stars = tex2D(_Stars, (i.uv + DepthParallaxOffset(_StarsDepth, i.viewDir)) / _StarsSize).rgb;

			//	fixed3 nebulaSolid = tex2D(_NebulaSolid, (i.uv + DepthParallaxOffset(_NebulaSolidDepth, i.viewDir)) / _NebulaSolidSize).rgb;
			//	fixed3 stars2 = tex2D(_Stars2, (i.uv + DepthParallaxOffset(_Stars2Depth, i.viewDir)) / _Stars2Size).rgb;
			//	fixed3 detailMask2 = tex2D(_DetailMask2, (i.uv + DepthParallaxOffset(_DetailMask2Depth, i.viewDir)) / _DetailMask2Size).rgb;
				//return fixed4(pow(detailMask2, 3) * 3, 1);
			//	fixed3 col = nebulaSolid + stars2 *detailMask2 *1.5 + nebulaBase + nebulaDetail + stars * pow(detailMask, 4) * 2;
				return fixed4(col, 1);
			}
			ENDCG
		}
	}
}


