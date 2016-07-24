Shader "Enviroment/Atmosphere"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_AmbientPower("Ambient Power", Range(0,10)) = 5
		_LitPower("Lit Power", Range(0,10)) = 3
		_AttenPower("Light Boost", Range(0.01,1)) = 0.25
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Cull Front
			Blend SrcAlpha One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			fixed4 _Color;
			fixed _AmbientPower;
			fixed _LitPower;
			fixed _AttenPower;

			float4 _LightColor0;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 position : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			float Fresnel(float3 viewDir, float3 worldNormal) {
				return saturate(dot(normalize(viewDir), normalize(worldNormal)));
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.position = mul(_Object2World, v.vertex);
				o.normal = normalize(mul(float4(v.normal, 0), _World2Object).xyz);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normalDir = i.normal;
				float3 viewDir = _WorldSpaceCameraPos.xyz - i.position.xyz;

				//enable lighting for point lights only
				float3 frag2LightSource = _WorldSpaceLightPos0.xyz - float3(0,2,0) - i.position.xyz;
				float pointAtten = pow(1 / length(frag2LightSource), _AttenPower);
				float3 pointLightDir = normalize(frag2LightSource);

				float atten = lerp(0, pointAtten, _WorldSpaceLightPos0.w);
				float3 lightDir = lerp(0, pointLightDir, _WorldSpaceLightPos0.w);

				float diffusePower = atten * saturate(dot(normalDir, lightDir));
				float rim = Fresnel(viewDir, -i.normal);

				fixed4 col = _Color;
				col *= pow(rim, _AmbientPower) + diffusePower * pow(rim, _LitPower);
				return col;
			}
			ENDCG
		}

		Pass
			{
				Tags{ "LightMode" = "ForwardAdd" }
				Cull Front
				Blend SrcAlpha One
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				fixed4 _Color;
				fixed _AmbientPower;
				fixed _LitPower;
				fixed _AttenPower;

				float4 _LightColor0;

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 position : TEXCOORD0;
					float3 normal : TEXCOORD1;
				};

				float Fresnel(float3 viewDir, float3 worldNormal) {
					return saturate(dot(normalize(viewDir), normalize(worldNormal)));
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.position = mul(_Object2World, v.vertex);
					o.normal = normalize(mul(float4(v.normal, 0), _World2Object).xyz);
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 normalDir = i.normal;
					float3 viewDir = _WorldSpaceCameraPos.xyz - i.position.xyz;

					//enable lighting for point lights only
					float3 frag2LightSource = _WorldSpaceLightPos0.xyz - float3(0,2,0) - i.position.xyz;
					float pointAtten = pow(1 / length(frag2LightSource), _AttenPower);
					float3 pointLightDir = normalize(frag2LightSource);

					float atten = lerp(0, pointAtten, _WorldSpaceLightPos0.w);
					float3 lightDir = lerp(0, pointLightDir, _WorldSpaceLightPos0.w);
					
					float diffusePower = atten * saturate(dot(normalDir, lightDir));
					float rim = Fresnel(viewDir, -i.normal);

					fixed4 col = _Color;
					col *= diffusePower * pow(rim, _LitPower);
					return col;
				}
				ENDCG
			}
	}
}
