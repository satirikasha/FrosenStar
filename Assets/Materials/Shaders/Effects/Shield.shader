Shader "Effects/Shield"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Range(1,10)) = 1
        _FrontMask ("Front Mask", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
            Blend SrcAlpha One
            ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
            #include "Assets/Materials/Shaders/Noise/Noise.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
                float3 normal : TEXCOORD1;
                float4 localPos: TEXCOORD3;
			};

            float _Scale;
            float4 _Color;

            sampler2D _FrontMask;
			sampler2D _MainTex;  
            
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.normal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.localPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                float3 normalDir = i.normal;
                float3 viewDir = _WorldSpaceCameraPos.xyz - i.worldPos.xyz;
                float fresnel = pow (1 - saturate (dot (normalize (viewDir), normalize (normalDir))), 0.75);
                float fresnelSoft = pow(1 - saturate(dot(normalize(viewDir), normalize(normalDir))), 7);
                float frontMask = tex2D(_FrontMask, i.uv).r;
                fresnel = fresnel - fresnelSoft;
                fresnel *= Noise (i.worldPos.xyz, 5) * frontMask * 2;
                float tex = tex2D(_MainTex, i.uv * _Scale).r;
				fixed4 col = _Color * (fresnel + fresnel * tex);
				return col;
			}
			ENDCG
		}
	}
}
